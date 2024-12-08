namespace LAES_Solver.Application.DTOs.RequestDTOs;

public class SolveSmallMatrixDto
{
    public List<List<double>> Matrix { get; set; }
    public List<double> VectorB { get; set; }
}