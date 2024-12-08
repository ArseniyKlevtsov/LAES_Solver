namespace LAES_Solver.Domain.Interfaces;

public interface IMatrixFileService
{
    Task<string> InitMatrixTaskAsync(string taskKey, int rowCount);
    Task<bool> ValidateTaskKeyAsync(string taskName, string taskKey);
    Task WriteRowDataAsync(string taskName, string matrixType, int rowIndex, List<double> rowData);
    Task<List<double>> ReadRowDataAsync(string taskName, string matrixType, int rowIndex);
    Task WriteVectorDataAsync(string taskName, string fileName, List<double> vectorData);
    Task<List<double>> ReadVectorDataAsync(string taskName, string fileName);
    void DeleteTask(string taskName);
    Task<List<int>> GetReceivedRowsAsync(string taskName);
}
