using Core.Banking;

namespace Core.Shop
{
    public interface IShopItem
    {
        int  Id { get; }
        string Name { get; }
        PurchaseBehavior Behavior { get; }
        int Price { get; }
        
        bool CanBuy(IOverworldWallet wallet);
        bool TryBuy(IOverworldWallet wallet);
    }
}