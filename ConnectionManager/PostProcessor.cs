using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO;

namespace ConnectionManager
{
    class PostProcessor
    {
        public Hub CentralHub { get; private set; }
        public PostProcessor( Hub hub )
        {
            CentralHub = hub;
            ConnManager.Instance.PostProc = this;
        }

        public void Receive( Server server , string msg )
        {
            //:Jens!~jens@91.182.54.135 PRIVMSG #zeus :3m is ideaal
            //:YellingBird!~TangBot@ip-213-49-92-48.dsl.scarlet.be PART #zeus
            //:YellingBird!~TangBot@ip-213-49-92-48.dsl.scarlet.be JOIN :#zeus
            //:wina.kelder.be 332 YellingBird #zeus :Zeus WPI | Please don't feed the trolls http://tinyurl.com/33ob4or | Got paint? http://bit.ly/9646nm
            //:wina.kelder.be 333 YellingBird #zeus Lolcat 1278758838
            //:wina.kelder.be 353 YellingBird = #zeus :YellingBird reeeelix kire Rofldawg kidk nudded CIA-1 Chton Tots Frying_ookelvis jaspervdj GilJ Mathiasdm +ddfreyne +polipie @Javache lucas +dolfijn +mrosmosis +MrWhite
            //:wina.kelder.be 366 YellingBird #zeus :End of /NAMES list.
            Message newMessage = new Message();
            newMessage.Time = DateTime.Now;
            newMessage.RawLine = msg;
            msg = msg.TrimStart( ':' );
            newMessage.MServer = server;
            // :Chton!~Chton@ip-81-11-181-99.dsl.scarlet.be PRIVMSG #zeus :excellent, time to reverse engineer me some IRC :p
            string[] messageparts = msg.Split( new char[] { ':' } , 2 );
            string left = messageparts[ 0 ];
            string right = "";
            if ( messageparts.Length > 1 )
            {
                right = messageparts[ 1 ];
            }
            //:Jens!~jens@91.182.54.135 PRIVMSG #zeus :3m is ideaal
            if ( left.Contains( "PRIVMSG" ) )
            {
                ProcessPrivMsg( newMessage , left , right );
            }
            //:YellingBird!~TangBot@ip-213-49-92-48.dsl.scarlet.be JOIN :#zeus
            else if ( left.Contains( "JOIN" ) )
            {
                ProcessJoin( newMessage , left , right );
            }
            //:YellingBird!~TangBot@ip-213-49-92-48.dsl.scarlet.be PART #zeus
            else if ( left.Contains( "PART" ) )
            {
                ProcessPart( newMessage , left );
            }
            //:nikolas!~nikolas@78-22-244-73.access.telenet.be QUIT :Quit: Lost terminal
            else if ( left.Contains( "QUIT" ) )
            {
                ProcessQuit( newMessage , left , right );
            }
            //:wina.kelder.be 332 YellingBird #zeus :Zeus WPI | Please don't feed the trolls http://tinyurl.com/33ob4or | Got paint? http://bit.ly/9646nm
            else if ( left.Contains( "332" ) )
            {
                ProcessTopic( newMessage , left , right );
            }
            //:wina.kelder.be 353 YellingBird = #zeus :YellingBird reeeelix kire Rofldawg kidk nudded CIA-1 Chton Tots Frying_ookelvis jaspervdj GilJ Mathiasdm +ddfreyne +polipie @Javache lucas +dolfijn +mrosmosis +MrWhite
            else if ( left.Contains( "353" ) )
            {
                ProcessNickList( newMessage , left , right );
            }
            //:wina.kelder.be 311 YellingBird Chton ~Chton ip-213-49-92-48.dsl.scarlet.be * :bram de buyser
            else if ( left.Contains( "311" ) )
            {
                ProcessWhois( newMessage , left , right );
            }
            if ( newMessage.Line == null )
            {
                newMessage.Line = "";
            }
            if ( newMessage.MUser == null )
            {
               newMessage.MUser = ( from User y in newMessage.MServer.AllUsers where y.Nick.Equals( newMessage.MServer.MyNick ) select y ).FirstOrDefault();
            }
            CentralHub.Receive( newMessage );

        }

        private void ProcessQuit( Message newMessage , string left , string right )
        {
            //:nikolas!~nikolas@78-22-244-73.access.telenet.be QUIT :Quit: Lost terminal
            string[] leftparts = left.Split( new char[] { ' ' } );
            newMessage.MUser = FindUser( newMessage , leftparts[ 0 ] , false );
            foreach ( Channel chan in newMessage.MServer.Channels )
            {
                if ( chan.CUsers.Contains( newMessage.MUser ) )
                {
                    chan.CUsers.Remove( newMessage.MUser );
                }
                chan.CServer.AllUsers.Remove( newMessage.MUser );
            }
            newMessage.MType = MessageType.Quit;
            newMessage.Line = right;
        }

        private void ProcessWhois( Message newMessage , string left , string right )
        {
            string[] leftparts = left.Split( new char[] { ' ' } );
            User result = ( from User y in newMessage.MServer.AllUsers where y.Nick.Equals( leftparts[ 3 ] ) select y ).FirstOrDefault();
            if ( result == null )
            {
                result = new User();
                result.Nick = leftparts[ 3 ];
                result.UServer = newMessage.MServer;
                newMessage.MServer.AllUsers.Add( result );
            }
            result.Ident = leftparts[ 4 ];
            result.HostName = leftparts[ 5 ];
            result.RealName = right;
        }

        private void ProcessNickList( Message newMessage , string left , string right )
        {
            string[] leftparts = left.Split( new char[] { ' ' } );
            newMessage.MChannel = ( from Channel x in newMessage.MServer.Channels where x.CFullName.Equals( leftparts[ 4 ] , StringComparison.OrdinalIgnoreCase ) select x ).FirstOrDefault();
            BuildUserList( right , newMessage.MServer , newMessage.MChannel );
        }

        private void BuildUserList( string right , Server server , Channel channel )
        {
            string[] rightparts = right.Split( new char[] { ' ' } );
            foreach ( string nick in rightparts )
            {
                string n = nick;
                n = n.Replace( "@" , "" );
                n = n.Replace( "+" , "" );
                n = n.Replace( "&" , "" );
                User result = ( from User y in server.AllUsers where y.Nick.Equals( n ) select y ).FirstOrDefault();
                if ( result == null )
                {
                    result = new User();
                    result.Nick = n;
                    result.UServer = server;
                    server.AllUsers.Add( result );
                }
                if ( !channel.CUsers.Contains( result ) )
                {
                    channel.CUsers.Add( result );
                }
                if ( result.Ident == null )
                {
                    ConnManager.Instance.Send( server , "WHOIS " + n );
                }
            }
        }

        private void ProcessTopic( Message newMessage , string left , string right )
        {
            string[] leftparts = left.Split( new char[] { ' ' } );
            newMessage.MChannel = ( from Channel x in newMessage.MServer.Channels where x.CFullName.Equals( leftparts[ 3 ] , StringComparison.OrdinalIgnoreCase ) select x ).FirstOrDefault();
            newMessage.Line = right;
            newMessage.MChannel.Topic = right;
        }

        private void ProcessPart( Message newMessage , string left )
        {
            string[] leftparts = left.Split( new char[] { ' ' } );
            newMessage.MChannel = ( from Channel x in newMessage.MServer.Channels where x.CFullName.Equals( leftparts[ 2 ] , StringComparison.OrdinalIgnoreCase ) select x ).FirstOrDefault();
            newMessage.MUser = FindUser( newMessage , leftparts[ 0 ] , false );
            if ( newMessage.MChannel.CUsers.Contains( newMessage.MUser ) )
            {
                newMessage.MChannel.CUsers.Remove( newMessage.MUser );
            }
            newMessage.MType = MessageType.Part;
        }

        private User FindUser( Message newMessage , string id , bool addtochannel )
        {
            User result = ( from User y in newMessage.MServer.AllUsers where ( y.Nick + "!" + y.Ident + "@" + y.HostName ).Equals( id , StringComparison.OrdinalIgnoreCase ) select y ).FirstOrDefault();
            if ( result == null )
            {
                User newUser = new User();
                newUser.Nick = id.Substring( 0 , id.IndexOf( "!" ) );
                newUser.Ident = id.Substring( id.IndexOf( "!" ) + 1 , id.IndexOf( "@" ) - id.IndexOf( "!" ) - 1 );
                newUser.HostName = id.Substring( id.IndexOf( "@" ) + 1 , id.Length - id.IndexOf( "@" ) - 1 );
                newUser.UServer = newMessage.MServer;
                newMessage.MServer.AllUsers.Add( newUser );
                result = newUser;
            }
            if ( addtochannel && !newMessage.MChannel.CUsers.Contains( result ) )
            {
                newMessage.MChannel.CUsers.Add( result );
            }
            return result;
        }

        private void ProcessJoin( Message newMessage , string left , string right )
        {
            string[] leftparts = left.Split( new char[] { ' ' } );
            newMessage.MChannel = newMessage.MChannel = ( from Channel x in newMessage.MServer.Channels where x.CFullName.Equals( right , StringComparison.OrdinalIgnoreCase ) select x ).FirstOrDefault();
            newMessage.MUser = FindUser( newMessage , leftparts[ 0 ] , true );
            newMessage.MType = MessageType.Join;
        }

        private void ProcessPrivMsg( Message newMessage , string left , string right )
        {
            left = left.Trim();
            string[] leftparts = left.Split( ' ' );
            if ( leftparts.Length == 3 )
            {
                newMessage.MType = MessageType.Message;
                newMessage.MChannel = ( from Channel x in newMessage.MServer.Channels where x.CFullName.Equals( leftparts[ 2 ] , StringComparison.OrdinalIgnoreCase ) select x ).FirstOrDefault();
                newMessage.MUser = FindUser( newMessage , leftparts[ 0 ] , true );
                if ( right.StartsWith( "\u0001" + "ACTION" ) )
                {
                    right = right.Replace( "\u0001" + "ACTION" , "" );
                    right = right.Replace( "\u0001" , "" );
                    right = right.Trim();
                    newMessage.MType = MessageType.Action;
                }
                newMessage.Line = right;
                if ( newMessage.MChannel != null && newMessage.Line == null )
                {
                    newMessage.Line = "";
                }
            }
        }
              
    }
}
