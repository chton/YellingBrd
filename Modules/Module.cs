using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConnectionManager;
using DTO;
using UserManagement;

namespace Modules
{
    public abstract class Module
    {
        protected Message LastMessage { get; set; }

        public Module()
        {
            Hub.Instance.MessageReceived += new Hub.MessageHandler( MessageReceived );
        }

        private void MessageReceived( Message mess )
        {
            LastMessage = mess;
            if ( UserManager.HasPermission( Permissions.Basic , mess.MUser ) )
            {
                Incoming( mess );
            }
        }

        public abstract void Incoming( Message mgs );

        public void Reply( string line )
        {
            Reply( line , LastMessage.MType );
        }

        public void Reply( MessageType type )
        {
            Reply( LastMessage.Line , type );
        }

        public void Reply( string line , MessageType type )
        {
            Message res = new Message();
            res.Line = line;
            res.MChannel = LastMessage.MChannel;
            res.MServer = LastMessage.MServer;
            res.MUser = LastMessage.MUser;
            res.MType = type;
            Reply( res );
        }

        public void Reply( Message message )
        {
            Hub.Instance.Send( message );
        }

        public void Dispose()
        {
            Hub.Instance.MessageReceived -= new Hub.MessageHandler( MessageReceived );
        }
    }
}
