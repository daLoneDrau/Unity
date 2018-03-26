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
using Assets.Scripts.BarbarianPrince.Graph;

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
                    GameObject go = new GameObject
                    {
                        name = "RestServiceClient"
                    };
                    instance = go.AddComponent<BPServiceClient>();
                }
                return instance;
            }
        }
        public string Endpoint { get; set; }
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
            print(sb.ToString());
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
                            io.Sprite = SpriteMap.Instance.GetSprite(node["sprite"].Value.ToString());
                        }
                        else
                        {
                            io.Sprite = SpriteMap.Instance.GetSprite(data.ItemName);
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
        public IEnumerator GetAllHexes(System.Action<Hex[]> result)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(Endpoint);
            sb.Append("hexs");
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
                Hex[] list = new Hex[0];
                var str = System.Text.Encoding.Default.GetString(www.downloadHandler.data);
                var n = JSON.Parse(str);
                if (n.IsArray)
                {
                    for (int i = 0, li = n.Count; i < li; i++)
                    {
                        JSONNode node = n[i];
                        Hex hex = new Hex
                        {
                            Location = new Vector2(node["x"].AsInt, node["y"].AsInt),
                            Type = HexType.ValueOf(node["hex_type"]["name"].Value.ToString())
                        };
                        hex.Index = ((int)hex.Location.x - 1) * 23 + (int)hex.Location.y - 1;
                        if (node["name"] != null)
                        {
                            hex.Name = node["name"].Value.ToString();
                        }
                        if (node["features"].IsArray)
                        {
                            for (int j = node["features"].Count - 1; j >= 0; j--)
                            {
                                JSONNode feature = node["features"][j];
                                hex.AddFeature(HexFeature.ValueOf(feature["name"].Value.ToString()));
                            }
                        }
                        list = ArrayUtilities.Instance.ExtendArray(hex, list);
                    }
                }
                result(list);
            }
        }
        public IEnumerator GetAllRiverCrossings(System.Action<RiverCrossing[]> result)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(Endpoint);
            sb.Append("river_crossings");
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
                RiverCrossing[] list = new RiverCrossing[0];
                var str = System.Text.Encoding.Default.GetString(www.downloadHandler.data);
                var n = JSON.Parse(str);
                if (n.IsArray)
                {
                    for (int i = 0, li = n.Count; i < li; i++)
                    {
                        JSONNode node = n[i];
                        RiverCrossing rc = new RiverCrossing();
                        int fx = node["from_x"].AsInt, fy = node["from_y"].AsInt, tx = node["to_x"].AsInt, ty = node["to_y"].AsInt;
                        rc.From = (fx - 1) * 23 + fy - 1;
                        rc.To = (tx - 1) * 23 + ty - 1;
                        rc.RiverName = node["name"].Value.ToString();
                        list = ArrayUtilities.Instance.ExtendArray(rc, list);
                    }
                }
                result(list);
            }
        }
        public IEnumerator GetAllRoads(System.Action<int[][]> result)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(Endpoint);
            sb.Append("roads");
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
                int[][] list = new int[0][];
                var str = System.Text.Encoding.Default.GetString(www.downloadHandler.data);
                var n = JSON.Parse(str);
                if (n.IsArray)
                {
                    for (int i = 0, li = n.Count; i < li; i++)
                    {
                        JSONNode node = n[i];
                        int fx = node["from_x"].AsInt, fy = node["from_y"].AsInt, tx = node["to_x"].AsInt, ty = node["to_y"].AsInt;
                        int[] r = new int[2] { (fx - 1) * 23 + fy - 1, (tx - 1) * 23 + ty - 1 };
                        list = ArrayUtilities.Instance.ExtendArray(r, list);
                    }
                }
                result(list);
            }
        }
        public int[][] LoadRoads()
        {
            int[][] list = new int[0][];
            // DEAD PLAINS ROAD
            int[] r = new int[2] { 8, 30 }; // 1,9 - 2,8
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new int[2] { 30, 29 };      // 2,8 - 2,7
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new int[2] { 29, 28 };      // 2,7 - 2,6
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            // GALDEN ROAD
            r = new int[2] { 38, 62 };      // 2,16 - 3,17
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new int[2] { 62, 63 };      // 3,17 - 3,18
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new int[2] { 63, 64 };      // 3,18 - 3,19
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new int[2] { 67, 68 };      // 3,22 - 3,23
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            return list;
        }
    }
}
