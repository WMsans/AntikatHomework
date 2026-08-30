using System;

namespace Core.Banking
{
    public class BankService : IBankService, IConversionBank
    {
        public int SurveillanceBalance { get; private set; }

        public int OverworldBalance { get; private set; }

        public event BalanceChange SurveillanceBalanceChanged;
        public event BalanceChange OverworldBalanceChanged;

        public BankService(int startingSurveillanceBalance = 0, int startingOverworldBalance = 0)
        {
            SurveillanceBalance = startingSurveillanceBalance < 0 ? 0 : startingSurveillanceBalance;
            OverworldBalance = startingOverworldBalance < 0 ? 0 : startingOverworldBalance;
        }

        public bool TrySpendSurveillanceBalance(int amount)
        {
            if (amount <= 0 || amount > SurveillanceBalance)
            {
                return false;
            }

            SurveillanceBalance -= amount;
            RaiseSurveillanceBalanceChanged(-amount);
            return true;
        }

        public bool TryDepositSurveillanceBalance(int amount)
        {
            if (amount <= 0 || amount > int.MaxValue - SurveillanceBalance)
            {
                return false;
            }

            SurveillanceBalance += amount;
            RaiseSurveillanceBalanceChanged(amount);
            return true;
        }

        public bool TrySpendOverworldBalance(int amount)
        {
            if (amount <= 0 || amount > OverworldBalance)
            {
                return false;
            }

            OverworldBalance -= amount;
            RaiseOverworldBalanceChanged(-amount);
            return true;
        }

        public bool TryDepositOverworldBalance(int amount)
        {
            if (amount <= 0 || amount > int.MaxValue - OverworldBalance)
            {
                return false;
            }

            OverworldBalance += amount;
            RaiseOverworldBalanceChanged(amount);
            return true;
        }

        bool IConversionBank.TryApplyConversion(int surveillanceCost, int requestedYen)
        {
            if (surveillanceCost <= 0 || requestedYen <= 0)
            {
                return false;
            }

            if (surveillanceCost > SurveillanceBalance)
            {
                return false;
            }

            if (requestedYen > int.MaxValue - OverworldBalance)
            {
                return false;
            }

            SurveillanceBalance -= surveillanceCost;
            OverworldBalance += requestedYen;

            RaiseSurveillanceBalanceChanged(-surveillanceCost);
            RaiseOverworldBalanceChanged(requestedYen);
            return true;
        }

        private void RaiseSurveillanceBalanceChanged(int changeAmount)
        {
            var handler = SurveillanceBalanceChanged;
            handler?.Invoke(changeAmount);
        }

        private void RaiseOverworldBalanceChanged(int changeAmount)
        {
            var handler = OverworldBalanceChanged;
            handler?.Invoke(changeAmount);
        }
    }
}
