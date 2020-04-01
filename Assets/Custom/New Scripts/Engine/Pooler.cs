using helloVoRld.Utilities.Debugging;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace helloVoRld.NewScripts.Engine
{
    public class Pooler<T, U, V>
        where T : ButtonModel<U, V>
        where U : Model<V>
        where V : IWebModel
    {
        private readonly RectTransform ScrollViewer;
        private readonly T PrefabToPool;

        public readonly List<T> AllObjects = new List<T>();

        public Pooler(T ButtonUIToPool, RectTransform ScrollTransfrom)
        {
            PrefabToPool = ButtonUIToPool != null ? ButtonUIToPool : throw new ArgumentNullException(nameof(ButtonUIToPool), "Gameobject for Pooler is null");
            ScrollViewer = ScrollTransfrom != null ? ScrollTransfrom : throw new ArgumentNullException(nameof(ScrollTransfrom), "Gameobject for Pooler is null");
        }

        public void FillViewer(int Count, Action<int, T> OnObjectCreated)
        {
            while (Count > AllObjects.Count)
            {
                GameObject spawnedObject = UnityEngine.Object.Instantiate(PrefabToPool.gameObject, ScrollViewer);
                spawnedObject.GetComponent<RectTransform>().localScale = Vector3.one;
                AllObjects.Add(spawnedObject.GetComponent<T>());
            }
            /*
            while (Count < AllObjects.Count)
            {
                UnityEngine.Object.Destroy(AllObjects[AllObjects.Count - 1]);
                AllObjects.RemoveAt(AllObjects.Count - 1);
            }*/

            for (int i = 0; i < Count; ++i)
            {
                AllObjects[i].transform.SetParent(ScrollViewer);
                AllObjects[i].gameObject.SetActive(true);
                OnObjectCreated(i, AllObjects[i]);
            }
        }

        public void ClearViewer()
        {
            foreach (var x in AllObjects)
            {
                x.UnloadOject();
                x.gameObject.SetActive(false);
            }
        }
    }
}