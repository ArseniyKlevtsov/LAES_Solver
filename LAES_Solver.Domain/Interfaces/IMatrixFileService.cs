namespace LAES_Solver.Domain.Interfaces;

public interface IMatrixFileService
{
    Task<string> InitMatrixTask(string taskKey, int rowCount);
}
