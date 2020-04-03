using helloVoRld.Networking.RestClient;
using helloVoRld.NewScripts.UI;
using System.Collections.Generic;
using helloVoRld.NewScripts.Fabric;
using UnityEngine;

namespace helloVoRld.NewScripts.Furniture
{
    public class FurnitureModel : Model<FurnitureWebModel>
    {
        public string Name;
        public List<helloVoRld.Test.Databases.AllowedParts> allowedParts = new List<helloVoRld.Test.Databases.AllowedParts>();
        public int meshForMaterialUpdate = 0;
        public int materialIndexToChange;
        public GameObject Prefab;

        public FurnitureModel(FurnitureWebModel model) : base("")
        {

        }

        public FurnitureModel(Test.Databases.Model Model) : base("")
        {
            Name = Model.modelName;
            ThumbnailSprite = Model.modelThumbnail;
            allowedParts = Model.allowedParts;
            meshForMaterialUpdate = Model.meshForMaterialUpdate;
            materialIndexToChange = Model.materialIndexToChange;
            Prefab = Model.modelPrefab;
        }
        
        protected override FixedCountDownloader TextureDownloader => CatalogueView.Instance.TextureDownloader;

        public override string ToString()
        {
            return Name;
        }
    }
}
