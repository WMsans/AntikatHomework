namespace Core.Banking
{
    public interface ISurveillanceWallet
    {
        int SurveillanceBalance { get; }
        
        bool TrySpendSurveillanceBalance(int amount);
        bool TryDepositSurveillanceBalance(int amount);
        
        event BalanceChange SurveillanceBalanceChanged;
    }
}