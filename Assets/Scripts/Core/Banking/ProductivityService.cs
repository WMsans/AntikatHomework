using UnityEngine;

namespace Core.Banking
{
    public class ProductivityService : IProductivityService
    {
        private const float DefaultEmaWeight = 0.25f;
        private readonly float emaWeight;
        private bool hasIncomeRecord;

        public int CurrentIncome { get; private set; }
        public int IncomeEma { get; private set; }
        public int PotentialIncome { get; private set; }

        public event ProductivityChange ProductivityChanged;

        public ProductivityService(float emaWeight = DefaultEmaWeight)
        {
            if (float.IsNaN(emaWeight) || float.IsInfinity(emaWeight) || emaWeight < 0 || emaWeight > 1)
            {
                this.emaWeight = DefaultEmaWeight;
            }
            else
            {
                this.emaWeight = emaWeight;
            }
        }

        public bool RecordDailyIncome(int amount)
        {
            if (amount < 0)
            {
                return false;
            }

            var oldProductivity = GetCurrentProductivity();
            CurrentIncome = amount;

            if (!hasIncomeRecord)
            {
                IncomeEma = amount;
                hasIncomeRecord = true;
            }
            else
            {
                var average = emaWeight * amount + (1f - emaWeight) * IncomeEma;
                IncomeEma = Mathf.RoundToInt(average);
            }

            RaiseProductivityChangedIfNeeded(oldProductivity);
            return true;
        }

        public bool SetPotentialIncome(int amount)
        {
            if (amount < 0)
            {
                return false;
            }

            var oldProductivity = GetCurrentProductivity();
            PotentialIncome = amount;
            RaiseProductivityChangedIfNeeded(oldProductivity);
            return true;
        }

        public int GetCurrentProductivity()
        {
            var productivity = IncomeEma > PotentialIncome ? IncomeEma : PotentialIncome;
            return productivity < 1 ? 1 : productivity;
        }

        private void RaiseProductivityChangedIfNeeded(int oldProductivity)
        {
            var newProductivity = GetCurrentProductivity();
            if (newProductivity == oldProductivity)
            {
                return;
            }

            var handler = ProductivityChanged;
            handler?.Invoke(newProductivity);
        }
    }
}
