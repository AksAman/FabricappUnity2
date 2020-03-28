using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using helloVoRld.NewScripts.Engine;
using UnityEngine;
using helloVoRld.Networking.RestClient;

namespace helloVoRld.NewScripts
{
    public abstract class Manager<T, IWebModel, Model, Button> : Singleton<T>
        where T : Component
        where Model : Model<IWebModel>
        where IWebModel : NewScripts.IWebModel
        where Button : ButtonModel<Model, IWebModel>
    {
        [Header("Pooling")]
        public GameObject ObjectToPool;

        public FixedCountDownloader TextureDownloader { get; set; }
        protected List<Model> ModelList { get; private set; } = new List<Model>();

        public void AddModels(IEnumerable<Model> models) => ModelList.AddRange(models);
        public void AddModel(Model model) => ModelList.Add(model);

        void Awake()
        {
            TextureDownloader = new FixedCountDownloader(this);

            if (!ObjectToPool.TryGetComponent<Button>(out _))
            {
                throw new Exception("Button does not have corresponding ButtonScript");
            }
        }

        public abstract void OnModelButtonClick(int index);
    }
}