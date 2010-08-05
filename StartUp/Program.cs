using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConnectionManager;
using DTO;
using Modules;

namespace StartUp
{
    class Program
    {
        static void Main( string[] args )
        {
            Hub.Instance = new Hub();
            Module newone = ObjectLoader.ReloadObject( "ModuleStarterMod" );
            if ( newone != null )
            {
                Console.WriteLine( "Loaded all Modules" );
            }
        }
    }
}
