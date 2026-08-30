namespace Core.Banking
{
    public interface IBankService : ISurveillanceWallet, IOverworldWallet
    {
    }

    public delegate void BalanceChange(int changeAmount);
}
