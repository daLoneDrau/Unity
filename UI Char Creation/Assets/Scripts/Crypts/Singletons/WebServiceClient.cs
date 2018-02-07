using Assets.Scripts.Crypts.Flyweights;
using Assets.Scripts.UI.SimpleJSON;
using RPGBase.Flyweights;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Crypts.Singletons
{
    public class WebServiceClient:MonoBehaviour
    {
        private static WebServiceClient instance;
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static WebServiceClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WebServiceClient();
                }
                return instance;
            }
            protected set { }
        }
        private WebServiceClient() { }
        public IEnumerator GetEquipmentElementByCode(string elem, System.Action<int> result)
        {
            using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/SW-CTService/sw_ct/equipment_element_types/code/" + elem))
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public IEnumerator GetElementModifierByCode(string code, System.Action<EquipmentItemModifier> result)
        {
            using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/SW-CTService/sw_ct/equipment_item_modifiers/code/" + code))
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
                        EquipmentItemModifier mod = null;
                        for (int i = 0, li = n.Count; i < li; i++)
                        {
                            mod = new EquipmentItemModifier
                            {
                                Value = n[i]["value"].AsFloat,
                                Percent = n[i]["percent"].AsBool
                            };
                            break;
                        }
                        result(mod);
                    }
                }
            }
        }
        public IEnumerator LoadHomelands(Action callback)
        {
            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++LoadHomelands");
            UnityWebRequest www = new UnityWebRequest("http://localhost:8080/SW-CTService/sw_ct/homelands")
            {
                downloadHandler = new DownloadHandlerBuffer()
            };
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
                    for (int i = 0, li = n.Count; i < li; i++)
                    {
                        Homeland homeland = new Homeland(n[i]["name"], n[i]["description"]);
                        if (n[i]["modifiers"] != null)
                        {
                            JSONObject o = n[i]["modifiers"].AsObject;
                            Console.WriteLine(o.ToString());
                            foreach (JSONNode node in o.Keys)
                            {
                                string elem = node;
                                string mod = o[elem].ToString();
                                int e = 0;
                                Console.WriteLine(elem);
                                Console.WriteLine(mod);
                                yield return StartCoroutine(GetEquipmentElementByCode(elem, value => e = value));
                                EquipmentItemModifier eim = null;
                                yield return StartCoroutine(GetElementModifierByCode(mod, value => eim = value));
                                homeland.SetModifier(e, eim);
                            }
                        }
                        else
                        {
                            Console.WriteLine("nomodifiers");
                        }
                    }
                }
                print("finished processing");
                callback();
            }
        }
    }
}
