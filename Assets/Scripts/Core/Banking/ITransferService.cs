namespace Core.Banking
{
    public interface ITransferService
    {
        bool TryConvert(int requestedYen, IProductivityService productivityService, IBankService bankService);
        bool CanConvert(int requestedYen, IProductivityService productivityService, IBankService bankService);
        
        event ConversionResult ConversionFailed;
        event ConversionResult ConversionSucceeded;
    }
    
    public delegate void ConversionResult(bool success);
}