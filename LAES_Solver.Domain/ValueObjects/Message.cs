namespace LAES_Solver.Domain.ValueObjects;

public class Message
{
    public string Command { get; set; }
    public object Dto { get; set; }

    public Message(string command, object dto)
    {
        Command = command;
        Dto = dto;
    }
}