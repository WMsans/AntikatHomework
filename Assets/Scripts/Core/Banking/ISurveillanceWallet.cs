namespace Core.Banking
{
    public interface ISurveillanceWallet
    {
        int SurveillanceBalance { get; }
        
        void ChangeSurveillanceBalance(int amount);
        
        event BalanceChange SurveillanceBalanceChanged;
    }
}