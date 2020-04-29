namespace helloVoRld.NewScripts.Catalogue
{
    /// <summary>
    /// Layout of JSON file for catalogue entry
    /// </summary>
    public class CatalogueWebModel : IWebModel
    {
        public int id;
        public string c_name;
        public string c_description;
        public string c_thumbnail_url;
        public string c_manufacturer_name;
        public string c_normal_map;
        public string modified;
    }
}
