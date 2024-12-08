using LAES_Solver.Application.DTOs.RequestDTOs;
using LAES_Solver.Application.Handlers.MatrixOperationHandlers;
using LAES_Solver.Application.Services;
using LAES_Solver.Domain.Interfaces;
using LAES_Solver.Domain.Interfaces.Services;
using LAES_Solver.Domain.ValueObjects;

namespace LAES_Solver.Application.Handlers;

public class MainHandler
{
    private readonly IDtoConvertor dtoConvertor;
    private readonly IMatrixFileService matrixFileService;
    private readonly LltService lltService;

    public MainHandler(IDtoConvertor dtoConvertor, LltService lltService, IMatrixFileService matrixFileService)
    {
        this.dtoConvertor = dtoConvertor;
        this.lltService = lltService;
        this.matrixFileService = matrixFileService;
    }

    public async Task<HandlerResponse> Route(Message message, string senderId)
    {
        HandlerResponse? response = null;
        var jsonData = message.Dto.ToString();
        switch (message.Command)
        {
            case "SolveSmallMatrix":
                var matrixDto = dtoConvertor.ConvertToDto<SolveSmallMatrixDto>(jsonData);
                response = SolveSmallMatrix.Execute(matrixDto, lltService);
                break;

            case "StartSendingBigMatrix":
                var startDto = dtoConvertor.ConvertToDto<StartSendingBigMatrixDto>(jsonData);
                response = await StartSendingBigMatrix.ExecuteAsync(startDto, matrixFileService);
                break;

            case "BigMatrixPart":
                var partDto = dtoConvertor.ConvertToDto<BigMatrixPartDto>(jsonData);
                await MatrixTaskValidator.ValidateAsync(partDto.TaskKey, partDto.TaskName, matrixFileService);
                response = await BigMatrixPart.ExecuteAsync(partDto, matrixFileService);
                break;

            case "GetTaskStatus":
                var statusDto = dtoConvertor.ConvertToDto<MatrixTaskDto>(jsonData);
                await MatrixTaskValidator.ValidateAsync(statusDto.TaskKey, statusDto.TaskName, matrixFileService);
                response = await GetTaskStatus.ExecuteAsync(statusDto, matrixFileService);
                break;

            case "StartSolving":
                var solvingDto = dtoConvertor.ConvertToDto<MatrixTaskDto>(jsonData);
                break;

            case "GetSolution":
                var getSolutionDto = dtoConvertor.ConvertToDto<MatrixTaskDto>(jsonData);
                break;

            case "DeleteTask":
                var deleteTaskDto = dtoConvertor.ConvertToDto<MatrixTaskDto>(jsonData);
                break;

            default:
                response = DefaultHandler.Execute(message);
                break;
        }

        return response!;
    }

}
