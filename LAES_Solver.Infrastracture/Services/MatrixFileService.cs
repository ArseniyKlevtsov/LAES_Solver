using LAES_Solver.Domain.Interfaces;

namespace LAES_Solver.Infrastracture.Services;

public class MatrixFileService : IMatrixFileService
{
    private readonly string _baseDirectory;

    public MatrixFileService(string baseDirectory)
    {
        _baseDirectory = baseDirectory;
        CreateMatrixDataDirectory();
    }

    public async Task<string> InitMatrixTask(string taskKey, int rowCount)
    {
        var taskGuid = Guid.NewGuid().ToString();

        var taskDirectory = Path.Combine(_baseDirectory, "MatrixData", taskGuid);
        Directory.CreateDirectory(Path.Combine(taskDirectory, "Matrices"));
        Directory.CreateDirectory(Path.Combine(taskDirectory, "Vectors"));

        var taskKeyFilePath = Path.Combine(taskDirectory, $"{taskKey}.txt");
        await File.WriteAllTextAsync(taskKeyFilePath, taskKey);

        return taskGuid;
    }

    private void CreateMatrixDataDirectory()
    {
        var matrixDataDirectory = Path.Combine(_baseDirectory, "MatrixData");
        if (!Directory.Exists(matrixDataDirectory))
        {
            Directory.CreateDirectory(matrixDataDirectory);
        }
    }
}
