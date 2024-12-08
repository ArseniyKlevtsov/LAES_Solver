using LAES_Solver.Domain.Interfaces;
using LAES_Solver.Domain.ValueObjects;

namespace LAES_Solver.Infrastracture.Services;

public class MatrixFileService : IMatrixFileService
{
    private readonly string _baseDirectory;

    public MatrixFileService(string baseDirectory)
    {
        _baseDirectory = baseDirectory;
        CreateMatrixDataDirectory();
    }
    private void CreateMatrixDataDirectory()
    {
        var matrixDataDirectory = Path.Combine(_baseDirectory, "MatrixData");
        if (!Directory.Exists(matrixDataDirectory))
        {
            Directory.CreateDirectory(matrixDataDirectory);
        }
    }

    public async Task<string> InitMatrixTaskAsync(string taskKey, int rowCount)
    {
        var taskGuid = Guid.NewGuid().ToString();
        var taskDirectory = Path.Combine(_baseDirectory, "MatrixData", taskGuid);

        Directory.CreateDirectory(Path.Combine(taskDirectory, "Matrices", "A"));
        Directory.CreateDirectory(Path.Combine(taskDirectory, "Matrices", "L"));
        Directory.CreateDirectory(Path.Combine(taskDirectory, "Matrices", "Lt"));
        Directory.CreateDirectory(Path.Combine(taskDirectory, "Vectors"));

        var taskKeyFilePath = Path.Combine(taskDirectory, $"TaskInfo.txt");
        var content = $"TaskKey: {taskKey}\nRowCount: {rowCount}\nReceivedRows:";
        await File.WriteAllTextAsync(taskKeyFilePath, content);

        return taskGuid;
    }

    public async Task WriteRowDataAsync(string taskName, string matrixType, int rowIndex, List<double> rowData)
    {
        var matrixDirectory = Path.Combine(_baseDirectory, "MatrixData", taskName, "Matrices");

        var matrixPath = Path.Combine(matrixDirectory, matrixType);
        if (!Directory.Exists(matrixPath))
        {
            throw new DirectoryNotFoundException($"Matrix directory '{matrixType}' does not exist.");
        }

        var rowFilePath = Path.Combine(matrixPath, $"{rowIndex}.txt");
        var content = string.Join(" ", rowData);
        await File.WriteAllTextAsync(rowFilePath, content);

        var taskInfoFilePath = Path.Combine(_baseDirectory, "MatrixData", taskName, "TaskInfo.txt");
        var indexContent = $" {rowIndex}"; 
        await File.AppendAllTextAsync(taskInfoFilePath, indexContent);
    }

    public async Task<List<double>> ReadRowDataAsync(string taskName, string matrixType, int rowIndex)
    {
        var matrixDirectory = Path.Combine(_baseDirectory, "MatrixData", taskName, "Matrices");
        var rowFilePath = Path.Combine(matrixDirectory, matrixType, $"{rowIndex}.txt");

        if (!File.Exists(rowFilePath))
        {
            throw new FileNotFoundException($"Row file '{rowIndex}.txt' not found in matrix '{matrixType}'.");
        }

        var content = await File.ReadAllTextAsync(rowFilePath);
        var values = content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        var rowData = new List<double>();
        foreach (var value in values)
        {
            if (double.TryParse(value, out double number))
            {
                rowData.Add(number);
            }
            else
            {
                throw new FormatException($"Invalid number format in row file '{rowIndex}.txt'.");
            }
        }

        return rowData;
    }

    public async Task WriteVectorDataAsync(string taskName, string fileName, List<double> vectorData)
    {
        var vectorsDirectory = Path.Combine(_baseDirectory, "MatrixData", taskName, "Vectors");

        if (!Directory.Exists(vectorsDirectory))
        {
            throw new DirectoryNotFoundException($"Vectors directory does not exist.");
        }

        var vectorFilePath = Path.Combine(vectorsDirectory, $"{fileName}.txt");
        var content = string.Join(" ", vectorData);
        await File.WriteAllTextAsync(vectorFilePath, content);
    }

    public async Task<List<double>> ReadVectorDataAsync(string taskName, string fileName)
    {
        var vectorsDirectory = Path.Combine(_baseDirectory, "MatrixData", taskName, "Vectors");
        var vectorFilePath = Path.Combine(vectorsDirectory, $"{fileName}.txt");

        if (!File.Exists(vectorFilePath))
        {
            throw new FileNotFoundException($"Vector file '{fileName}.txt' not found in Vectors directory.");
        }

        var content = await File.ReadAllTextAsync(vectorFilePath);
        var values = content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        var vectorData = new List<double>();
        foreach (var value in values)
        {
            if (double.TryParse(value, out double number))
            {
                vectorData.Add(number);
            }
            else
            {
                throw new FormatException($"Invalid number format in vector file '{fileName}.txt'.");
            }
        }

        return vectorData;
    }

    public async Task<bool> ValidateTaskKeyAsync(string taskName, string taskKey)
    {
        var taskDirectory = Path.Combine(_baseDirectory, "MatrixData", taskName);
        var taskKeyFilePath = Path.Combine(taskDirectory, "TaskInfo.txt");

        if (!File.Exists(taskKeyFilePath))
        {
            throw new FileNotFoundException($"TaskInfo file not found for task '{taskName}'.");
        }

        var content = await File.ReadAllTextAsync(taskKeyFilePath);
        foreach (var line in content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (line.StartsWith("TaskKey:"))
            {
                var storedTaskKey = line.Substring("TaskKey:".Length).Trim();
                return storedTaskKey.Equals(taskKey, StringComparison.OrdinalIgnoreCase);
            }
        }

        return false;
    }

    public void DeleteTask(string taskName)
    {
        var taskDirectory = Path.Combine(_baseDirectory, "MatrixData", taskName);
        if (Directory.Exists(taskDirectory))
        {
            Directory.Delete(taskDirectory, true);
        }
        else
        {
            throw new DirectoryNotFoundException($"Task directory '{taskName}' does not exist.");
        }
    }

    public async Task<TaskInfo> GetTaskInfoAsync(string taskName)
    {
        var taskInfoFilePath = Path.Combine(_baseDirectory, "MatrixData", taskName, "TaskInfo.txt");

        if (!File.Exists(taskInfoFilePath))
        {
            throw new FileNotFoundException($"TaskInfo file not found for task '{taskName}'.");
        }

        var content = await File.ReadAllTextAsync(taskInfoFilePath);
        var taskInfo = new TaskInfo
        {
            TaskName = taskName,
            ReceivedRows = new List<int>()
        };

        foreach (var line in content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (line.StartsWith("RowCount:"))
            {
                if (int.TryParse(line.Substring("RowCount:".Length).Trim(), out int rowCount))
                {
                    taskInfo.RowCount = rowCount;
                }
            }
            else if (line.StartsWith("ReceivedRows:"))
            {
                var receivedRows = line.Substring("ReceivedRows:".Length).Trim();
                taskInfo.ReceivedRows = receivedRows.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(int.Parse)
                                                     .ToList();
            }
        }

        return taskInfo;
    }
}
