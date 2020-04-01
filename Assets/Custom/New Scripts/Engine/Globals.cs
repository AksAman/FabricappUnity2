using helloVoRld.NewScripts.Catalogue;
using System.Collections.Generic;

namespace helloVoRld
{
    public static class Globals
    {
        public static readonly string IP = @"http://hvrplfabricapp.ml";
        public static readonly string AllCataloguesIP = IP + @"/fabricapp/api/catalogues";
        public static readonly string AllFabricsIP = IP + @"/fabricapp/api/fabrics";
        public static readonly List<CatalogueModel> Catalogues = new List<CatalogueModel>();

        public static string FabricIP(int catIndex)
        {
            return IP + @"/fabricapp/api/fabrics?catpk=" + catIndex;
        }
    }
}
