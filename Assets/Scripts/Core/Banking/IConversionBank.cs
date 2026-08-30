namespace Core.Banking
{
    internal interface IConversionBank
    {
        bool TryApplyConversion(int surveillanceCost, int requestedYen);
    }
}
