using System.Collections.Generic;
using System.Linq;
using Core.Banking;

namespace Core.Shop
{
    public class ShopService : IShopService
    {
        private readonly List<IShopItem> items = new List<IShopItem>();
        private readonly HashSet<int> purchasedItemIds = new HashSet<int>();

        public List<IShopItem> Items => new(items);

        public ShopService(IEnumerable<IShopItem> items)
        {
            AddUniqueItems(items);
        }

        private bool HasPurchased(int itemId)
        {
            return purchasedItemIds.Contains(itemId);
        }

        public bool TryBuy(IShopItem item, IOverworldWallet wallet)
        {
            if (item == null)
            {
                return false;
            }

            var catalogItem = FindItem(item.Id);
            if (catalogItem == null)
            {
                return false;
            }

            if (catalogItem.Behavior == PurchaseBehavior.OneTime && HasPurchased(catalogItem.Id))
            {
                return false;
            }

            if (!catalogItem.TryBuy(wallet))
            {
                return false;
            }

            if (catalogItem.Behavior == PurchaseBehavior.OneTime)
            {
                purchasedItemIds.Add(catalogItem.Id);
            }

            return true;
        }

        private void AddUniqueItems(IEnumerable<IShopItem> source)
        {
            if (source == null)
            {
                return;
            }

            var knownIds = new HashSet<int>();
            foreach (var item in source)
            {
                if (item != null && knownIds.Add(item.Id))
                {
                    items.Add(item);
                }
            }
        }

        private IShopItem FindItem(int id)
        {
            return items.FirstOrDefault(t => t.Id == id);
        }
    }
}
