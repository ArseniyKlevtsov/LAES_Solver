using LAES_Solver.Domain.Interfaces;

namespace LAES_Solver.Application.Services;

public static class MatrixTaskValidator
{
    public static async Task ValidateAsync(string taskKey, string taskName, IMatrixFileService matrixFileService)
    {
        if (!(await matrixFileService.ValidateTaskKeyAsync(taskName, taskKey)))
        {
            throw new UnauthorizedAccessException("Access denied");
        }
    }
}
