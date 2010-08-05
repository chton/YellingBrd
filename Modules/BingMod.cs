using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConnectionManager;
using DTO;
using Modules.BingService;

namespace Modules
{
    class BingMod : Module
    {
        LiveSearchService lss;
        public BingMod() : base()
        {
            lss = new LiveSearchService();
        }

        public override void Incoming( Message mgs )
        {
            string line = mgs.Line;
            if ( line.StartsWith( "!b " ) || (mgs.MType == MessageType.Action && line.StartsWith("googles ", StringComparison.OrdinalIgnoreCase)))
            {
                line = line.Replace( "!b " , "" );
                if ( mgs.MType == MessageType.Action && line.StartsWith( "googles " , StringComparison.OrdinalIgnoreCase ) )
                {
                    line = line.Replace( "googles ", "" );
                }
                SearchRequest sr = new SearchRequest();
                sr.AppId = "msappidhere";
                sr.Query = line;

                sr.Sources = new SourceType[] { SourceType.Web };

                SearchResponse resp = lss.Search( sr );
                if ( resp.Web.Results != null && resp.Web.Results.Length > 0 )
                {
                    WebResult result = resp.Web.Results.FirstOrDefault();
                    if ( result != null )
                    {
                        if ( mgs.MType == MessageType.Action )
                        {
                            Reply( "googled that for you: " + result.Title + " || " + result.Url );
                        }
                        else
                        {
                            Reply( "Your result: " + result.Title + " || " + result.Url );
                        }
                    }
                }
                else
                {
                    if ( mgs.MType == MessageType.Action )
                    {
                        Reply( "couldn't find that." );
                    }
                    else
                    {
                        Reply( "No results found!" );
                    }

                }
            }
            if ( line.StartsWith( "!v " ) )
            {
                line = line.Replace( "!v " , "" );
                SearchRequest sr = new SearchRequest();
                sr.AppId = "msappidhere";
                sr.Query = line;

                sr.Sources = new SourceType[] { SourceType.Video };

                SearchResponse resp = lss.Search( sr );
                if ( resp.Video.Results != null && resp.Video.Results.Length > 0 )
                {
                    VideoResult result = resp.Video.Results.FirstOrDefault();
                    if ( result != null )
                    {
                        Reply("Your video: " + result.Title + " || " + result.PlayUrl);
                    }
                }
                else
                {
                    Reply("No videos found!");
                }
            }
            if ( line.StartsWith( "!i " ) )
            {
                line = line.Replace( "!i " , "" );
                SearchRequest sr = new SearchRequest();
                sr.AppId = "msappidhere";
                sr.Query = line;

                sr.Sources = new SourceType[] { SourceType.Image };

                SearchResponse resp = lss.Search( sr );
                if ( resp.Image.Results != null && resp.Image.Results.Length > 0 )
                {
                    ImageResult result = resp.Image.Results.FirstOrDefault();
                    if ( result != null )
                    {
                        Reply("Your image: " + result.Title + " || " + result.MediaUrl);
                    }
                }
                else
                {
                   Reply("No images found!");
                }
            }
        }
    }
}
