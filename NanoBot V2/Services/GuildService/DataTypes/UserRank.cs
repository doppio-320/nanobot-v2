using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public class UserRank
    {
        public int currentRank;
        public int currentXP;
        private DateTime rateLimit;

        public UserRank()
        {
            currentRank = 1;
            currentXP = 0;
            rateLimit = DateTime.Now;
        }

        public int AddXP(UserBalance _bal, int _xp = 350)
        {
            if (DateTime.Now < rateLimit)
                return -1; //Rate limited

            currentXP += _xp;
            rateLimit = DateTime.Now.AddMinutes(1.5d);
            if(currentXP > RequiredXP(currentRank))
            {
                var prevRank = currentRank;
                while(currentXP > RequiredXP(currentRank))
                {
                    currentXP -= RequiredXP(currentRank); //Cutoff excess rank repeatedly until no more excess rank

                    currentRank++;

                    _bal.Deposit(currentRank * 75);
                }

                var rankJump = currentRank - prevRank;

                return rankJump; //Rank up! (rankJump = 1 means normal rank up, rankJump > 1 means user skipped a rank or more)
            }            

            return 0; //Normal; just added xp (-2 for wrong channel)
        }

        public static int RequiredXP(int _rank)
        {
            return _rank * 1000;
        }

        public float GetRankIndex()
        {
            return (float)currentRank + ((float)currentXP / RequiredXP(currentRank));
        }
    }
}
