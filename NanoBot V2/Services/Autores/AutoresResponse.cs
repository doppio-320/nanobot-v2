using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public class AutoresResponse
    {
        public AutoresCondition condition;
        public bool caseSensitive;

        public string trigger;
        public string response;

        public AutoresResponse(string _trig, string _res, AutoresCondition _cond = AutoresCondition.Contains, bool _case = false)
        {
            trigger = _trig;
            response = _res;
            condition = _cond;
            caseSensitive = _case;
        }

        public static AutoresCondition ParseAutoresCondition(string _txt)
        {
            AutoresCondition triggerMode = Services.AutoresCondition.Contains;
            switch (_txt.ToLower())
            {
                case "exact":
                    triggerMode = Services.AutoresCondition.Exact;
                    break;

                case "contains":
                    triggerMode = Services.AutoresCondition.Contains;
                    break;
            }

            return triggerMode;
        }
    }

    public enum AutoresCondition
    {
        Contains, Exact
    }    
}
