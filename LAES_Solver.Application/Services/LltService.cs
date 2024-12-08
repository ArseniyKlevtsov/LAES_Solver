namespace LAES_Solver.Application.Services;

public class LltService
{
    public double[][] LLT(double[][] A)
    {
        int n = A.Length;
        double[][] L = new double[n][];

        for (int i = 0; i < n; i++)
        {
            L[i] = new double[n];
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

    public double[] Solve(double[][] L, double[] b)
    {
        int n = L.Length;
        double[] y = new double[n];
        double[] x = new double[n];

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