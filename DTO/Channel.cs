using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO
{
    public class Channel
    {
        private List<User> cusers = new List<User>();
        public Server CServer { get; set; }
        public String CName { get; set; }
        public String CFullName { get; set; }
        public String Topic { get; set; }
        public List<User> CUsers
        {
            get {
                return cusers;
            }
            set
            {
                cusers = value;
            }
        }
    }
}
