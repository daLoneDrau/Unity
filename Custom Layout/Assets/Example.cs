using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Example : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(GetText());
    }
    IEnumerator GetElement(string elem, System.Action<int> result)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/FFService/ff/equipment_element_types/code/" + elem))
        {
            yield return www.Send();

            if (www.isError)
            {
                Debug.Log(www.error);
            }
            else
            {
                var str = System.Text.Encoding.Default.GetString(www.downloadHandler.data);
                var n = JSON.Parse(str);
                if (n.IsArray)
                {
                    int val = 0;
                    for (int i = 0, li = n.Count; i < li; i++)
                    {
                        val = n[i]["value"];
                    }
                    result(val);
                }
            }
        }
    }
    IEnumerator GetText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/FFService/ff/io_item_data"))
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
                var n = JSON.Parse(str);
                if (n.IsArray)
                {
                    print("is array " + n.Count);
                    for (int i = 0, li = n.Count; i < li; i++)
                    {
                        var ni = n[i];
                        print(ni.ToString());
                        if (ni["modifiers"] != null)
                        {
                            JSONObject o = ni["modifiers"].AsObject;
                            print(o);
                            foreach (JSONNode node in o.Keys)
                            {
                                string elem = node;
                                string mod = o[elem].ToString();
                                int e = 0;
                                yield return StartCoroutine(GetElement(elem, value => e = value));
                                print("e::" + e);
                            }
                        }
                        else
                        {
                            print("nomodifiers");
                        }
                    }
                }
            }
        }
    }
}
