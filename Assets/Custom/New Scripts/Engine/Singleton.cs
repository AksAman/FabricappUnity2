using System;
using UnityEngine;

namespace helloVoRld.NewScripts.Engine
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        // In multithreading environment, _instance==null condition may give rise to errors
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var objs = FindObjectsOfType(typeof(T)) as T[];

                    switch (objs.Length)
                    {
                        case 0:
                            GameObject obj = new GameObject
                            {
                                name = string.Format("_{0}", typeof(T).Name)
                            };
                            _instance = obj.AddComponent<T>();
                            break;

                        case 1:
                            _instance = objs[0];
                            break;

                        default:
                            throw new Exception("There are more than one " + typeof(T).Name + " in the scene");
                    }
                }

                return _instance;
            }
        }
    }
}