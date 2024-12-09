
using LAES_Solver.Domain.Interfaces;

namespace LAES_Solver.Application.Services;

public class LltTaskservice
{
    private readonly IMatrixFileService matrixFileService;

    public LltTaskservice(IMatrixFileService matrixFileService)
    {
        this.matrixFileService = matrixFileService;
    }

    public async Task LLTAsync(string taskName)
    {
        var taskInfo = await matrixFileService.GetTaskInfoAsync(taskName);
        int n = taskInfo.RowCount;

        for (int i = 0; i < n; i++)
        {
            var tempRow = CreateZeroRow(n);
            double sum = 0;

            var rowAi = await matrixFileService.ReadRowDataAsync(taskName, "A", i);

            for (int k = 0; k < i; k++)
            {
                var rowLtk = await matrixFileService.ReadRowDataAsync(taskName, "Lt", k);
                var elem = rowLtk[i];
                sum += elem * elem;
            }
            tempRow[i] = Math.Sqrt(rowAi[i] - sum);

            for (int j = i + 1; j < n; j++)
            {
                sum = 0;

                for (int k = 0; k < i; k++)
                {
                    var rowLtk = await matrixFileService.ReadRowDataAsync(taskName, "Lt", k);
                    sum += rowLtk[j] * rowLtk[i];
                }
                var rowAj = await matrixFileService.ReadRowDataAsync(taskName, "A", j);
                
                tempRow[j] = (rowAj[i] - sum) / tempRow[i];
            }

            await matrixFileService.WriteRowDataAsync(taskName, "Lt", i, tempRow);
        }
    }

    private List<double> CreateZeroRow(int lenght)
    {
        var tempRow = new List<double>();
        for (int i = 0; i < lenght; i++)
        {
            tempRow.Add(0);
        }
        return tempRow;
    }

    public async Task Solve(string taskName)
    {
        var taskInfo = await matrixFileService.GetTaskInfoAsync(taskName);
        int n = taskInfo.RowCount;

        var b = await matrixFileService.ReadVectorDataAsync(taskName, "b");
        var y = CreateZeroRow(n);
        var x = CreateZeroRow(n);

        for (int i = 0; i < n; i++)
        {
            var rowLti = await matrixFileService.ReadRowDataAsync(taskName, "Lt", i);
            double sum = 0;
            for (int j = 0; j < i; j++)
            {
                var rowLtj = await matrixFileService.ReadRowDataAsync(taskName, "Lt", j);
                sum += rowLtj[i] * y[j];
            }
            y[i] = (b[i] - sum) / rowLti[i];
        }
        await matrixFileService.WriteVectorDataAsync(taskName, "y", y);

        for (int i = n - 1; i >= 0; i--)
        {
            var rowLti = await matrixFileService.ReadRowDataAsync(taskName, "Lt", i);
            double sum = 0;
            for (int j = i + 1; j < n; j++)
            {
                sum += rowLti[j] * x[j];
            }
            x[i] = (y[i] - sum) / rowLti[i];
        }
        await matrixFileService.WriteVectorDataAsync(taskName, "x", x);

        await matrixFileService.SetInfoAsync(taskName, "Solved", "solved");
    }
}
