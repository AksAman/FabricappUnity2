using System.Collections;
using System.Collections.Generic;
using helloVoRld.Core.Singletons;
using helloVoRld.Networking.Models;
using helloVoRld.Utilities.Debugging;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace helloVoRld.Networking.RestClient
{
    public class RestWebClient : Singleton<RestWebClient>
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private const string defaultContentType = "application/json";

        public IEnumerator HttpGet(string url, System.Action<Response> callback, IEnumerable<RequestHeader> headers = null)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                if (headers != null)
                {
                    foreach (var x in headers)
                        webRequest.SetRequestHeader(x.Key, x.Value);
                }

                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                    });
                }

                if (webRequest.isDone)
                {
                    string data = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                        Data = data
                    });
                }
            }
        }

        public IEnumerator HttpDownloadImage(string url, System.Action<ImageResponse, int> callback, int index)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || (webRequest.error != null && webRequest.error != ""))
                {
                    callback(new ImageResponse
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                        textureDownloaded = null
                    }, index);
                }
                else if (webRequest.isDone)
                {
                    callback(new ImageResponse
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                        textureDownloaded = DownloadHandlerTexture.GetContent(webRequest),
                    }, index);
                }
            }
        }

        public IEnumerator HttpDownloadImage(string url, System.Action<ImageResponse> callback, System.Action<float> Progress)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
            {
                DownloadHandlerTexture handlerTexture = new DownloadHandlerTexture();
                webRequest.downloadHandler = handlerTexture;

                var operation = webRequest.SendWebRequest();

                while (!operation.isDone)
                {
                    Progress(webRequest.downloadProgress);
                    yield return null;
                }

                if (webRequest.isNetworkError || (webRequest.error != null && webRequest.error != ""))
                {
                    callback(new ImageResponse
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                        textureDownloaded = null
                    });
                }
                else if (webRequest.isDone)
                {
                    callback(new ImageResponse
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                        textureDownloaded = DownloadHandlerTexture.GetContent(webRequest),
                    });
                }
            }
        }

        public IEnumerator HttpDelete(string url, System.Action<Response> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Delete(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error
                    });
                }

                if (webRequest.isDone)
                {
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode
                    });
                }
            }
        }

        public IEnumerator HttpPost(string url, string body, System.Action<Response> callback, IEnumerable<RequestHeader> headers = null)
        {

            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, body))
            {
                if (headers != null)
                {
                    foreach (RequestHeader header in headers)
                    {
                        webRequest.SetRequestHeader(header.Key, header.Value);
                    }
                }

                webRequest.uploadHandler.contentType = defaultContentType;
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));

                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error
                    });
                }

                if (webRequest.isDone)
                {
                    string data = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                        Data = data
                    });
                }
            }
        }

        public IEnumerator HttpPost(string url, WWWForm form, System.Action<Response> callback, IEnumerable<RequestHeader> headers = null)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                if (headers != null)
                {
                    foreach (RequestHeader header in headers)
                    {
                        webRequest.SetRequestHeader(header.Key, header.Value);
                    }
                }

                webRequest.uploadHandler.contentType = defaultContentType;
                //webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(form.ToString()));

                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error
                    });
                }

                if (webRequest.isDone)
                {
                    string data = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                        Data = data
                    });
                }
            }
        }

        public IEnumerator HttpPut(string url, string body, System.Action<Response> callback, IEnumerable<RequestHeader> headers = null)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Put(url, body))
            {
                if (headers != null)
                {
                    foreach (RequestHeader header in headers)
                    {
                        webRequest.SetRequestHeader(header.Key, header.Value);
                    }
                }

                webRequest.uploadHandler.contentType = defaultContentType;
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));

                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                    });
                }

                if (webRequest.isDone)
                {
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                    });
                }
            }
        }

        public IEnumerator HttpHead(string url, System.Action<Response> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Head(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                    });
                }

                if (webRequest.isDone)
                {
                    var responseHeaders = webRequest.GetResponseHeaders();
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                        Headers = responseHeaders
                    });
                }
            }
        }

        public IEnumerator HttpInternetCheck(string url, Action OnSuccess, Action OnFail)
        {
            using (UnityWebRequest webRequest = new UnityWebRequest(url))
            {
                webRequest.timeout = 5;
                yield return webRequest.SendWebRequest();
                if (webRequest.responseCode < 299 && webRequest.responseCode >= 200)
                    OnSuccess();
                else
                    OnFail();
            }
        }
    }
}