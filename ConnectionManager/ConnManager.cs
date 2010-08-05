using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using DTO;

namespace ConnectionManager
{
    class ConnManager
    {
        private static ConnManager instance;
        public List<Server> ServerList { get; set; }
        public Dictionary<Server , Connection> Sockets { get; set; }
        public ConnManager()
        {
            ServerList = ServerFactory.GetServers();
            Connect();

        }
        public static ConnManager Instance
        {
            get
            {
                if ( instance == null )
                {
                    instance = new ConnManager();
                }
                return instance;
            }
        }
        public PostProcessor PostProc { get; set; }

        public bool Send( Server serv , string message )
        {

            if ( serv.Name == null )
            {
                foreach ( var item in Sockets.Values )
                {
                    item.Write( message );
                }
            }
            if ( Sockets.ContainsKey( serv ) )
            {
                return Sockets[ serv ].Write( message );
            }
            else
            {
                return false;
            }
        }

        public void Receive( Server serv , string message )
        {
            while(PostProc == null) {}
            PostProc.Receive( serv , message );
        }

        public void Connect()
        {
            if ( Sockets == null )
            {
                Sockets = new Dictionary<Server , Connection>();
            }
            foreach ( var item in ServerList )
            {
                if ( Sockets != null )
                {
                    Sockets[ item ] = new Connection( item , this );
                }

            }
        }
    }
}
