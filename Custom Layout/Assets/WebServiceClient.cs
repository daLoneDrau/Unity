using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebServiceClient {

    public static IEnumerator GetText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/FFService/ff/attributes"))
        {
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
                byte[] results = www.downloadHandler.data;
                var str = System.Text.Encoding.Default.GetString(results);
                Debug.Log(str);
            }
        }
    }
}
