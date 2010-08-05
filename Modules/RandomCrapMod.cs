using System;
using System.Collections.Generic;
using System.Text;
using ConnectionManager;
using DTO;
using UserManagement;

namespace Modules
{
    class RandomCrapMod : Module
    {
        public override void Incoming( Message mgs )
        {
            if ( mgs.Line.Equals( "!d" ) )
            {
                Reply( "DIRECTED BY M. NIGHT SHYAMALAN" , MessageType.Message );
            }
            if ( mgs.Line.Equals( "botsnack" , StringComparison.OrdinalIgnoreCase ) )
            {
                Reply( ":D" );
            }
            else if ( mgs.Line.Equals( "relixsnack" , StringComparison.OrdinalIgnoreCase ) )
            {
                Reply( ":DG<" );
            }

        }


    }
}
