using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.IO;

namespace Modules
{
    public class ObjectLoader
    {
        public static string CSPATH = "C:/YellingBird/Modules/";

        public static Module ReloadObject( string csname )
        {
            // *** Example form input has code in a text box
            StreamReader streamReader = new StreamReader( CSPATH + csname + ".cs" );
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            ICodeCompiler loCompiler = new CSharpCodeProvider( new Dictionary<string , string>() { { "CompilerVersion" , "v3.5" } } ).CreateCompiler();
            CompilerParameters loParameters = new CompilerParameters();
            // *** Start by adding any referenced assemblies
            loParameters.ReferencedAssemblies.Add( "System.dll" );
            loParameters.ReferencedAssemblies.Add( "System.Windows.Forms.dll" );
            loParameters.ReferencedAssemblies.Add( "Modules.dll" );
            loParameters.ReferencedAssemblies.Add( "Settings.dll" );
            loParameters.ReferencedAssemblies.Add( "DTO.dll" );
            loParameters.ReferencedAssemblies.Add( "System.Data.dll" );
            loParameters.ReferencedAssemblies.Add( "System.Core.dll" );
            loParameters.ReferencedAssemblies.Add( "ConnectionManager.dll" );
            loParameters.ReferencedAssemblies.Add( "UserManagement.dll" );
            loParameters.ReferencedAssemblies.Add( "System.Web.Services.dll" );
            loParameters.ReferencedAssemblies.Add( "System.EnterpriseServices.dll" );
            loParameters.ReferencedAssemblies.Add( "System.Xml.dll" );
            loParameters.ReferencedAssemblies.Add( "System.Data.DataSetExtensions.dll" );
            // *** Load the resulting assembly into memory
            loParameters.GenerateInMemory = false;
            // *** Now compile the whole thing
            CompilerResults loCompiled = loCompiler.CompileAssemblyFromSource( loParameters , text );
            if ( loCompiled.Errors.HasErrors )
            {
                string lcErrorMsg = "";
                lcErrorMsg = loCompiled.Errors.Count.ToString() + " Errors:";
                for ( int x = 0 ; x < loCompiled.Errors.Count ; x++ )
                    lcErrorMsg = lcErrorMsg + "\r\nLine: " + loCompiled.Errors[ x ].Line.ToString() + " - " + loCompiled.Errors[ x ].ErrorText;
                Console.WriteLine( lcErrorMsg + " :: " + csname , "Compiler Demo" );
                return null;

            }
            Assembly loAssembly = loCompiled.CompiledAssembly;
            // *** Retrieve an obj ref – generic type only
            object loObject = loAssembly.CreateInstance( "Modules." + csname );
            if ( loObject == null )
            {
                Console.WriteLine( "Couldn't load class." );
                return null;
            }
            return loObject as Module;
        }
    }
}
