using LAES_Solver.Domain.Interfaces;
using LAES_Solver.Domain.ValueObjects;
using System.Text.Json;

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

    public async Task SetInfoAsync(string taskName, TaskInfo taskInfoData)
    {
        var taskDirectory = Path.Combine(_baseDirectory, "MatrixData", taskName);
        var taskInfoFilePath = Path.Combine(taskDirectory, "TaskInfo.json");

        var jsonContent = JsonSerializer.Serialize(taskInfoData, new JsonSerializerOptions { WriteIndented = true });

        await File.WriteAllTextAsync(taskInfoFilePath, jsonContent);
    }

    public async Task<string> InitMatrixTaskAsync(string taskKey, int rowCount)
    {
        var taskGuid = Guid.NewGuid().ToString();
        var taskDirectory = Path.Combine(_baseDirectory, "MatrixData", taskGuid);

        Directory.CreateDirectory(Path.Combine(taskDirectory, "Matrices", "A"));
        Directory.CreateDirectory(Path.Combine(taskDirectory, "Matrices", "L"));
        Directory.CreateDirectory(Path.Combine(taskDirectory, "Matrices", "Lt"));
        Directory.CreateDirectory(Path.Combine(taskDirectory, "Vectors"));

        var taskInfo = new TaskInfo
        {
            TaskName = taskGuid,
            TaskKey = taskKey,
            RowCount = rowCount,
            ReceivedRows = new List<int>(),
            Solved = false
        };

        var taskInfoFilePath = Path.Combine(taskDirectory, "TaskInfo.json");

        var jsonContent = JsonSerializer.Serialize(taskInfo, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(taskInfoFilePath, jsonContent);

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
        if(matrixType == "A")
        {
            await AddRowIndexAsync(taskName, rowIndex);
        }
    }

    private async Task AddRowIndexAsync(string taskName, int rowIndex)
    {
        var taskInfoFilePath = Path.Combine(_baseDirectory, "MatrixData", taskName, "TaskInfo.json");

        var taskInfo = await GetTaskInfoAsync(taskName);

        if (!taskInfo.ReceivedRows.Contains(rowIndex))
        {
            taskInfo.ReceivedRows.Add(rowIndex);

            var updatedJsonContent = JsonSerializer.Serialize(taskInfo, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(taskInfoFilePath, updatedJsonContent);
        }
        else
        {
            Console.WriteLine($"Row index {rowIndex} already exists in ReceivedRows.");
        }
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
        try
        {
            var taskInfo = await GetTaskInfoAsync(taskName);

            return taskInfo.TaskKey.Equals(taskKey, StringComparison.OrdinalIgnoreCase);
        }
        catch (FileNotFoundException)
        {
            return false;
        }
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
        var taskInfoFilePath = Path.Combine(_baseDirectory, "MatrixData", taskName, "TaskInfo.json");

        if (!File.Exists(taskInfoFilePath))
        {
            throw new FileNotFoundException($"TaskInfo file not found for task '{taskName}'.");
        }

        var jsonContent = await File.ReadAllTextAsync(taskInfoFilePath);

        var taskInfo = JsonSerializer.Deserialize<TaskInfo>(jsonContent);

        return taskInfo;
    }
}
