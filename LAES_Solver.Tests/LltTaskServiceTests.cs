using LAES_Solver.Application.Services;
using LAES_Solver.Domain.Interfaces;
using LAES_Solver.Infrastracture.Services;

[TestClass]
public class LltTaskServiceTests
{
    private IMatrixFileService _matrixFileService;
    private LltTaskService _lltTaskService;

    public LltTaskServiceTests()
    {
        string baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "MatrixData");
        _matrixFileService = new MatrixFileService(baseDirectory);
        _lltTaskService = new LltTaskService(_matrixFileService);
    }

    [TestMethod]
    public async Task LLTAsync()
    {
        // Заглушка
        Assert.IsTrue(true);
    }

    [TestMethod]
    public async Task SolveAsync()
    {
        // Заглушка
        Assert.IsTrue(true);
    }
}