using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public class GuildAutores
    {
        public List<AutoresResponse> autoresResponses = new List<AutoresResponse>();

        public GuildAutores()
        {

        }

        public bool AddAutores(string _trig, string _res, AutoresCondition _cond = AutoresCondition.Contains, bool _case = false)
        {
            if (!ResponsePossible(_trig))
            {
                autoresResponses.Add(new AutoresResponse(_trig, _res, _cond, _case));
                return true;
            }
            return false;
        }

        public List<AutoresResponse> GetAutoresResponses()
        {
            return autoresResponses;
        }

        public bool RemoveAutores(int _id)
        {
            if(autoresResponses[_id] != null)
            {
                autoresResponses.RemoveAt(_id);
                return true;
            }
            return false;
        }

        public bool ResponsePossible(string _text)
        {
            foreach (var autoRes in autoresResponses)
            {
                if(autoRes.condition == AutoresCondition.Exact)
                {
                    if(!autoRes.caseSensitive && _text.ToLower() == autoRes.trigger.ToLower())
                    {
                        return true;
                    }
                    else if(autoRes.caseSensitive && _text == autoRes.trigger)
                    {
                        return true;
                    }
                }
                else
                {
                    if (!autoRes.caseSensitive)
                    {
                        if (_text.ToLower().Contains(autoRes.trigger.ToLower()))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (_text.Contains(autoRes.trigger))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public string ReturnAutores(string _trigger)
        {
            foreach (var autoRes in autoresResponses)
            {
                if (autoRes.condition == AutoresCondition.Exact)
                {
                    if (!autoRes.caseSensitive && _trigger.ToLower() == autoRes.trigger.ToLower())
                    {
                        return autoRes.response;
                    }
                    else if (autoRes.caseSensitive && _trigger == autoRes.trigger)
                    {
                        return autoRes.response;
                    }
                }
                else
                {
                    if (!autoRes.caseSensitive)
                    {
                        if (_trigger.ToLower().Contains(autoRes.trigger.ToLower()))
                        {
                            return autoRes.response;
                        }
                    }
                    else
                    {
                        if (_trigger.Contains(autoRes.trigger))
                        {
                            return autoRes.response;
                        }
                    }
                }
            }
            return "";
        }
    }
}
