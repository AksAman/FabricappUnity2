using helloVoRld.Networking;
using helloVoRld.NewScripts.UI;
using helloVoRld.NewScripts.Catalogue;
using UnityEngine;

namespace helloVoRld.NewScripts.Fabric
{
    /// <summary>
    /// Extracted JSON Fabric Model
    /// </summary>
    public class FabricModel : Model<FabricWebModel>
    {
        /// <summary>
        /// Fabric WEB Id
        /// </summary>
        public readonly int WebID;
        /// <summary>
        /// Fabric Title
        /// </summary>
        public readonly string Title;
        /// <summary>
        /// Parent Catalogue, which contains current fabric
        /// </summary>
        public readonly CatalogueModel Catalogue;

        /// <summary>
        /// Material WEB index associated with current fabric
        /// </summary>
        public readonly int MaterialIndex;

        /// <summary>
        ///  This is assumed to be image url, not JSON string
        /// </summary>
        public readonly string MainTexURL;

        protected override FixedCountDownloader TextureDownloader => FabricView.Instance.TextureDownloader;

        public FabricModel(FabricWebModel model) : base(model.f_thumbnail_url, model.modified)
        {
            WebID = model.id;
            Title = model.f_name;
            Catalogue = CatalogueView.Instance.ModelList.Find((cat) => cat.WebID == model.id);
            MaterialIndex = model.f_material;
            MainTexURL = Globals.IP + model.f_thumbnail_url;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
