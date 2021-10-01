using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public class InventoryItem
    {
        public int itemID;
        public string altnID;
        public string itemName;
        public string itemIcon;
        public string description;

        public InventoryItem(int _id, string _altn, string _name, string _icon, string _desc)
        {
            itemID = _id;
            altnID = _altn;
            itemName = _name;
            itemIcon = _icon;
            description = _desc;
        }
    }
}
