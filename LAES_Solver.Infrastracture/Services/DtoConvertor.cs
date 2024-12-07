using LAES_Solver.Domain.Exceptons;
using LAES_Solver.Domain.Interfaces.Services;
using Newtonsoft.Json;

namespace LAES_Solver.Infrastracture.Services;

public class DtoConvertor : IDtoConvertor
{
    public T ConvertToDto<T>(string? jsonData)
    {
        if (string.IsNullOrEmpty(jsonData))
        {
            throw new DtoConvertException("JSON data cannot be null or empty");
        }

        try
        {
            var dto = JsonConvert.DeserializeObject<T>(jsonData);
            if (dto == null) 
            {
                throw new DtoConvertException("JSON data cannot be null");
            }
            return dto;
        }
        catch (JsonException)
        {
            throw new DtoConvertException($"Failed to convert JSON to {typeof(T).Name}");
        }
    }
}
