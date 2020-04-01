using helloVoRld.Utilities.Debugging;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace helloVoRld.NewScripts.Engine
{
    public class Pooler
    {
        private readonly RectTransform ScrollViewer;
        private readonly GameObject PrefabToPool;

        private readonly List<GameObject> AllObjects = new List<GameObject>();

        public Pooler(GameObject ButtonUIToPool, RectTransform ScrollTransfrom)
        {
            PrefabToPool = ButtonUIToPool != null ? ButtonUIToPool : throw new ArgumentNullException(nameof(ButtonUIToPool), "Gameobject for Pooler is null");
            ScrollViewer = ScrollTransfrom != null ? ScrollTransfrom : throw new ArgumentNullException(nameof(ScrollTransfrom), "Gameobject for Pooler is null");
        }

        public void FillViewer(int Count, Action<int, GameObject> OnObjectCreated)
        {
            while (Count > AllObjects.Count)
            {
                GameObject spawnedObject = UnityEngine.Object.Instantiate(PrefabToPool, ScrollViewer);
                spawnedObject.GetComponent<RectTransform>().localScale = Vector3.one;
                AllObjects.Add(spawnedObject);
            }

            for (int i = 0; i < Count; ++i)
            {
                AllObjects[i].SetActive(true);
                OnObjectCreated(i, AllObjects[i]);
            }
        }

        public  void ClearViewer()
        {
            foreach (var x in AllObjects)
                x.SetActive(false);
        }
    }
}