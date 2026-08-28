namespace Core.Banking
{
    public interface IOverworldWallet
    {
        int OverworldBalance { get; }
        
        void AddOverworldBalance(int amount);
        
        event BalanceChange OverworldBalanceChanged;
    }
}