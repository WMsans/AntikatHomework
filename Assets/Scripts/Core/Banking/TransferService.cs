using UnityEngine;

namespace Core.Banking
{
    public class TransferService : ITransferService
    {
        private const float DefaultTax = 1f;
        private const float DefaultDifficulty = 1f;
        private const float ConversionScale = 0.35f;
        private const float YenPower = 1.35f;

        private readonly float tax;
        private readonly float difficulty;

        public event ConversionResult ConversionFailed;
        public event ConversionResult ConversionSucceeded;

        public TransferService(float tax = DefaultTax, float difficulty = DefaultDifficulty)
        {
            this.tax = NormalizeTax(tax);
            this.difficulty = NormalizeDifficulty(difficulty);
        }

        public bool CanConvert(int requestedYen, IProductivityService productivityService, IBankService bankService)
        {
            if (!TryGetSurveillanceCost(requestedYen, productivityService, out var surveillanceCost))
            {
                return false;
            }

            if (bankService == null)
            {
                return false;
            }

            if (bankService.SurveillanceBalance < surveillanceCost)
            {
                return false;
            }

            return true;
        }

        public bool TryConvert(int requestedYen, IProductivityService productivityService, IBankService bankService)
        {
            if (!TryGetSurveillanceCost(requestedYen, productivityService, out var surveillanceCost))
            {
                RaiseConversionFailed(false);
                return false;
            }

            var success = TryApplyConversion(bankService, surveillanceCost, requestedYen);

            if (success)
            {
                RaiseConversionSucceeded(true);
            }
            else
            {
                RaiseConversionFailed(false);
            }

            return success;
        }

        private bool TryGetSurveillanceCost(int requestedYen, IProductivityService productivityService, out int surveillanceCost)
        {
            surveillanceCost = 0;
            if (requestedYen <= 0 || productivityService == null)
            {
                return false;
            }

            var productivity = productivityService.GetCurrentProductivity();
            var yenCost = Mathf.Pow(requestedYen, YenPower) + tax;
            var calculatedCost = yenCost * productivity * ConversionScale * difficulty;

            calculatedCost = Mathf.Ceil(calculatedCost);
            if (calculatedCost <= 0 || calculatedCost > int.MaxValue)
            {
                return false;
            }

            surveillanceCost = (int)calculatedCost;
            return true;
        }

        private static bool TryApplyConversion(IBankService bankService, int surveillanceCost, int requestedYen)
        {
            if (bankService == null)
            {
                return false;
            }

            if (bankService is IConversionBank conversionBank)
            {
                return conversionBank.TryApplyConversion(surveillanceCost, requestedYen);
            }

            if (bankService.SurveillanceBalance < surveillanceCost)
            {
                return false;
            }

            var spent = bankService.TrySpendSurveillanceBalance(surveillanceCost);
            var deposited = bankService.TryDepositOverworldBalance(requestedYen);
            return spent && deposited;
        }

        private void RaiseConversionFailed(bool success)
        {
            var handler = ConversionFailed;
            handler?.Invoke(success);
        }

        private void RaiseConversionSucceeded(bool success)
        {
            var handler = ConversionSucceeded;
            handler?.Invoke(success);
        }

        private static float NormalizeTax(float value) => Mathf.Max(value, 0);

        private static float NormalizeDifficulty(float value) => Mathf.Max(value, DefaultDifficulty);
    }
}
