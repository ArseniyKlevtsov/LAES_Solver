namespace LAES_Solver.Application.Services;

public class LltService
{
    public List<List<double>> LLT(List<List<double>> A)
    {
        int n = A.Count;
        var L = new List<List<double>>();

        for (int i = 0; i < n; i++)
        {
            L.Add(new List<double>(new double[n]));
        }

        for (int i = 0; i < n; i++)
        {
            double sum = 0;
            for (int k = 0; k < i; k++)
            {
                sum += L[i][k] * L[i][k];
            }
            L[i][i] = Math.Sqrt(A[i][i] - sum);

            for (int j = i + 1; j < n; j++)
            {
                sum = 0;
                for (int k = 0; k < i; k++)
                {
                    sum += L[j][k] * L[i][k];
                }
                L[j][i] = (A[j][i] - sum) / L[i][i];
            }
        }
        return L;
    }

    public List<double> Solve(List<List<double>> L, List<double> b)
    {
        int n = L.Count;
        var y = new List<double>(new double[n]);
        var x = new List<double>(new double[n]);

        for (int i = 0; i < n; i++)
        {
            double sum = 0;
            for (int j = 0; j < i; j++)
            {
                sum += L[i][j] * y[j];
            }
            y[i] = (b[i] - sum) / L[i][i];
        }

        for (int i = n - 1; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < n; j++)
            {
                sum += L[j][i] * x[j];
            }
            x[i] = (y[i] - sum) / L[i][i];
        }

        return x;
    }
}