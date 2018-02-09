using Assets.Scripts.BarbarianPrince.Flyweights;
using Assets.Scripts.UI;
using Assets.Scripts.UI.SimpleJSON;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.BarbarianPrince.Singletons
{
    public class BPServiceClient : MonoBehaviour
    {
        private static BPServiceClient instance;
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static BPServiceClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BPServiceClient();
                }
                return instance;
            }
            protected set { }
        }
        private BPServiceClient() { print("setting instance"); instance = this; }
        public string Endpoint { get; set; }
        /// <summary>
        /// the sprite map for setting item icons.
        /// </summary>
        [SerializeField]
        private SpriteMap SpriteMap;
        public IEnumerator GetEquipmentElementByCode(string elem, System.Action<int> result)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(Endpoint);
            sb.Append("equipment_element_types/code/");
            sb.Append(elem);
            using (UnityWebRequest www = UnityWebRequest.Get(sb.ToString()))
            {
                sb.ReturnToPool();
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
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(Endpoint);
            sb.Append("equipment_item_modifiers/code/");
            sb.Append(code);
            using (UnityWebRequest www = UnityWebRequest.Get(sb.ToString()))
            {
                sb.ReturnToPool();
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
        public IEnumerator GetItemByName(string name, System.Action<BPInteractiveObject> result)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(Endpoint);
            sb.Append("io_item_data/name/");
            sb.Append(name.Replace(" ", "%20"));
            UnityWebRequest www = new UnityWebRequest(sb.ToString())
            {
                downloadHandler = new DownloadHandlerBuffer()
            };
            sb.ReturnToPool();
            yield return www.Send();
            if (www.isError)
            {
                Debug.Log(www.error);
            }
            else
            {
                BPInteractiveObject io =
                        ((BPInteractive)Interactive.Instance).NewItem();
                BPItemData data = (BPItemData)io.ItemData;
                var str = System.Text.Encoding.Default.GetString(www.downloadHandler.data);
                var n = JSON.Parse(str);
                if (n.IsArray)
                {
                    for (int i = 0, li = n.Count; i < li; i++)
                    {
                        JSONNode node = n[i];
                        data.StackSize = node["stack_size"].AsInt;
                        data.Price = node["price"].AsFloat;
                        data.ItemName = node["name"].Value.ToString();
                        data.MaxOwned = node["max_owned"].AsInt;
                        data.Description = node["description"];
                        if (node["types"].IsArray)
                        {
                            for (int j = node["types"].Count - 1; j >= 0; j--)
                            {
                                JSONNode type = node["types"][j];
                                io.AddTypeFlag(type["flag"].AsInt);
                            }
                        }
                        if (node["modifiers"] != null)
                        {
                            JSONNode modifier = node["modifiers"];
                            foreach (JSONNode nodeMod in modifier.Keys)
                            {
                                string elem = nodeMod;
                                string mod = modifier[elem].Value.ToString();
                                int e = 0;
                                yield return StartCoroutine(GetEquipmentElementByCode(elem, value => e = value));
                                EquipmentItemModifier eim = null;
                                yield return StartCoroutine(GetElementModifierByCode(mod, value => eim = value));
                                data.Equipitem.GetElementModifier(e).Set(eim);
                            }
                        }
                        // sprite
                        if (node["sprite"] != null)
                        {
                            io.Sprite = SpriteMap.GetSprite(node["sprite"].Value.ToString());
                        }
                        else
                        {
                            io.Sprite = SpriteMap.GetSprite(data.ItemName);
                        }
                        // script
                        string script = node["internal_script"].Value.ToString();
                        sb = StringBuilderPool.Instance.GetStringBuilder();
                        sb.Append("Assets.Scripts.BarbarianPrince.Scriptables.Items.");
                        sb.Append(script);
                        Type blob = Type.GetType(sb.ToString());
                        sb.ReturnToPool();
                        object o = Activator.CreateInstance(blob);
                        io.Script = (Scriptable)o;
                        Script.Instance.SendInitScriptEvent(io);
                    }
                }
                result(io);
            }
        }
    }
}
