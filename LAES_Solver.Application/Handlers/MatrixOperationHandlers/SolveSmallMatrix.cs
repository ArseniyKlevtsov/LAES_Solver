using LAES_Solver.Application.DTOs.RequestDTOs;
using LAES_Solver.Application.DTOs.ResponseDTOs;
using LAES_Solver.Application.Services;
using LAES_Solver.Domain.ValueObjects;

namespace LAES_Solver.Application.Handlers.MatrixOperationHandlers;

public static class SolveSmallMatrix
{
    public static HandlerResponse Execute(SolveSmallMatrixDto dto, LltService lltService)
    {
        Console.WriteLine("MARIXAS");
        foreach (var item in dto.Matrix)
        {
            foreach (var item1 in item)
            {
                Console.WriteLine( item1.GetType());
            }
        }
        Console.WriteLine("VectorB");
        foreach (var item in dto.VectorB)
        {
            Console.WriteLine(item.GetType());
        }

        var matrixL = lltService.LLT(dto.Matrix);

        var vectorX = lltService.Solve(matrixL, dto.VectorB);

        var responseDto = new SmallMatrixSolutionDto() { VectorX = vectorX };
        var message = new Message("SmallMatrixSolution", responseDto);
        var handlerResponse = new HandlerResponse() { SenderMessage = message };

        return handlerResponse;
    }

}
