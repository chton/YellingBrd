using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserManagement;
using System.IO;

namespace Modules
{
    public class ModuleStarterMod : Module
    {
        List<Module> loaded = new List<Module>();   

        public ModuleStarterMod()
            : base()
        {
            StartModules();
        }

        public void StartModules()
        {
            string[] files = Directory.GetFiles( ObjectLoader.CSPATH );
            foreach ( string path in files )
            {
                string filename = path.Replace( ObjectLoader.CSPATH , "" );
                if ( filename.EndsWith( "mod.cs" , StringComparison.OrdinalIgnoreCase ) && !filename.Contains( "ModuleStarter"  ) )
                {
                    filename = filename.Replace( ".cs" , "" );
                    Module mod = ObjectLoader.ReloadObject( filename );
                    loaded.Add( mod );
                }
            }
        }

        public override void Incoming( DTO.Message mgs )
        {
            if ( UserManager.HasPermission( Permissions.Admin , mgs.MUser ) )
            {
                if ( mgs.Line.StartsWith( "!reload" ) )
                {
                    ReloadMods();
                }
            }
        }

        private void ReloadMods()
        {
            foreach ( Module mod in loaded )
            {
                if ( mod != null )
                    mod.Dispose();
            }
            Module newone = ObjectLoader.ReloadObject( "ModuleStarterMod" );
            if ( newone != null )
            {
                Reply( "reloaded all Modules" );
                this.Dispose();
            }
            else
            {
                Reply( "Error occured! Modules not reloaded!" );
            }
        }
    }
}
