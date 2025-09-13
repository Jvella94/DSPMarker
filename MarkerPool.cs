using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DSPMarker
{
    public class MarkerPool : MonoBehaviour
    {
        public static Dictionary<int, List<int>> markerIdInPlanet = new Dictionary<int, List<int>>();
        public static Dictionary<int, Marker> markerPool = new Dictionary<int, Marker>();

        public static int markerCursor = 0;
        public static bool markerEnable = true;

        public static GameObject[] markers;
        public static Image[] Base;
        public static Image[] BaseRound;
        public static Image[] icon1s;
        public static Image[] icon2s;
        public static Text[] decs;

        public static bool markerCreated = false;

        public struct Marker
        {
            public int planetID;
            public Vector3 pos;
            public int icon1ID;
            public int icon2ID;
            public Color color;
            public string desc;
            public bool enabled;
            public bool throughPlanet;
            public bool ShowArrow;
        }

        public static void Create()
        {
            markers = new GameObject[Main.maxMarkerAmount];
            Base = new Image[Main.maxMarkerAmount];
            BaseRound = new Image[Main.maxMarkerAmount];
            icon1s = new Image[Main.maxMarkerAmount];
            icon2s = new Image[Main.maxMarkerAmount];
            decs = new Text[Main.maxMarkerAmount];

            for (int i = 0; i < Main.maxMarkerAmount; i++)
            {
                markers[i] = Instantiate(MarkerPrefab.pinBasePrefab.gameObject, MarkerPrefab.markerGroup.transform);
                Base[i] = markers[i].GetComponent<Image>();
                BaseRound[i] = markers[i].transform.Find("round").GetComponent<Image>();
                icon1s[i] = markers[i].transform.Find("round/pinBaseIcon1").GetComponent<Image>();
                icon2s[i] = markers[i].transform.Find("round/pinBaseIcon2").GetComponent<Image>();
                decs[i] = markers[i].transform.Find("round/pinBaseText").GetComponent<Text>();
            }
            markerCreated = true;
        }

        public static void Refresh()
        {
            if (GameMain.localPlanet is null) return;
            int planetId = GameMain.localPlanet.id;

            if (!markerIdInPlanet.ContainsKey(planetId))
            {
                List<int> list = new List<int>();
                markerIdInPlanet.Add(planetId, list);
            }

            if (markerIdInPlanet[planetId].Count > 0 && markerCreated)
            {
                for (int i = 0; i < markerIdInPlanet[planetId].Count; i++)
                {
                    var num = markerIdInPlanet[planetId][i];
                    if (markerPool.ContainsKey(num))
                    {
                        Base[i].color = markerPool[num].color;
                        var halfColor = new Color(markerPool[num].color.r * 0.3f, markerPool[num].color.g * 0.3f, markerPool[num].color.b * 0.3f, 1f);
                        BaseRound[i].color = halfColor;
                        var desc = markerPool[num].desc.Length > 0 ? markerPool[num].desc + "\n" : string.Empty;
                        if (markerPool[num].icon1ID == 0)
                        {
                            icon1s[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            icon1s[i].gameObject.SetActive(true);
                            icon1s[i].sprite = LDB.signals.IconSprite(markerPool[num].icon1ID);
                            desc += $" ({LDB.items.Select(markerPool[num].icon1ID).name})\n";
                        }
                        if (markerPool[num].icon2ID == 0)
                        {
                            icon2s[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            icon2s[i].gameObject.SetActive(true);
                            icon2s[i].sprite = LDB.signals.IconSprite(markerPool[num].icon2ID);
                            desc += $" ({LDB.items.Select(markerPool[num].icon2ID).name})";
                        }
                        decs[i].text = desc;
                        if (markerPool[num].icon1ID == 0 && markerPool[num].icon2ID == 0)
                        {
                            decs[i].GetComponent<RectTransform>().sizeDelta = new Vector3(130, 130, 0);
                            decs[i].transform.localPosition = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            decs[i].GetComponent<RectTransform>().sizeDelta = new Vector3(130, 70, 0);
                            decs[i].transform.localPosition = new Vector3(0, -28, 0);
                        }
                        if (markerPool[num].icon2ID == 0 && desc == "")
                        {
                            icon1s[i].GetComponent<RectTransform>().sizeDelta = new Vector3(130, 130, 0);
                            icon1s[i].transform.localPosition = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            icon1s[i].GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);
                            icon1s[i].transform.localPosition = new Vector3(-30, 30, 0);
                        }
                    }
                }
            }
        }

        public static void Clear()
        {
            for (int i = 0; i < Main.maxMarkerAmount; i++)
            {
                markers[i].gameObject.SetActive(false);
            }
        }

        public static void Update()
        {
            //LogManager.Logger.LogInfo("------------------------------------------------------------------markerPool Update ");
            if (!MarkerList.showList)
            {
                MarkerPrefab.markerGroup.SetActive(false);
                return;
            }
            int planetId = GameMain.data.localPlanet.id;
            MarkerPrefab.markerGroup.SetActive(true);

            for (int i = 0; i < Main.maxMarkerAmount; i++)
            {
                //LogManager.Logger.LogInfo("------------------------------------------------------------------i : " + i);
                if (markerIdInPlanet.ContainsKey(planetId) && i < markerIdInPlanet[planetId].Count)
                {
                    var localMarkerId = markerIdInPlanet[planetId][i];
                    TransformPosAndDraw(i, localMarkerId);
                }
                else
                {
                    markers[i].gameObject.SetActive(false);
                }
            }
        }

        public static void TransformPosAndDraw(int markerIndex, int localMarkerId)
        {
            if (markerPool[localMarkerId].enabled == false)
            {
                markers[markerIndex].SetActive(false);
                return;
            }
            Vector3 localPosition = GameCamera.main.transform.localPosition;
            Vector3 forward = GameCamera.main.transform.forward;

            float realRadius = GameMain.localPlanet.realRadius;    //高さ

            Vector3 vector;
            vector = markerPool[localMarkerId].pos.normalized * (realRadius + 15f);
            Vector3 vector2 = vector - localPosition;
            float magnitude = vector2.magnitude;
            float num = Vector3.Dot(forward, vector2);

            if (magnitude < 1f || num < 1f)
            {
                markers[markerIndex].SetActive(false);
            }
            else
            {
                bool flag = UIRoot.ScreenPointIntoRect(GameCamera.main.WorldToScreenPoint(vector), MarkerPrefab.markerGroup.GetComponent<RectTransform>(), out Vector2 vector3);
                if (Mathf.Abs(vector3.x) > 8000f)
                {
                    flag = false;
                }
                if (Mathf.Abs(vector3.y) > 8000f)
                {
                    flag = false;
                }
                if (Phys.RayCastSphere(localPosition, vector2 / magnitude, magnitude, Vector3.zero, realRadius, out _))
                {
                    flag = false;
                }
                if (flag || markerPool[localMarkerId].throughPlanet)
                {
                    UIRoot.ScreenPointIntoRect(GameCamera.main.WorldToScreenPoint(vector), MarkerPrefab.markerGroup.GetComponent<RectTransform>(), out vector3);

                    markers[markerIndex].GetComponent<RectTransform>().anchoredPosition = vector3;

                    if (magnitude < 50)
                    {
                        MarkerPrefab.pinBasePrefab.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    }
                    else if (magnitude < 250)
                    {
                        float num2 = (float)(0.8 - magnitude * 0.002);
                        markers[markerIndex].transform.localScale = new Vector3(1, 1, 1) * num2;
                    }
                    else
                    {
                        markers[markerIndex].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    }
                    markers[markerIndex].SetActive(true);
                }
                else
                {
                    markers[markerIndex].SetActive(false);
                }
            }
        }
    }
}