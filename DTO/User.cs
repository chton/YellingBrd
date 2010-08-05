using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO
{
    public class User
    {
        public string Nick { get; set; }
        public string RealName { get; set; }
        public string Ident { get; set; }
        public string HostName { get; set; }
        public Server UServer { get; set; }

    }
}
