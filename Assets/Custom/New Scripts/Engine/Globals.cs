using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace helloVoRld
{
    public static class Globals
    {
        public static readonly string IP = @"http://hvrplfabricapp.ml";
        public static readonly string AllCataloguesIP = IP + @"/fabricapp/api/catalogues";
        public static readonly string AllFabricsIP = IP + @"/fabricapp/api/fabrics";

        public static string FabricIP(int catIndex) => IP + @"/fabricapp/api/fabrics?catpk=" + catIndex;
    }
}
