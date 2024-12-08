using LAES_Solver.Application.DTOs.RequestDTOs;
using LAES_Solver.Application.Handlers.MatrixOperationHandlers;
using LAES_Solver.Application.Services;
using LAES_Solver.Domain.Interfaces.Services;
using LAES_Solver.Domain.ValueObjects;

namespace LAES_Solver.Application.Handlers;

public class MainHandler
{
    private readonly IDtoConvertor _dtoConvertor;
    private readonly LltService _lltService;

    public MainHandler(IDtoConvertor dtoConvertor, LltService lltService)
    {
        _dtoConvertor = dtoConvertor;
        _lltService = lltService;
    }

    public HandlerResponse Route(Message message, string senderId)
    {
        HandlerResponse? response = null;
        var jsonData = message.Dto.ToString();
        Console.WriteLine(jsonData);
        Console.WriteLine("jsonData");
        switch (message.Command)
        {
            case "SolveSmallMatrix":
                var dto = _dtoConvertor.ConvertToDto<SolveSmallMatrixDto>(jsonData);
                response = SolveSmallMatrix.Execute(dto, _lltService);
                break;
            case "StartSendingBigMatrix":
                ; 
                break;
            case "BigMatrixPart":
                ; 
                break;
            case "StopSendingBigMatrix":
                ; 
                break;
            case "StartSolving":
                ; 
                break;
            case "GetSolution":
                ; 
                break;
            default:
                response = DefaultHandler.Execute(message);
                break;
        }

        return response!;
    }

}
