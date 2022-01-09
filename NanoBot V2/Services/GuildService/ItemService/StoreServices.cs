using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using MongoDB.Bson;
using MongoDB.Driver;

namespace NanoBot_V2.Services
{
    public static class StoreServices
    {
        public static Dictionary<int, KeyValuePair<int, int>> storePrices = new Dictionary<int, KeyValuePair<int, int>>();

        public static void Load()
        {
            var collection = DatabaseConnectionManager.GetCollectionFromStatic("market_data");
            var allPrices = collection.Find(new BsonDocument()).ToList();

            foreach (var price in allPrices)
            {
                var itemID = price.GetValue("item_id").AsInt32;
                var buyPrice = price.GetValue("buy_price").AsInt32;
                var sellPrice = price.GetValue("sell_price").AsInt32;

                storePrices.Add(itemID, new KeyValuePair<int, int>(buyPrice, sellPrice));
            }

            ProgramLogger.LogImportant("Loaded StorePrices Data.");
        }

        public static int BuyPrice(int _itemID, int _amount = 1)
        {
            var item = InventoryItemServices.FromID(_itemID);

            if (item == null)
                return -1;

            return storePrices[_itemID].Key * _amount;
        }

        public static int SellPrice(int _itemID, int _amount = 1)
        {
            var item = InventoryItemServices.FromID(_itemID);

            if (item == null)
                return -1;

            return storePrices[_itemID].Value * _amount;
        }

        public static bool Buy(ulong _guildID, ulong _userID, int _itemID, int _amount = 1)
        {
            var item = InventoryItemServices.FromID(_itemID);

            if (item == null)
                return false;

            if (GuildServices.GetGuildData(_guildID).GetUserData(_userID).balance.balance < (storePrices[_itemID].Key * _amount))
            {
                return false;
            }
            else
            {
                GuildServices.GetGuildData(_guildID).GetUserData(_userID).balance.Withdraw(storePrices[_itemID].Key * _amount);
                InventoryItemServices.GetUserInventory(_guildID, _userID).AddItem(_itemID, _amount);
                return true;
            }
        }

        public static bool Sell(ulong _guildID, ulong _userID, int _itemID, int _amount = 1)
        {
            var item = InventoryItemServices.FromID(_itemID);

            if (item == null)
                return false;

            if (InventoryItemServices.GetUserInventory(_guildID, _userID).HasEnoughItems(_itemID, _amount))
            {
                InventoryItemServices.GetUserInventory(_guildID, _userID).RemoveItem(_itemID, _amount);
                GuildServices.GetGuildData(_guildID).GetUserData(_userID).balance.Deposit(storePrices[_itemID].Value * _amount);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
