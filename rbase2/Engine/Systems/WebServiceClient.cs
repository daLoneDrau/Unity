using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace RPGBase.Engine.Systems
{
    public class WebServiceClient
    {
        IEnumerator WaitForRequest(System.Action onComplete)
        {
            UnityWebRequest www = UnityWebRequest.Get("http://www.my-server.com");
            yield return www.Send();

            if (www.isError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);

                // Or retrieve results as binary data
                string results = www.downloadHandler.text;
            }
        }
    }
}