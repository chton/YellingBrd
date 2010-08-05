using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO
{
    class UnknownChannelException: Exception
    {
        Server server;
        Channel channel;

        public UnknownChannelException( Server server , Channel channel ) : base()
        {
            this.server = server;
            this.channel = channel;
        }
    }
}
