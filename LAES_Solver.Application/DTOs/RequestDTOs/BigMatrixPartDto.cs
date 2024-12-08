namespace LAES_Solver.Application.DTOs.RequestDTOs;

public class BigMatrixPartDto
{
    public int RowIndex { get; set; } 
    public List<double> RowData { get; set; }
    public string TaskKey { get; set; }
    public string TaskName { get; set; }
}
