using helloVoRld.Networking.RestClient;
using helloVoRld.NewScripts.UI;

namespace helloVoRld.NewScripts.Catalogue
{
    public class CatalogueModel : Model<CatalogueWebModel>
    {
        public readonly int WebID;
        public readonly string Name;
        public readonly string Description;
        public readonly string ManufacturerName;



        public CatalogueModel(CatalogueWebModel model) : base(model.c_thumbnail_url)
        {
            WebID = model.id;
            Name = model.c_name;
            Description = model.c_description;
            ManufacturerName = model.c_manufacturer_name;
        }

        protected override FixedCountDownloader TextureDownloader => CatalogueView.Instance.TextureDownloader;

        public override string ToString()
        {
            return Name + " by " + ManufacturerName;
        }
    }
}