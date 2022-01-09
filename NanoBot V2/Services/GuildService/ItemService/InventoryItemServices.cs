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
    public static class InventoryItemServices
    {
        public static Dictionary<int, InventoryItem> loadedItems = new Dictionary<int, InventoryItem>();

        public static void Load()
        {
            var collection = DatabaseConnectionManager.GetCollectionFromStatic("item_data");
            var allItems = collection.Find(new BsonDocument()).ToList();

            foreach (var item in allItems)
            {
                var itemID = item.GetValue("item_id").AsInt32;
                var itemAltn = item.GetValue("altn_id").AsString;
                var itemName = item.GetValue("item_name").AsString;
                var itemEmo = item.GetValue("item_icon").AsString;
                var itemDesc = item.GetValue("desc").AsString;

                loadedItems.Add(itemID, new InventoryItem(itemID, itemAltn, itemName, itemEmo, itemDesc));
            }


            ProgramLogger.LogImportant("Loaded ItemData Data.");
        }

        public static InventoryItem FromID(int _id)
        {
            if (!loadedItems.ContainsKey(_id))
            {
                return null;
            }
            else
            {
                return loadedItems[_id];
            }
        }

        public static InventoryItem FromAltnID(string _altn)
        {
            foreach (var item in loadedItems)
            {
                if (item.Value.altnID.ToLower() == _altn.ToLower())
                {
                    return item.Value;
                }                
            }
            return null;
        }

        public static InventoryItem FromName(string _name)
        {
            foreach (var item in loadedItems)
            {
                if (item.Value.itemName.ToLower().Contains(_name.ToLower()))
                {
                    return item.Value;
                }
            }
            return null;
        }

        public static UserInventory GetUserInventory(ulong _guildID, ulong _userID)
        {
            return GuildServices.GetGuildData(_guildID).GetUserData(_userID).inventory;
        }
    }
}
