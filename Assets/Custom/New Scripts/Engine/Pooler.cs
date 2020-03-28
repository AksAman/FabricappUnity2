using helloVoRld.Utilities.Debugging;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace helloVoRld.NewScripts.Engine
{
    public class Pooler
    {
        private readonly GameObject PrefabToPool;
        private readonly List<GameObject> AllObjects = new List<GameObject>();
        private readonly List<bool> IsIdle = new List<bool>();

        public Pooler(GameObject g)
        {
            PrefabToPool = g != null ? g : throw new ArgumentNullException("g", "Gameobject for Pooler is null");
        }

        public GameObject GetObject(Transform Parent)
        {
            int index = IsIdle.IndexOf(true);
            if (index != -1)
            {
                IsIdle[index] = false;
                return AllObjects[index];
            }

            GameObject spawnedObject = UnityEngine.Object.Instantiate(PrefabToPool, Parent);
            spawnedObject.SetActive(true);

            AllObjects.Add(spawnedObject);
            IsIdle.Add(false);

            return spawnedObject;
        }

        public bool ReturnToPool(GameObject obj)
        {
            int index = AllObjects.IndexOf(obj);

            if (index == -1)
            {
                UnityEngine.Object.Destroy(obj);
                DebugHelper.LogWarning(obj.name + " was returned to a different pool, destroying it!");
                return false;
            }

            obj.SetActive(false);
            IsIdle[index] = true;
            return true;
        }
    }
}