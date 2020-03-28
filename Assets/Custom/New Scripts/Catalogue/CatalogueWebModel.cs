using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace helloVoRld.NewScripts.Catalogue
{
    public class CatalogueWebModel : IWebModel
    {
        public int id;
        public string c_name;
        public string c_description;
        public string c_thumbnail_url;
        public string c_manufacturer_name;
        public string c_normal_map;
    }
}
