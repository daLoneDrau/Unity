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

        public Hex[] LoadHexes()
        {
            int i = 0;
            Hex[] list = new Hex[0];
            Hex hex = new Hex();
            hex.Name = "The Free City of Ogon";
            hex.AddFeature(HexFeature.TOWN);
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(1, 1);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(1, 2);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.MOUNTAIN;
            hex.Location = new Vector2(1, 3);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(1, 4);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.HILL;
            hex.Location = new Vector2(1, 5);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Name = "Dead Plains";   
            hex.Type = HexType.DESERT;
            hex.Location = new Vector2(1, 6);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.HILL;
            hex.Location = new Vector2(1, 7);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(1, 8);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Name = "Angleae";
            hex.AddFeature(HexFeature.TOWN);
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(1, 9);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(1, 10);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(1, 11);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(1, 12);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(1, 13);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.SWAMP;
            hex.Name = "Llewyla Moor";
            hex.Location = new Vector2(1, 14);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.SWAMP;
            hex.Name = "Llewyla Moor";
            hex.Location = new Vector2(1, 15);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(1, 16);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(1, 17);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(1, 18);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(1, 19);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(1, 20);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(1, 21);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(1, 22);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(1, 23);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.MOUNTAIN;
            hex.Location = new Vector2(2, 1);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(2, 2);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(2, 3);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.HILL;
            hex.Location = new Vector2(2, 4);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.HILL;
            hex.Location = new Vector2(2, 5);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Name = "The Dead Plains";
            hex.AddFeature(HexFeature.OASIS);
            hex.AddFeature(HexFeature.RUINS);
            hex.Type = HexType.DESERT;
            hex.Location = new Vector2(2, 6);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.HILL;
            hex.Location = new Vector2(2, 7);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(2, 8);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(2, 9);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(2, 10);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(2, 11);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(2, 12);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.SWAMP;
            hex.Name = "Llewyla Moor";
            hex.Location = new Vector2(2, 13);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.SWAMP;
            hex.Name = "Llewyla Moor";
            hex.Location = new Vector2(2, 14);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(2, 15);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FARM;
            hex.Name = "Galden";
            hex.AddFeature(HexFeature.TOWN);
            hex.Location = new Vector2(2, 16);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FARM;
            hex.Location = new Vector2(2, 17);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(2, 18);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(2, 19);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(2, 20);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(2, 21);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FARM;
            hex.Location = new Vector2(2, 22);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(2, 23);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.MOUNTAIN;
            hex.Location = new Vector2(3, 1);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(3, 2);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(3, 3);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.MOUNTAIN;
            hex.Location = new Vector2(3, 4);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(3, 5);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.DESERT;
            hex.Name = "The Dead Plains";
            hex.Location = new Vector2(3, 6);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.HILL;
            hex.Location = new Vector2(3, 7);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(3, 8);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(3, 9);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(3, 10);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(3, 11);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.SWAMP;
            hex.Name = "Llewyla Moor";
            hex.Location = new Vector2(3, 12);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.SWAMP;
            hex.Name = "Llewyla Moor";
            hex.Location = new Vector2(3, 13);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(3, 14);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FOREST;
            hex.Location = new Vector2(3, 15);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(3, 16);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FARM;
            hex.Location = new Vector2(3, 17);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(3, 18);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FARM;
            hex.Location = new Vector2(3, 19);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(3, 20);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.COUNTRY;
            hex.Location = new Vector2(3, 21);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FARM;
            hex.Location = new Vector2(3, 22);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            hex = new Hex();
            hex.Type = HexType.FARM;
            hex.Name = "Drogat Castle";
            hex.AddFeature(HexFeature.CASTLE);
            hex.Location = new Vector2(3, 23);
            hex.Index = i++;
            list = ArrayUtilities.Instance.ExtendArray(hex, list);

            return list;
        }
        public RiverCrossing[] LoadRiverCrossings()
        {
            RiverCrossing[] list = new RiverCrossing[0];
            RiverCrossing r = new RiverCrossing();
            r.From = 0; // 1,1
            r.To = 1;  // 1,2
            r.RiverName = "Tragoth River";
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new RiverCrossing();
            r.From = 23; // 2,1
            r.To = 1; // 1,2
            r.RiverName = "Tragoth River";
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new RiverCrossing();
            r.From = 23;    // 2,1
            r.To = 24;      // 2,2 
            r.RiverName = "Tragoth River";
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new RiverCrossing();
            r.From = 47;    //3,2
            r.To = 24;      //2,2
            r.RiverName = "Tragoth River";
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new RiverCrossing();
            r.From = 48;    //3,3
            r.To = 47;      //3,2
            r.RiverName = "Tragoth River";
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new RiverCrossing();
            r.From = 13;    // 1,14
            r.To = 14;      // 1,15
            r.RiverName = "Nesser River";
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new RiverCrossing();
            r.From = 13;    // 1,14
            r.To = 35;      // 2,13
            r.RiverName = "Nesser River";
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new RiverCrossing();
            r.From = 13;    // 1,14
            r.To = 36;      // 2,14
            r.RiverName = "Nesser River";
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new RiverCrossing();
            r.From = 12;    // 1,13
            r.To = 35;      // 2,13
            r.RiverName = "Nesser River";
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new RiverCrossing();
            r.From = 34;    // 2,12
            r.To = 35;      // 2,13
            r.RiverName = "Nesser River";
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new RiverCrossing();
            r.From = 34;    // 2,12
            r.To = 58;      // 3,13
            r.RiverName = "Nesser River";
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            r = new RiverCrossing();
            r.From = 57;    // 3,12
            r.To = 58;      // 3,13
            r.RiverName = "Nesser River";
            list = ArrayUtilities.Instance.ExtendArray(r, list);

            return list;
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
