using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserManagement;
using DTO;
using System.IO;

namespace Modules
{
    class TellMod : Module
    {
        private Dictionary<string , List<string>> tells = new Dictionary<string , List<string>>();

        public TellMod()
            : base()
        {
            ReadTells();
        }

        private void ReadTells()
        {
            try
            {
                Console.WriteLine( "begun reading" );
                StreamReader streamReader = new StreamReader( "C:/YellingBird/Test/tells.txt" );
                string text = streamReader.ReadToEnd();
                streamReader.Close();
                Console.WriteLine( "done reading" );
                string[] textparts = text.Split( new string[] { "\r\n" } , StringSplitOptions.RemoveEmptyEntries );
                foreach ( string p in textparts )
                {
                    string[] pparts = p.Split( new char[] { ':' } , 2 );
                    if ( pparts.Length == 2 )
                    {
                        pparts[ 0 ] = pparts[ 0 ].Trim();
                        pparts[ 1 ] = pparts[ 1 ].Trim();
                        if ( tells.ContainsKey( pparts[ 0 ] ) )
                        {
                            tells[ pparts[ 0 ] ].Add( pparts[ 1 ] );
                        }
                        else
                        {
                            tells[ pparts[ 0 ] ] = new List<string>();
                            tells[ pparts[ 0 ] ].Add( pparts[ 1 ] );
                        }
                    }
                }
                Console.WriteLine( "done splitting" );
            }
            catch(Exception e)
            {
                Console.WriteLine( e.Message );
            }
        }

        private void WriteTells()
        {
            string text = "";
            foreach ( string user in tells.Keys )
            {
                foreach ( string mess in tells[ user ] )
                {
                    text += user + ":" + mess + "\r\n";
                }
            }
            StreamWriter streamWriter = new StreamWriter( "C:/YellingBird/Test/tells.txt", false );
            streamWriter.Write( text );
            streamWriter.Close();

        }

        public override void Incoming( Message mgs )
        {
            if ( mgs.MUser != null )
            {
                if ( tells.ContainsKey( mgs.MUser.Nick ) )
                {
                    foreach ( string s in tells[ mgs.MUser.Nick ] )
                    {
                        Reply( mgs.MUser.Nick + ": " + s , MessageType.Message );
                    }
                    tells.Remove( mgs.MUser.Nick );
                    WriteTells();
                }
                if ( mgs.Line.StartsWith( "!ytell " , StringComparison.OrdinalIgnoreCase ) )
                {
                    string[] lineparts = mgs.Line.Split( new char[] { ' ' } , 3 );
                    if ( lineparts.Length == 3 )
                    {
                        if ( tells.ContainsKey( lineparts[ 1 ] ) )
                        {
                            if ( tells[ lineparts[ 1 ] ].Count < 10 )
                            {
                                tells[ lineparts[ 1 ] ].Add( mgs.Time.Hour + ":" + mgs.Time.Minute + " <" + mgs.MUser.Nick + "> " + lineparts[ 2 ] );
                                Reply( "I'll pass that on when " + lineparts[ 1 ] + " is around." , MessageType.Message );
                            }
                            else
                            {
                                Reply( "Sorry, there are already too much messages in the queue for " + lineparts[ 1 ] + "." , MessageType.Message );
                            }
                        }
                        else
                        {
                            tells[ lineparts[ 1 ] ] = new List<string>();
                            tells[ lineparts[ 1 ] ].Add( mgs.Time.Hour + ":" + mgs.Time.Minute + " <" + mgs.MUser.Nick + "> " + lineparts[ 2 ] );
                            Reply( "I'll pass that on when " + lineparts[ 1 ] + " is around." , MessageType.Message );
                        }
                        WriteTells();
                    }
                }
            }
        }
    }
}
