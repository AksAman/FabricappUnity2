using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace helloVoRld.Networking.RestClient
{
    public class FixedCountDownloader
    {
        readonly IEnumerator[] Routines = new IEnumerator[5];
        readonly Queue<(string, Action<Sprite>, Action<float>)> Queue = new Queue<(string, Action<Sprite>, Action<float>)>();

        static MonoBehaviour monoBehaviour;
        static FixedCountDownloader Current;

        public FixedCountDownloader(MonoBehaviour behaviour)
        {
            if (monoBehaviour == null)
                monoBehaviour = behaviour;
            if (behaviour == null && monoBehaviour == null)
                throw new Exception("Behaviour was null when passed to FixedCountDownloader");
        }

        public void Update()
        {
            if (Queue.Count != 0)
            {
                int emptyIndex = Array.IndexOf(Routines, default);

                if (emptyIndex == -1)
                    return;

                (var Path, var Action, var Progreess) = Queue.Dequeue();
                Routines[emptyIndex] = RestWebClient.Instance.HttpDownloadImage(Path, (response) =>
                {
                    Debug.Log(Path);
                    Routines[emptyIndex] = default;

                    Texture2D tex = response.textureDownloaded;
                    if (response.textureDownloaded == null)
                        return;

                    Sprite s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

                    Action(s);
                },
                (progress) => Progreess(progress));

                monoBehaviour.StartCoroutine(Routines[emptyIndex]);
            }
        }

        public void AddTask(string path, Action<Sprite> Act, Action<float> Progress)
        {
            Queue.Enqueue((path, Act, Progress));
        }

        public static void StopAll(FixedCountDownloader downloader)
        {
            for (int i = 0; i < downloader.Routines.Length; ++i)
            {
                if (downloader.Routines[i] != default)
                {
                    monoBehaviour.StopCoroutine(downloader.Routines[i]);
                    downloader.Routines[i] = null;
                }
            }
        }
    }
}