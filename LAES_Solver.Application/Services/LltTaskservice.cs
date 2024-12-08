
using LAES_Solver.Domain.Interfaces;

namespace LAES_Solver.Application.Services;

public class LltTaskservice
{
    private readonly IMatrixFileService matrixFileService;

    public LltTaskservice(IMatrixFileService matrixFileService)
    {
        this.matrixFileService = matrixFileService;
    }

    public async Task<List<List<double>>> LLTAsync(string taskName)
    {
        var taskInfo = await matrixFileService.GetTaskInfoAsync(taskName);
        int n = taskInfo.RowCount;

        for (int i = 0; i < n; i++)
        {
            var tempRow = CreateTempRow(n);
            var rowA = await matrixFileService.ReadRowDataAsync(taskName, "A", i);
            var rowLti = await matrixFileService.ReadRowDataAsync(taskName, "Lt", i);

            double sum = 0;
            for (int k = 0; k < i; k++)
            {
                var elem = rowLti[k];
                sum += elem * elem;
            }
            tempRow[i] = Math.Sqrt(rowA[i] - sum);

            for (int j = i + 1; j < n; j++)
            {
                var rowAj = await matrixFileService.ReadRowDataAsync(taskName, "A", j);
                var rowLtj = await matrixFileService.ReadRowDataAsync(taskName, "Lt", j);

                sum = 0;
                for (int k = 0; k < i; k++)
                {
                    sum += rowLtj[k] * rowLti[k];
                }
                tempRow[j] = (rowAj[i] - sum) / tempRow[i];
            }

            // Записываем найденные элементы в временный список Lt
            var tempLtRow = new List<double>();
            for (int k = 0; k <= i; k++)
            {
                tempLtRow.Add(L[k][i]); // Добавляем столбец L в Lt
            }
            Lt[i] = tempLtRow; // Записываем строку в Lt
        }

        // Сохраняем матрицу Lt в сервисе
        for (int i = 0; i < n; i++)
        {
            await matrixFileService.WriteRowDataAsync(taskName, "Lt", i, Lt[i]);
        }

        return L;
    }

    private List<double> CreateTempRow(int lenght)
    {
        var tempRow = new List<double>();
        for (int i = 0; i < lenght; i++)
        {
            tempRow.Add(0);
        }
        return tempRow;
    }
}
