using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConnectionManager;
using DTO;
using UserManagement;

namespace Modules
{
    class ChannelMod : Module
    {
        public override void Incoming( Message mgs )
        {
            if ( UserManager.HasPermission( Permissions.Operator , mgs.MUser ) )
            {
                string line = mgs.Line;
                if ( line.StartsWith( mgs.MServer.MyNick + ": " , StringComparison.OrdinalIgnoreCase ) )
                {
                    line = line.Replace( mgs.MServer.MyNick + ": " , "" );
                    if ( line.StartsWith( "join " , StringComparison.OrdinalIgnoreCase ) )
                    {
                        line = line.Replace( "join " , "" );
                        Reply( line , MessageType.Join );
                    }
                    else if ( line.Equals( "part" , StringComparison.OrdinalIgnoreCase ) )
                    {
                        Reply( MessageType.Part );
                    }
                    else if ( line.StartsWith( "quit " , StringComparison.OrdinalIgnoreCase ) )
                    {
                        line = line.Replace( "quit" , "" );
                        line = line.Trim();
                        Reply( line , MessageType.Quit );
                    }
                    else if ( line.Equals( "quit" , StringComparison.OrdinalIgnoreCase ) )
                    {
                        line = "F*CK ALL OF YOU!";
                        Reply( line , MessageType.Quit );
                    }
                }
            }
        }
    }
}
