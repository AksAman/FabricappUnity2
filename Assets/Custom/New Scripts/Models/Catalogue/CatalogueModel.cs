using helloVoRld.Networking;
using helloVoRld.NewScripts.UI;
using System.Collections.Generic;
using helloVoRld.NewScripts.Fabric;

namespace helloVoRld.NewScripts.Catalogue
{
    public class CatalogueModel : Model<CatalogueWebModel>
    {
        public readonly int WebID;
        public readonly string Name;
        public readonly string Description;
        public readonly string ManufacturerName;

        public readonly string NormalMapURL;

        public readonly List<FabricModel> FabricList;

        public CatalogueModel(CatalogueWebModel model) : base(model.c_thumbnail_url, model.modified)
        {
            WebID = model.id;
            Name = model.c_name;
            Description = model.c_description;
            ManufacturerName = model.c_manufacturer_name;
            NormalMapURL = model.c_normal_map;

            FabricList = new List<FabricModel>();
        }

        protected override FixedCountDownloader TextureDownloader => CatalogueView.Instance.TextureDownloader;

        public override string ToString()
        {
            return Name + " by " + ManufacturerName;
        }
    }
}