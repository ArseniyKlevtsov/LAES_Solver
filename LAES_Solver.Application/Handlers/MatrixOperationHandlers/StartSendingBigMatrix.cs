using LAES_Solver.Application.DTOs.RequestDTOs;
using LAES_Solver.Application.DTOs.ResponseDTOs;
using LAES_Solver.Domain.Interfaces;
using LAES_Solver.Domain.ValueObjects;

namespace LAES_Solver.Application.Handlers.MatrixOperationHandlers;

public static class StartSendingBigMatrix
{
    public static async Task<HandlerResponse> ExecuteAsync(StartSendingBigMatrixDto dto, IMatrixFileService matrixFileService)
    {
        var taskName = await matrixFileService.InitMatrixTaskAsync(dto.TaskKey, dto.TotalRows);
        await matrixFileService.WriteVectorDataAsync(taskName, "b", dto.VectorB);

        var message = new Message("StartSendingAccepted", new MatrixTaskDtoResponse() 
        { 
            TaskName = taskName,
            TaskKey = dto.TaskKey,
            RowCount = dto.TotalRows
        });
        var handlerResponse = new HandlerResponse() { SenderMessage = message };

        return handlerResponse;
    }
}
