namespace LAES_Solver.Application.DTOs.ResponseDTOs;

public class TaskInfoDto
{
    public int RowCount { get; set; }
    public string TaskName { get; set; }
    public string TaskKey { get; set; }
    public List<int> ReceivedRows { get; set; }
    public bool Solved { get; set; }
}
