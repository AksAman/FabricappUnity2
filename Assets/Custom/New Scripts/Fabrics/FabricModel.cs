using helloVoRld.Networking.RestClient;
using helloVoRld.NewScripts.UI;
using helloVoRld.NewScripts.Catalogue;


namespace helloVoRld.NewScripts.Fabric
{
    public class FabricModel : Model<FabricWebModel>
    {
        public int WebID;
        public string Title;
        public CatalogueModel Catalogue;

        protected override FixedCountDownloader TextureDownloader => FabricView.Instance.TextureDownloader;

        public FabricModel(FabricWebModel model) : base(model.f_thumbnail_url)
        {
            WebID = model.id;
            Title = model.f_name;
            Catalogue = CatalogueView.Instance.ModelList.Find((cat) => cat.WebID == model.id);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
