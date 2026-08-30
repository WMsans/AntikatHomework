using System;

namespace Core.Banking
{
    public interface IProductivityService
    {
        // Monitor the income to evaluate the productivity
        int CurrentIncome { get; }
        int IncomeEma { get; }
        int PotentialIncome { get; }

        int GetCurrentProductivity();

        event ProductivityChange ProductivityChanged;
    }
    
    public delegate void ProductivityChange(int changeAmount);
}
