using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO;
using UserManagement;

namespace ConnectionManager
{
    public class Hub
    {
        private PreProcessor Pre { get; set; }
        private PostProcessor Post { get; set; }
        private UserManager usm { get; set; }

        public Hub()
        {
            Pre = new PreProcessor();
            Post = new PostProcessor(this);
            usm = new UserManager();
        }
        public static Hub Instance { get; set; }
        public delegate void MessageHandler(Message args);

        public event MessageHandler MessageReceived;

        public void Receive( Message msg )
        {
            if ( MessageReceived != null)
            {
                MessageReceived.Invoke( msg );
            }
            //Console.WriteLine( msg.MServer.Name + " - " + msg.Line );
        }


        public bool Send( Message mgs )
        {
            return Pre.Send( mgs );
        }
    }
}
