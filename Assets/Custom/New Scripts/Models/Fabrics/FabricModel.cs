using helloVoRld.Networking;
using helloVoRld.NewScripts.UI;
using helloVoRld.NewScripts.Catalogue;
using UnityEngine;

namespace helloVoRld.NewScripts.Fabric
{
    public class FabricModel : Model<FabricWebModel>
    {
        public readonly int WebID;
        public readonly string Title;
        public readonly CatalogueModel Catalogue;

        public readonly int MaterialIndex;
        // This is assumed to be image url, not JSON string
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
