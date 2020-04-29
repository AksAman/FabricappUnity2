using helloVoRld.Networking;
using helloVoRld.NewScripts.UI;
using System.Collections.Generic;
using helloVoRld.NewScripts.Fabric;

namespace helloVoRld.NewScripts.Catalogue
{
    /// <summary>
    /// Extracted JSON Catalogue Model
    /// </summary>
    public class CatalogueModel : Model<CatalogueWebModel>
    {
        /// <summary>
        /// Catalogue WEB ID
        /// </summary>
        public readonly int WebID;
        /// <summary>
        /// Catalogue Name
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Catalogue Info
        /// </summary>
        public readonly string Description;
        /// <summary>
        /// Catalogue Manufacturer
        /// </summary>
        public readonly string ManufacturerName;

        /// <summary>
        /// Normal map url assocuated with current catalogue
        /// </summary>
        public readonly string NormalMapURL;

        /// <summary>
        /// Fabrics List attached to current Catalogue
        /// </summary>
        public readonly List<FabricModel> FabricList;

        public CatalogueModel(CatalogueWebModel model) : base(model.c_thumbnail_url, model.modified)
        {
            WebID = model.id;
            Name = model.c_name;
            Description = model.c_description;
            ManufacturerName = model.c_manufacturer_name;
            NormalMapURL = Globals.IP + model.c_normal_map;

            FabricList = new List<FabricModel>();
        }

        protected override FixedCountDownloader TextureDownloader => CatalogueView.Instance.TextureDownloader;

        public override string ToString()
        {
            return Name + " by " + ManufacturerName;
        }
    }
}