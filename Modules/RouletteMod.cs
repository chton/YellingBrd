using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO;
using System.Threading;

namespace Modules
{

    class RouletteMod : Module
    {
        private Random rnd;
        public override void Incoming( Message mgs )
        {
            if ( mgs.Line.StartsWith( "!rr3 " ) && mgs.MChannel !=  null && mgs.MUser != null )
            {
                string l = mgs.Line.Split( new char[] { ' ' } , 2 )[1].Trim();
                User target = ( from User u in mgs.MChannel.CUsers where u.Nick.Equals( l , StringComparison.OrdinalIgnoreCase ) select u ).FirstOrDefault();
                if ( target == null )
                {
                    Reply( "No such user in channel" , MessageType.Message );
                }
                else if ( target.Nick == mgs.MServer.MyNick )
                {
                    Reply( "Sorry, I won't play." , MessageType.Message );
                }
                else if ( target == mgs.MUser )
                {
                    Reply( "Russian roulette is a 2 person game man, piss of some more people!" , MessageType.Message );
                }
                else
                {
                    Reply( "*RR* " + mgs.MUser.Nick + " challenged " + target.Nick + " to russian roulette." , MessageType.Message );
                    rnd = new Random();
                    int i = 0;
                    while ( i < 6 )
                    {
                        Thread.Sleep( 500 );
                        if ( i % 2 == 0 )
                        {
                            if ( UserGetsKicked( i ) )
                            {
                                Reply( "*RR* " + mgs.MUser.Nick + " takes the gun : *BANG*" , MessageType.Message );
                                KickUser( "You're dead." , mgs.MUser , mgs.MChannel );
                                return;
                            }
                            else
                            {
                                Reply( "*RR* " + mgs.MUser.Nick + " takes the gun : *Click*" , MessageType.Message );
                            }
                        }
                        else
                        {
                            if ( UserGetsKicked( i ) )
                            {
                                Reply( "*RR* " + target.Nick + " takes the gun : *BANG*" , MessageType.Message );
                                KickUser( "You're dead." , target , mgs.MChannel );
                                return;
                            }
                            else
                            {
                                Reply( "*RR* " + target.Nick + " takes the gun : *Click*" , MessageType.Message );
                            }
                        }
                        i++;
                    }
                    Reply( "*RR* " + mgs.MUser.Nick + " takes the gun : *BANG*" , MessageType.Message );
                    KickUser( "You're dead." , mgs.MUser , mgs.MChannel );
                }

            }
        }

        private bool UserGetsKicked(int round)
        {
            int x = rnd.Next( round , 6 );
            return x == 5;
        }

        private void KickUser( string message , User us , Channel chan )
        {
            Message msg = new Message();
            msg.Line = message;
            msg.MChannel = chan;
            msg.MType = MessageType.Kick;
            msg.MUser = us;
            msg.MServer = LastMessage.MServer;
            Reply( msg );
        }


    }
}
