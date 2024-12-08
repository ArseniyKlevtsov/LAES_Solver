namespace LAES_Solver.Domain.Interfaces.Services;

public interface IDtoConvertor
{
    public T ConvertToDto<T>(string? jsonData);
    public string ConvertToJson<T>(T dto);
}
