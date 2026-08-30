using Core.Banking;

namespace Core.Shop
{
    public class ShopItem : IShopItem
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public PurchaseBehavior Behavior { get; private set; }
        public int Price { get; private set; }

        public ShopItem(int id, string name, int price, PurchaseBehavior behavior)
        {
            Id = id;
            Name = name ?? string.Empty;
            Price = price;
            Behavior = behavior;
        }

        public bool CanBuy(IOverworldWallet wallet)
        {
            return wallet != null && Price > 0 && wallet.OverworldBalance >= Price;
        }

        public bool TryBuy(IOverworldWallet wallet)
        {
            if (!CanBuy(wallet))
            {
                return false;
            }

            return wallet.TrySpendOverworldBalance(Price);
        }
    }
}
