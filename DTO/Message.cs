using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO
{
    public class Message
    {
        public String Line { get; set; }
        public String RawLine { get; set; }
        public Channel MChannel { get; set; }
        public User MUser { get; set; }
        public MessageType MType { get; set; }
        public DateTime Time { get; set; }
        public Server MServer
        {
            get;
            set; 
        }

    }

    public enum MessageType
    {
        Message ,
        Action ,
        Join ,
        Part ,
        Quit ,
        Kick ,
    }

}
