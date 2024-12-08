namespace LAES_Solver.Application.DTOs.RequestDTOs;

public class StartSendingBigMatrixDto
{
    public List<double> VectorB {  get; set; }
    public int TotalRows { get; set; }
    public string TaskKey { get; set; }
}
