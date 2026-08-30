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

        public void ChangeSurveillanceBalance(int amount)
        {
            var delta = ComputeClampedDelta(SurveillanceBalance, amount);
            if (delta == 0)
            {
                return;
            }

            SurveillanceBalance += delta;
            RaiseSurveillanceBalanceChanged(delta);
        }

        public void AddOverworldBalance(int amount)
        {
            var delta = ComputeClampedDelta(OverworldBalance, amount);
            if (delta == 0)
            {
                return;
            }

            OverworldBalance += delta;
            RaiseOverworldBalanceChanged(delta);
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

        private static int ComputeClampedDelta(int balance, int amount)
        {
            if (amount == 0)
            {
                return 0;
            }

            var target = balance + amount;
            if (target < 0)
            {
                target = 0;
            }

            return (int)(target - balance);
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
