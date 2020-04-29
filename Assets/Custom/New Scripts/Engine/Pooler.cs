using helloVoRld.Utilities.Debugging;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace helloVoRld.NewScripts.Engine
{
    /// <summary>
    /// Base Class for managing 'Model' butons on scrollviewer
    /// </summary>
    /// <typeparam name="T">Button Prefab</typeparam>
    /// <typeparam name="U">Button Model</typeparam>
    /// <typeparam name="V">Web Data Model</typeparam>
    public class Pooler<T, U, V>
        where T : ButtonModel<U, V>
        where U : Model<V>
        where V : IWebModel
    {
        /// <summary>
        /// Reference for the scrollviewer, in which buttons will be filled
        /// </summary>
        private readonly RectTransform ScrollViewer;
        /// <summary>
        /// Button reference, used for creating new objects
        /// </summary>
        private readonly T PrefabToPool;
        /// <summary>
        /// Current list of objects in scrollviewer, and those that are hidden due to less count than any previous case
        /// </summary>
        public readonly List<T> AllObjects = new List<T>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ButtonUIToPool">Button Prefab Reference</param>
        /// <param name="ScrollTransfrom">Reference to ScrollViewer in which buttons will be filled</param>
        public Pooler(T ButtonUIToPool, RectTransform ScrollTransfrom)
        {
            PrefabToPool = ButtonUIToPool != null ? ButtonUIToPool : throw new ArgumentNullException(nameof(ButtonUIToPool), "Gameobject for Pooler is null");
            ScrollViewer = ScrollTransfrom != null ? ScrollTransfrom : throw new ArgumentNullException(nameof(ScrollTransfrom), "Gameobject for Pooler is null");
        }

        /// <summary>
        /// Fill the scroll Viewer
        /// </summary>
        /// <param name="Count">Number of buttons to display in ScrollViewer</param>
        /// <param name="OnObjectCreated">Callback for each button that will be initialized, first parameter denoting index in scrollviewer, and second is button itself</param>
        public void FillViewer(int Count, Action<int, T> OnObjectCreated)
        {
            // It is must that ClearViewer() is called before this method.
            // More buttons are requested than in current list
            while (Count > AllObjects.Count)
            {
                GameObject spawnedObject = UnityEngine.Object.Instantiate(PrefabToPool.gameObject, ScrollViewer);
                spawnedObject.GetComponent<RectTransform>().localScale = Vector3.one;
                AllObjects.Add(spawnedObject.GetComponent<T>());
            }

            // Add buttons to scrollViewer and request callbacks
            for (int i = 0; i < Count; ++i)
            {
                AllObjects[i].transform.SetParent(ScrollViewer);
                AllObjects[i].gameObject.SetActive(true);
                OnObjectCreated(i, AllObjects[i]);
            }
        }

        /// <summary>
        /// Must be called on when Scrollviewer is not visible on screen
        /// </summary>
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