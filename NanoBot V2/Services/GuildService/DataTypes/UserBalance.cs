using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public class UserBalance
    {
        public int balance;

        public UserBalance(int _init = 200)
        {
            balance = _init;
        }

        public bool Withdraw(int _cost)
        {
            if(balance < _cost)
            {
                return false;
            }
            else
            {
                balance -= _cost;
                return true;
            }
        }

        public void Deposit(int _amount)
        {
            balance += _amount;
        }
    }
}
