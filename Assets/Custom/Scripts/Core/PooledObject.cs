using UnityEngine;

namespace helloVoRld.Core.Pooling
{
    public class PooledObject : MonoBehaviour
    {
        #region Variables
        private ObjectPooler pooler;

        public ObjectPooler Pooler { get => pooler; set => pooler = value; }
        #endregion


    }
}

