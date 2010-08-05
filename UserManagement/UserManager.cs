using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO;
using System.IO;

namespace UserManagement
{
    public class UserManager
    {
        public static bool HasPermission( Permissions needed , User user )
        {
            if ( user != null )
            {
                return GetPermissions( user ) >= needed;
            }
            else
            {
                return true;
            }
        }

        private static Permissions GetPermissions( User user )
        {
            StreamReader streamReader = new StreamReader( "C:/YellingBird/Test/superuseridents.txt" );
            string text = streamReader.ReadToEnd().ToLower();
            string[] idents = text.Split( new char[] { ' ' } );
            streamReader.Close();
            if ( user != null && idents.Contains( user.Ident.ToLower() ) )
            {
                return Permissions.Deity;
            }
            return Permissions.Basic;
        }
    }
}
