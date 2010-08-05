using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO;

namespace ConnectionManager
{
    class PreProcessor
    {
        public bool Send( Message msg )
        {
            if ( msg.MServer == null || msg.MChannel == null )
            {
                string builtMessage = msg.Line;
                msg.MServer = new Server();
                //return ConnManager.Instance.Send( msg.MServer , builtMessage );
                return false;
            }
            else if ( msg.MType == MessageType.Message )
            {
                string builtMessage = "PRIVMSG " + msg.MChannel.CFullName + " :" + msg.Line;
                return ConnManager.Instance.Send( msg.MServer , builtMessage );
            }
            else if ( msg.MType == MessageType.Action )
            {
                string builtMessage = "PRIVMSG " + msg.MChannel.CFullName + " :" + "\u0001" + "ACTION " + msg.Line + "\u0001";
                return ConnManager.Instance.Send( msg.MServer , builtMessage );
            }
            else if ( msg.MType == MessageType.Join )
            {
                string builtMessage = "JOIN " + msg.Line;
                Channel c = new Channel();
                c.CFullName = msg.Line;
                c.CName = msg.Line.Replace( "#" , "" );
                c.CServer = msg.MServer;
                msg.MServer.Channels.Add( c );
                return ConnManager.Instance.Send( msg.MServer , builtMessage );
            }
            else if ( msg.MType == MessageType.Part )
            {
                string builtMessage = "PART " + msg.MChannel.CFullName;
                msg.MServer.Channels.Remove( msg.MChannel );
                return ConnManager.Instance.Send( msg.MServer , builtMessage );
            }
            else if ( msg.MType == MessageType.Quit )
            {
                string builtMessage = "QUIT :" + msg.Line;
                //ConnManager.Instance.Sockets.Remove( msg.MServer );
                return ConnManager.Instance.Send( msg.MServer , builtMessage );


            }
            else if ( msg.MType == MessageType.Kick )
            {
                string builtMessage = "KICK " + msg.MChannel.CFullName + " " + msg.MUser.Nick + " :" + msg.Line;
                return ConnManager.Instance.Send( msg.MServer , builtMessage );
            }
            else return false;
        }
    }
}
