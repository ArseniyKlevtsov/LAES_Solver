using LAES_Solver.Application.DTOs.RequestDTOs;
using LAES_Solver.Application.DTOs.ResponseDTOs;
using LAES_Solver.Domain.Interfaces;
using LAES_Solver.Domain.ValueObjects;

namespace LAES_Solver.Application.Handlers.MatrixOperationHandlers;

public static class BigMatrixPart
{
    public static async Task<HandlerResponse> ExecuteAsync(BigMatrixPartDto dto, IMatrixFileService matrixFileService)
    {
        await matrixFileService.WriteRowDataAsync(dto.TaskName, "A", dto.RowIndex, dto.RowData);
        var message = new Message("PartSaved", new PartSavedDto() 
        { 
            TaskName = dto.TaskName,
            RowIndex = dto.RowIndex,
        });
        var handlerResponse = new HandlerResponse() { SenderMessage = message };

        return handlerResponse;
    }
}
