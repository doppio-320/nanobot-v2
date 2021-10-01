using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public class UserInventory
    {
        public Dictionary<int, int> storedItems = new Dictionary<int, int>();

        public UserInventory()
        {
            storedItems = new Dictionary<int, int>();            
        }

        public bool HasItem(int _itemID)
        {
            foreach (var item in storedItems)
            {
                if(item.Key == _itemID)
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
            return false;
        }        

        //Return -1 if none
        public int GetItemAmount(int _itemID)
        {
            if (!HasItem(_itemID))
            {
                return -1;
            }
            return storedItems[_itemID];
        }    

        public bool HasEnoughItems(int _itemID, int _amount)
        {
            if (!HasItem(_itemID))
            {
                return false;
            }

            if(storedItems[_itemID] < _amount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public void AddItem(int _itemID, int _amount = 1)
        {
            if(_amount < 1)
            {
                return;
            }
            if (!HasItem(_itemID))
            {
                storedItems.Add(_itemID, _amount);
            }
            else
            {
                storedItems[_itemID] += _amount;
            }
        }

        public bool RemoveItem(int _itemID, int _amount = 1)
        {
            if(_amount < 1)
            {
                return false;
            }

            if (!HasItem(_itemID))
            {
                return false;
            }
            else
            {
                if (!HasEnoughItems(_itemID, _amount))
                {
                    return false;
                }
                else if (storedItems[_itemID] == _amount)
                {
                    storedItems.Remove(_itemID);
                    return true;
                }
                else if(storedItems[_itemID] > _amount)
                {
                    storedItems[_itemID] -= _amount;
                    return true;
                }
            }
            return false;
        }
    }
}
