public static class LltTaskInRamService
{
    public static async Task<List<List<double>>> LLTAsync(List<List<double>> A)
    {
        int n = A.Count;
        var L = new List<List<double>>();

        // Инициализация матрицы L
        for (int i = 0; i < n; i++)
        {
            L.Add(new List<double>(new double[n]));
        }

        // Определяем размер задач
        int iterationsPerTask = GetIterationsPerTask(n);

        for (int i = 0; i < n; i++)
        {
            double sum = await CalculateSumForLAsync(L, A, i, iterationsPerTask);
            L[i][i] = Math.Sqrt(A[i][i] - sum);

            await FillLAsync(L, A, i, iterationsPerTask);
        }

        return L;
    }

    private static async Task<double> CalculateSumForLAsync(
        List<List<double>> L,
        List<List<double>> A,
        int i,
        int iterationsPerTask)
    {
        double sum = 0;
        for (int k = 0; k < i; k++)
        {
            sum += L[i][k] * L[i][k];
            if (k % iterationsPerTask == 0) 
            {
                await Task.Yield();
            }
        }
        return sum;
    }

    private static async Task FillLAsync(
        List<List<double>> L,
        List<List<double>> A,
        int i,
        int iterationsPerTask)
    {
        int n = A.Count;

        for (int j = i + 1; j < n; j++)
        {
            double sum = 0;
            for (int k = 0; k < i; k++)
            {
                sum += L[j][k] * L[i][k];
                if (k % iterationsPerTask == 0) // Пауза для имитации асинхронной работы
                {
                    await Task.Yield();
                }
            }
            L[j][i] = (A[j][i] - sum) / L[i][i];
        }
    }

    public static async Task<List<double>> SolveAsync(List<List<double>> L, List<double> b)
    {
        int n = L.Count;
        var y = new List<double>(new double[n]);
        var x = new List<double>(new double[n]);

        // Решение Ly = b
        for (int i = 0; i < n; i++)
        {
            double sum = 0;
            for (int j = 0; j < i; j++)
            {
                sum += L[i][j] * y[j];
            }
            y[i] = (b[i] - sum) / L[i][i];
            await Task.Yield(); // Асинхронная пауза
        }

        // Решение L^Tx = y
        for (int i = n - 1; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < n; j++)
            {
                sum += L[j][i] * x[j];
            }
            x[i] = (y[i] - sum) / L[i][i];
            await Task.Yield();
        }

        return x;
    }

    private static int GetIterationsPerTask(int size)
    {
        if (size <= 100) return 50;
        if (size <= 1000) return 10;
        return 1;
    }
}