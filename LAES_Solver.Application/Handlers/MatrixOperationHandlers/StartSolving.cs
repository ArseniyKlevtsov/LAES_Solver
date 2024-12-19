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
        // you can customize the selection of methods
        // for example: n<1000 => in ram, else in files

        //await SolveInFiles(dto.TaskName, lltTaskService);
        await SolveInRam(dto.TaskName, matrixFileService);

        var message = new Message("TaskSolved", new MatrixTaskDtoResponse()
        {
            TaskName = dto.TaskName,
        });
        var handlerResponse = new HandlerResponse() { SenderMessage = message };

        return handlerResponse;
    }

    private static async Task SolveInRam(string taskName, IMatrixFileService matrixFileService)
    {
        var matrixA = await LoadA(taskName, matrixFileService);
        var vectorB = await matrixFileService.ReadVectorDataAsync(taskName, "b");

        var matrixL = await LltTaskInRamService.LLTAsync(matrixA);
        matrixA = null;

        var vectorX = await LltTaskInRamService.SolveAsync(matrixL, vectorB);
        await matrixFileService.WriteVectorDataAsync(taskName, "x", vectorX);

        var taskInfo = await matrixFileService.GetTaskInfoAsync(taskName);
        taskInfo.Solved = true;
        await matrixFileService.SetInfoAsync(taskName, taskInfo);
    }

    private static async Task<List<List<double>>> LoadA(string taskName, IMatrixFileService matrixFileService)
    {
        var taskInfo = await matrixFileService.GetTaskInfoAsync(taskName);
        int n = taskInfo.RowCount;
        var A = new List<List<double>>();

        for (int i = 0; i < n; i++)
        {
            var row = await matrixFileService.ReadRowDataAsync(taskName, "A", i);
            A.Add(row);
        }
        return A;
    }

    private static async Task SolveInFiles(string taskName, LltTaskService lltTaskService)
    {
        await lltTaskService.LLTAsync(taskName);
        await lltTaskService.SolveAsync(taskName);
    }
}
