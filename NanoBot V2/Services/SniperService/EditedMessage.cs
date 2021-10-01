using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public class EditedMessage
    {
        public IUser Author;
        public string AfterEditMessage;
        public string BeforeEditMessage;
        public DateTime TimeEdited;

        public EditedMessage(IUser _user, string _before, string _after, DateTime _edited)
        {
            Author = _user;
            AfterEditMessage = _after;
            BeforeEditMessage = _before;
            TimeEdited = _edited;
        }
    }
}
