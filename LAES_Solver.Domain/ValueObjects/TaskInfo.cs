namespace LAES_Solver.Domain.ValueObjects;

public class TaskInfo
{
    public int RowCount { get; set; }
    public string TaskName { get; set; }
    public List<int> ReceivedRows { get; set; }
}
