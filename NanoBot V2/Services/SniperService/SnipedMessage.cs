using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public class SnipedMessage
    {
        public IUser Author;
        public string Message;
        public DateTime TimeDeleted;
        public IAttachment Attachment;

        public SnipedMessage(IUser _user, string _msg, DateTime _deleted, IAttachment _attachment = null)
        {
            Author = _user;
            Message = _msg;
            TimeDeleted = _deleted;
            Attachment = _attachment;
        }
    }
}
