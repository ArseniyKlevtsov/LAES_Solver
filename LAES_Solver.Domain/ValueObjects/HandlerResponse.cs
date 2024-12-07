namespace LAES_Solver.Domain.ValueObjects;

public class HandlerResponse
{
    public Message? SenderMessage { get; set; }
    public Message? RecipientMessage { get; set; }
    public List<string> RecipientIds { get; set; } = new List<string>();
}
