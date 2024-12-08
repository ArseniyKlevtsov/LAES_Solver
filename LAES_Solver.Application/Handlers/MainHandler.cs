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
        switch (message.Command)
        {
            case "SolveSmallMatrix":
                var dto = _dtoConvertor.ConvertToDto<SolveSmallMatrixDto>(jsonData);
                response = SolveSmallMatrix.Execute(dto, _lltService);
                break;
            case "StartSendingBigMatrix":
                var startDto = _dtoConvertor.ConvertToDto<StartSendingBigMatrixDto>(jsonData);
                break;
            case "BigMatrixPart":
                var partDto = _dtoConvertor.ConvertToDto<BigMatrixPartDto>(jsonData);
                break;
            case "StopSendingBigMatrix":
                var stopDto = _dtoConvertor.ConvertToDto<StopSendingBigMatrixDto>(jsonData);
                break;
            case "StartSolving":
                var solvingDto = _dtoConvertor.ConvertToDto<MatrixTaskDto>(jsonData);
                break;
            case "GetSolution":
                var getSolutionDto = _dtoConvertor.ConvertToDto<MatrixTaskDto>(jsonData);
                break;
            default:
                response = DefaultHandler.Execute(message);
                break;
        }

        return response!;
    }

}
