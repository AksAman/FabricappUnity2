using System;
using System.Collections;
using System.Collections.Generic;
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

