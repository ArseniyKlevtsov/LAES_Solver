using LAES_Solver.Application.DTOs.RequestDTOs;
using LAES_Solver.Application.DTOs.ResponseDTOs;
using LAES_Solver.Domain.Interfaces;
using LAES_Solver.Domain.ValueObjects;

namespace LAES_Solver.Application.Handlers.MatrixOperationHandlers;

public static class GetSolution
{
    public static async Task<HandlerResponse> ExecuteAsync(MatrixTaskDto dto, IMatrixFileService matrixFileService)
    {
        var x = await matrixFileService.ReadVectorDataAsync(dto.TaskName, "x");
        var message = new Message("TaskSolution", new TaskSolutionDto()
        {
            TaskName = dto.TaskName,
            VectorX = x
        });
        var handlerResponse = new HandlerResponse() { SenderMessage = message };

        return handlerResponse;
    }
}
