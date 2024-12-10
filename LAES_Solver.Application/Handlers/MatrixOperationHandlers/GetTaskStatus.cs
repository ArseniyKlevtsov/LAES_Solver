using LAES_Solver.Application.DTOs.RequestDTOs;
using LAES_Solver.Application.DTOs.ResponseDTOs;
using LAES_Solver.Domain.Interfaces;
using LAES_Solver.Domain.ValueObjects;

namespace LAES_Solver.Application.Handlers.MatrixOperationHandlers;

public static class GetTaskStatus
{
    public static async Task<HandlerResponse> ExecuteAsync(MatrixTaskDto dto, IMatrixFileService matrixFileService)
    {
        var taskInfo = await matrixFileService.GetTaskInfoAsync(dto.TaskName);

        var message = new Message("TaskInfo", new TaskInfoDto()
        {
            TaskName = taskInfo.TaskName,
            TaskKey = taskInfo.TaskKey,
            RowCount = taskInfo.RowCount,
            ReceivedRows = taskInfo.ReceivedRows,
            Solved = taskInfo.Solved
        });
        var handlerResponse = new HandlerResponse() { SenderMessage = message };

        return handlerResponse;
    }
}
