using LAES_Solver.Application.DTOs.RequestDTOs;
using LAES_Solver.Application.DTOs.ResponseDTOs;
using LAES_Solver.Application.Services;
using LAES_Solver.Domain.Interfaces;
using LAES_Solver.Domain.ValueObjects;

namespace LAES_Solver.Application.Handlers.MatrixOperationHandlers;

public static class StartSolving
{
    public static async Task<HandlerResponse> ExecuteAsync(MatrixTaskDto dto, IMatrixFileService matrixFileService, LltTaskService lltTaskService)
    {
        await lltTaskService.LLTAsync(dto.TaskName);
        await lltTaskService.SolveAsync(dto.TaskName);

        var message = new Message("TaskSolved", new MatrixTaskDtoResponse()
        {
            TaskName = dto.TaskName,
        });
        var handlerResponse = new HandlerResponse() { SenderMessage = message };

        return handlerResponse;
    }
}
