namespace Core.Banking
{
    public interface IProductivityService
    {
        // Monitor the income to evaluate the productivity
        int CurrentIncome { get; }
        int IncomeEMA { get; }
        int PotentialIncome { get; }
        
        int GetCurrentProductivity();
    }
}