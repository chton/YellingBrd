using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO
{
    public class Server
    {
        private List<User> allusers = new List<User>();
        private List<Channel> chans = new List<Channel>();
        public List<Channel> Channels { get { return chans; } set { chans = value; } }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string MyNick { get; set; }
        public string Ident { get; set; }
        public string MyDescription { get; set; }
        public List<User> AllUsers { get { return allusers;  } set{ allusers = value;} }

    }
}
