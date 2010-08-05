using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using DTO;

namespace ConnectionManager
{
    class Connection
    {
        public TcpClient Sock { get; set; }
        public Server Serv { get; set; }
        public StreamReader Reader { get; set; }
        public StreamWriter Writer { get; set; }
        public ConnManager CMan { get; set; }

        public Connection( Server serv, ConnManager man )
        {
            Serv = serv;
            CMan = man;
            Thread t = new Thread( new ThreadStart( Active ) );
            t.Start();

        }

        public void Active()
        {
            StartUp();
            Init();
            string line = "";
            while(Sock.Connected && (line = Reader.ReadLine()) != null) {
                Process(line);
            }
            Active();
        }

        private void StartUp()
        {
            Sock = new TcpClient();
            Sock.Connect( Serv.Address , Serv.Port );
            Reader = new StreamReader( Sock.GetStream() );
            Writer = new StreamWriter( Sock.GetStream() );
            Writer.NewLine = "\r\n";
            Writer.AutoFlush = true;
        }

        private void Process(string line)
        {
            //Console.WriteLine( line );
            if ( line.StartsWith( "PING " ) )
            {
                Write( "PONG " + line.Substring( 5 ) );
            }
            CMan.Receive( Serv , line );
        }

        private void Init()
        {
            Write( "USER " + Serv.Ident + " Tang Tang :" + Serv.MyDescription );
            Write( "NICK " + Serv.MyNick );
            string line = "";
            while ( Sock.Connected && ( line = Reader.ReadLine() ) != null )
            {
                //:wina.kelder.be 376 YellingBird :End of /MOTD command.
                if ( line.Substring( line.IndexOf( ' ' ) + 1 ).StartsWith( "376" ) )
                {
                    break;
                }
                else if ( line.StartsWith( "PING " ) )
                {
                    Write( "PONG " + line.Substring( 5 ) );
                }
            }
            foreach ( var chan in Serv.Channels )
            {
                Write( "JOIN " + chan.CFullName );
            }
        }

        public bool Write( string linetowrite )
        {
            if ( Sock.Connected )
            {
                Writer.WriteLine( linetowrite);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
