using helloVoRld.Utilities.Debugging;
using System.Collections.Generic;
using UnityEngine;

namespace helloVoRld.Core.Pooling
{
    public class ObjectPooler : MonoBehaviour
    {
        #region Variables
        public GameObject prefabToPool;
        public bool isPoolInitialized = false;

        public Stack<GameObject> inactiveInstancesStack = new Stack<GameObject>();
        #endregion


        #region Main functions

        public void InitializePool(int count)
        {
            while (count > 0)
            {
                CreatePooledObject();
                count--;
            }
            isPoolInitialized = true;
        }

        #endregion


        #region Helper Functions

        private GameObject CreatePooledObject()
        {
            GameObject spawnedObject = Instantiate(prefabToPool, this.transform);
            spawnedObject.SetActive(false);
            spawnedObject.AddComponent<PooledObject>().Pooler = this;

            inactiveInstancesStack.Push(spawnedObject);

            return spawnedObject;
        }

        public GameObject GetObject()
        {
            GameObject objectToReturn;

            if (inactiveInstancesStack.Count > 0)
            {
                objectToReturn = inactiveInstancesStack.Pop();
            }

            else
            {
                objectToReturn = CreatePooledObject();
            }

            objectToReturn.transform.SetParent(null);
            objectToReturn.SetActive(true);
            return objectToReturn;
        }

        public bool ReturnObject(GameObject returnedObject)
        {
            PooledObject pooledObject = returnedObject.GetComponent<PooledObject>();

            if (pooledObject != null)
            {
                if (pooledObject.Pooler == this)
                {
                    returnedObject.transform.SetParent(this.transform);
                    returnedObject.SetActive(false);

                    inactiveInstancesStack.Push(returnedObject);
                    return true;
                }

                else
                {
                    DebugHelper.LogWarning(returnedObject.name + " was returned to a different pool, destroying it!");
                    Destroy(returnedObject);
                    return false;
                }
            }
            else
            {
                DebugHelper.LogError(returnedObject.name + " has no pooled object component on it!");
                return false;
            }
        }

        #endregion
    }
}

