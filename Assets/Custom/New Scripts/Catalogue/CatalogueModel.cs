using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using helloVoRld.Networking.RestClient;
using helloVoRld.NewScripts.Engine;
using helloVoRld.NewScripts;

namespace helloVoRld.NewScripts.Catalogue
{
    public class CatalogueModel : Model<CatalogueWebModel>
    {
        public readonly int WebID;
        public readonly string Name;
        public readonly string Description;
        public readonly string ManufacturerName;

        protected override FixedCountDownloader TextureDownloader => CatalogueManager.Instance.TextureDownloader;

        public CatalogueModel(CatalogueWebModel model) : base(model.c_thumbnail_url)
        {
            WebID = model.id;
            Name = model.c_name;
            Description = model.c_description;
            ManufacturerName = model.c_manufacturer_name;
        }

        public override string ToString()
        {
            return Name + " by " + ManufacturerName;
        }
    }
}