using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO;

namespace ConnectionManager
{
    class ServerFactory
    {
        public static List<Server> GetServers()
        {
            List<Server> newList = new List<Server>();
            Server s = new Server();
            s.Name = "WiNA";
            s.Address = "wina.ugent.be";
            s.Ident = "TangBot";
            s.MyNick = "YellingBird";
            s.MyDescription = "I'm AWESOME";
            s.Port = 6667;
            s.Channels = new List<Channel>();

            Channel c = new Channel();
            c.CName = "zeus";
            c.CFullName = "#zeus";
            c.CServer = s;
           // s.Channels.Add(c);

            c = new Channel();
            c.CName = "YellingBird";
            c.CFullName = "#YellingBird";
            c.CServer = s;
            s.Channels.Add( c );

            newList.Add( s );
            return newList;
        }
    }
}
