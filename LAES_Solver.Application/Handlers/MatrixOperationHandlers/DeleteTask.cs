using LAES_Solver.Application.DTOs.RequestDTOs;
using LAES_Solver.Application.DTOs.ResponseDTOs;
using LAES_Solver.Domain.Interfaces;
using LAES_Solver.Domain.ValueObjects;

namespace LAES_Solver.Application.Handlers.MatrixOperationHandlers;

public static class DeleteTask
{
    public static async Task<HandlerResponse> ExecuteAsync(MatrixTaskDto dto, IMatrixFileService matrixFileService)
    {
        matrixFileService.DeleteTask(dto.TaskName);

        var message = new Message("TaskDeleted", new TaskDeletedDto()
        {
            TaskName = dto.TaskName
        });
        var handlerResponse = new HandlerResponse() { SenderMessage = message };

        return handlerResponse;
    }
}
