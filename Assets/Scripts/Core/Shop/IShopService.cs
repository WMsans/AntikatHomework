using System.Collections.Generic;
using Core.Banking;

namespace Core.Shop
{
    public interface IShopService
    {
        List<IShopItem> Items { get; }
        
        bool TryBuy(IShopItem item, IOverworldWallet wallet);
    }
}