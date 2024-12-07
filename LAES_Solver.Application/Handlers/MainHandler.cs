using LAES_Solver.Application.DTOs.RequestDTOs;
using LAES_Solver.Application.Handlers.MatrixOperationHandlers;
using LAES_Solver.Domain.Interfaces.Services;
using LAES_Solver.Domain.ValueObjects;

namespace LAES_Solver.Application.Handlers;

public class MainHandler
{
    private readonly IDtoConvertor _dtoConvertor;

    public MainHandler(IDtoConvertor dtoConvertor)
    {
        _dtoConvertor = dtoConvertor;
    }

    public HandlerResponse Route(Message message, string senderId)
    {
        HandlerResponse? response = null;
        var jsonData = message.Dto.ToString();
        switch (message.Command)
        {
            case "SolveSmallMatrix":
                var dto = _dtoConvertor.ConvertToDto<SolveSmallMatrixDto>(jsonData);
                response = SolveSmallMatrix.Execute(dto);
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
