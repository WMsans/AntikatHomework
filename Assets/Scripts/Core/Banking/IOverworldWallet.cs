namespace Core.Banking
{
    public interface IOverworldWallet
    {
        int OverworldBalance { get; }
        
        bool TrySpendOverworldBalance(int amount);
        bool TryDepositOverworldBalance(int amount);
        
        event BalanceChange OverworldBalanceChanged;
    }
}