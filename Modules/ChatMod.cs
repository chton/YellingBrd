using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO;
using System.Threading;

namespace Modules
{
    class ChatMod : Module
    {
        public ChatMod()
            : base()
        {
            Thread t = new Thread(new ThreadStart( MessagesIn ));
            t.Start();
        }

        public override void Incoming( Message mgs )
        {
            if ( mgs.MChannel != null && mgs.MUser != null )
            {
                Console.WriteLine( mgs.MType + ": (" + mgs.MServer.Name + " - " + mgs.MChannel.CFullName + ") " + mgs.Time.TimeOfDay + " <" + mgs.MUser.Nick + "> " + mgs.Line );
            }
            else if ( !String.IsNullOrEmpty( mgs.Line ) )
            {
                Console.WriteLine( mgs.MType + ": (" + mgs.MServer.Name + ") " + mgs.Time.TimeOfDay + ":" + mgs.Line );
            }
            else
            {
                Console.WriteLine( mgs.MType + ": (" + mgs.MServer.Name + ") " + mgs.Time.TimeOfDay + " " + mgs.RawLine );
            }
        }

        public void MessagesIn()
        {
            string line = "";
            while ( ( line = Console.ReadLine() ) != null )
            {
                string[] lineparts = line.Split( new char[] { ':' } , 2 );
                Message res = new Message();

                if ( lineparts.Length > 1 )
                {
                    res.Line = lineparts[1];
                    if ( !lineparts[ 0 ].Contains( "#" ) )
                    {
                        lineparts[ 0 ] = "#" + lineparts[ 0 ]; 
                    }
                    res.MChannel = ( from Channel x in LastMessage.MServer.Channels where x.CFullName.Equals( lineparts[ 0 ] , StringComparison.OrdinalIgnoreCase ) select x ).FirstOrDefault();
                    LastMessage.MChannel = res.MChannel;
                }
                else
                {
                    res.Line = line;
                    res.MChannel = LastMessage.MChannel;
                }
                res.MServer = LastMessage.MServer;
                res.MUser = LastMessage.MUser;
                res.MType = MessageType.Message;
                res.Line = res.Line.Trim();
                Reply( res );
            }
        }

    }
}
