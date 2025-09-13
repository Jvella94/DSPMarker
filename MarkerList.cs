using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace DSPMarker
{
    public class MarkerList : ManualBehaviour
    {
        public static GameObject markerList = new GameObject();
        public static GameObject markerButton = new GameObject();
        public static GameObject listBase = new GameObject();
        public static GameObject boxBasePrefab = new GameObject();
        public static GameObject boxBaseSquare = new GameObject();
        public static GameObject boxBaseText = new GameObject();
        public static GameObject boxBaseIcon1 = new GameObject();
        public static GameObject boxBaseIcon2 = new GameObject();
        public static GameObject modeText = new GameObject();

        public static GameObject[] boxMarker;
        public static GameObject[] boxSquare;
        public static GameObject[] boxText;
        public static GameObject[] boxIcon1;
        public static GameObject[] boxIcon2;

        public static Sprite markerIcon;
        public static int boxSize = 40;

        public static bool showList = true;
        public static bool editMode;
        public static bool Guiding = false;

        //UI解像度計算
        public static int UIheight = DSPGame.globalOption.uiLayoutHeight;
        public static int UIwidth = UIheight * Screen.width / Screen.height;

        public static void Create()
        {
            boxMarker = new GameObject[Main.maxMarkerAmount];
            boxSquare = new GameObject[Main.maxMarkerAmount];
            boxText = new GameObject[Main.maxMarkerAmount];
            boxIcon1 = new GameObject[Main.maxMarkerAmount];
            boxIcon2 = new GameObject[Main.maxMarkerAmount];

            //ボタン＆リスト用オブジェクトの作成
            markerList = new GameObject
            {
                name = "MarkerList"
            };
            markerList.transform.SetParent(GameObject.Find("UI Root/Overlay Canvas/In Game/Windows").transform);
            markerList.transform.localPosition = new Vector3(Screen.width / 2 - 60, Screen.height / 2 - 45, 0);
            markerList.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

            //リストの表示切替用ボタンの作成
            markerButton = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Game Menu/detail-func-group/dfunc-1"), markerList.transform) as GameObject;
            markerButton.name = "markerButton";
            markerButton.transform.localPosition = new Vector3(0, 0, 0);
            markerButton.GetComponent<UIButton>().tips.tipTitle = "Marker List".Translate();
            markerButton.GetComponent<UIButton>().tips.tipText = "Click to show/hide Marker List.\nRight Click to enter/exit Edit mode.".Translate();
            markerButton.GetComponent<UIButton>().tips.corner = 4;
            markerButton.GetComponent<UIButton>().tips.offset = new Vector2(-50, 20);
            markerButton.GetComponent<UIButton>().tips.width = 215;
            markerButton.transform.Find("icon").GetComponent<Image>().sprite = Main.ButtonSprite;
            markerButton.GetComponent<UIButton>().highlighted = true;
            markerButton.AddComponent<UIClickHandler>();
            //ボタンイベントの作成
            markerButton.GetComponent<UIButton>().button.onClick.AddListener(new UnityAction(OnClickMarkerButton));

            //ボタン＆リスト用オブジェクトの作成
            listBase = new GameObject
            {
                name = "listBase"
            };
            listBase.transform.SetParent(markerList.transform);
            listBase.transform.localPosition = new Vector3(0, 0, -1);
            listBase.transform.localScale = new Vector3(1, 1, 1);

            //リストprefabの作成

            boxBasePrefab.transform.SetParent(listBase.transform);
            boxBasePrefab.name = "boxBasePrefab";
            boxBasePrefab.AddComponent<Image>().color = new Color(0.7f, 0.5f, 0, 1);
            boxBasePrefab.AddComponent<Button>();
            boxBasePrefab.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            boxBasePrefab.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            boxBasePrefab.GetComponent<RectTransform>().sizeDelta = new Vector3(70, 70, 0);
            boxBasePrefab.transform.localPosition = new Vector3(5, -80, 0);
            boxBasePrefab.transform.localScale = new Vector3(1, 1, 1);
            boxBasePrefab.SetActive(false);

            boxBaseSquare.transform.SetParent(boxBasePrefab.transform);
            boxBaseSquare.name = "boxBaseSquare";
            boxBaseSquare.AddComponent<Image>().color = new Color(0.3f, 0.3f, 0, 1);
            boxBaseSquare.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            boxBaseSquare.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            boxBaseSquare.GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);
            boxBaseSquare.transform.localPosition = new Vector3(0, 0, 0);
            boxBaseSquare.transform.localScale = new Vector3(1, 1, 1);
            boxBaseSquare.SetActive(false);

            boxBaseText = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Scene UIs/Vein Marks/vein-tip-prefab/info-text"), boxBasePrefab.transform);
            boxBaseText.name = "boxBaseText";
            boxBaseText.AddComponent<Outline>().effectDistance = new Vector2(1, -1);
            boxBaseText.GetComponent<Text>().text = "New\nMarker".Translate();
            boxBaseText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            boxBaseText.GetComponent<Text>().lineSpacing = 0.7f;
            boxBaseText.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Wrap;
            boxBaseText.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Truncate;
            boxBaseText.GetComponent<Text>().resizeTextForBestFit = true;
            boxBaseText.GetComponent<Text>().fontSize = 5;
            boxBaseText.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            boxBaseText.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            boxBaseText.GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);
            boxBaseText.transform.localPosition = new Vector3(0, 0, 0);
            boxBaseText.transform.localScale = new Vector3(1, 1, 1);
            Destroy(boxBaseText.GetComponent<Shadow>());
            boxBaseText.SetActive(true);

            modeText = Instantiate(boxBaseText.gameObject, markerList.transform);
            modeText.name = "modeText";
            modeText.transform.localPosition = new Vector3(-70, 10, 0);
            modeText.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
            modeText.GetComponent<Text>().text = "Guide Mode".Translate();
            modeText.GetComponent<RectTransform>().sizeDelta = new Vector3(100, 25, 0);
            modeText.SetActive(true);

            boxBaseIcon1.AddComponent<Image>().sprite = LDB.techs.Select(1001).iconSprite;
            boxBaseIcon1.name = "boxBaseIcon1";
            boxBaseIcon1.transform.SetParent(boxBasePrefab.transform);
            boxBaseIcon1.AddComponent<Outline>().effectDistance = new Vector2(1, -1);
            boxBaseIcon1.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            boxBaseIcon1.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            boxBaseIcon1.GetComponent<RectTransform>().sizeDelta = new Vector3(30, 30, 0);
            boxBaseIcon1.transform.localPosition = new Vector3(-15, 15, 0);
            boxBaseIcon1.transform.localScale = new Vector3(1, 1, 1);
            boxBaseIcon1.SetActive(false);

            boxBaseIcon2 = Instantiate(boxBaseIcon1.gameObject);
            boxBaseIcon2.name = "boxBaseIcon2";
            boxBaseIcon2.transform.SetParent(boxBasePrefab.transform);
            boxBaseIcon2.transform.localPosition = new Vector3(15, 15, 0);
            boxBaseIcon2.transform.localScale = new Vector3(1, 1, 1);

            //リストの作成
            int maxRow = Screen.height / 78;
            float scale = (float)boxSize / 70;
            for (int i = 0; i < Main.maxMarkerAmount; i++)
            {
                boxMarker[i] = Instantiate(boxBasePrefab.gameObject, listBase.transform);
                boxMarker[i].name = "boxMarker" + i;
                boxMarker[i].transform.localScale = new Vector3(scale, scale, 1);
                int x = 20 - (boxSize + 3) * (i / maxRow);
                int y = -68 - (boxSize + 3) * (i % maxRow);
                boxMarker[i].transform.localPosition = new Vector3(x, y, 0);
                boxSquare[i] = boxMarker[i].transform.Find("boxBaseSquare").gameObject;
                boxText[i] = boxMarker[i].transform.Find("boxBaseText").gameObject;
                boxText[i].transform.localPosition = new Vector3(0, 0, 0);
                boxIcon1[i] = boxMarker[i].transform.Find("boxBaseIcon1").gameObject;
                boxIcon1[i].transform.localPosition = new Vector3(-15, 15, 0);
                boxIcon2[i] = boxMarker[i].transform.Find("boxBaseIcon2").gameObject;
                boxIcon2[i].transform.localPosition = new Vector3(15, 15, 0);
                var count = i;
                boxMarker[i].AddComponent<UIClickHandler>();
                boxMarker[i].GetComponent<Button>().onClick.AddListener(() => OnClickBoxMarker(count));
            }
            boxBasePrefab.SetActive(false);
        }

        //イベント
        //リスト表示の切り替え
        public static void OnClickMarkerButton()
        {
            showList = !showList;
            modeText.SetActive(showList);
            listBase.gameObject.SetActive(showList);
            markerButton.GetComponent<UIButton>().highlighted = showList;
        }

        public static void Clear()
        {
            for (int i = 0; i < Main.maxMarkerAmount; i++)
            {
                boxMarker[i].gameObject.SetActive(false);
            }
        }

        //右クリックで新規
        //リストをクリックしたら
        public static void OnClickBoxMarker(int i)
        {
            //LogManager.Logger.LogInfo("---------------------------------------------------------i : " + i);
            if (editMode)
            {
                //LogManager.Logger.LogInfo("---------------------------------------------------------i : " + i);
                if (MarkerEditor.window.activeSelf) MarkerEditor.Close();
                int planetId = GameMain.localPlanet.id;
                bool visibleEditbox = editMode && MarkerPool.markerIdInPlanet[planetId].Count < Main.maxMarkerAmount;
                //LogManager.Logger.LogInfo("Box Marker " + i + " clicked. visibleEditbox: " + visibleEditbox);
                if (i == 0 && visibleEditbox) MarkerEditor.Open(MarkerPool.markerIdInPlanet[planetId].Count);
                else MarkerEditor.Open(visibleEditbox ? i - 1 : i);
            }
            else if (!GameMain.data.mainPlayer.sailing)
            {
                if (Guiding)
                {
                    GameMain.mainPlayer.ClearOrders();
                    Guiding = false;
                }

                int planetId = GameMain.data.localPlanet.id;
                var num = MarkerPool.markerIdInPlanet[planetId][i];
                //オーダー設定
                OrderNode order = new OrderNode
                {
                    type = EOrderType.Move,
                    target = MarkerPool.markerPool[num].pos,
                    objType = EObjectType.Entity,
                    objId = 0,
                    objPos = MarkerPool.markerPool[num].pos
                };
                GameMain.mainPlayer.orders.currentOrder = order;

                CircleGizmo circleGizmo = CircleGizmo.Create(0, order.target, 0.27f);
                circleGizmo.relateObject = order;
                circleGizmo.color = MarkerPool.markerPool[num].color;
                circleGizmo.fadeInScale = 1.8f;
                circleGizmo.fadeInTime = 0.15f;
                circleGizmo.fadeOutScale = 1.8f;
                circleGizmo.fadeOutTime = 0.15f;
                circleGizmo.alphaMultiplier = 0.5f;
                circleGizmo.multiplier = 3f;
                circleGizmo.Open();
                GameMain.mainPlayer.gizmo.orderGizmos.Add(circleGizmo);

                LineGizmo lineGizmo = LineGizmo.Create(0, order.target, GameMain.mainPlayer.position);
                lineGizmo.color = MarkerPool.markerPool[num].color;
                lineGizmo.relateObject = order;
                lineGizmo.autoRefresh = true;
                lineGizmo.width = 3f;
                lineGizmo.alphaMultiplier = 0.5f;
                lineGizmo.multiplier = 3f;
                lineGizmo.spherical = true;
                lineGizmo.Open();
                GameMain.mainPlayer.gizmo.orderGizmos.Add(lineGizmo);
                Guiding = true;
            }
        }

        //全ての右クリック
        public static void OnRightClick(GameObject obj)
        {
            //LogManager.Logger.LogInfo("--------------------------------------------------------obj.name : " + obj.name);
            if (obj.name == "markerButton")
            {
                if (!GameMain.data.mainPlayer.sailing)
                {
                    if (editMode)
                    {
                        modeText.GetComponent<Text>().text = "Guide Mode".Translate();
                        MarkerEditor.Close();
                        editMode = false;
                    }
                    else
                    {
                        if (showList == false)
                        {
                            showList = !showList;
                            listBase.gameObject.SetActive(showList);
                            markerButton.GetComponent<UIButton>().highlighted = showList;
                        }
                        modeText.GetComponent<Text>().text = "Edit Mode".Translate();
                        editMode = true;
                    }
                    Refresh();
                    //LogManager.Logger.LogInfo("---------------------------------------------------------markerButton RIGHT cLICK ");
                }
            }
        }
        public static void Refresh()
        {
            //LogManager.Logger.LogInfo("---------------------------------------------------------refresh");
            //LogManager.Logger.LogInfo("---------------------------------------------------------MarkerPool.markerPool.Count : " + MarkerPool.markerPool.Count);
            if (GameMain.localPlanet is null) return;
            int planetId = GameMain.localPlanet.id;
            

            if (!MarkerPool.markerIdInPlanet.ContainsKey(planetId))
            {
                List<int> list = new List<int>();
                MarkerPool.markerIdInPlanet.Add(planetId, list);
            }

            bool showEditBox = editMode && MarkerPool.markerIdInPlanet[planetId].Count < Main.maxMarkerAmount;

            if (showEditBox)
            {
                // Show the edit box in slot 0
                boxMarker[0].GetComponent<Image>().color = new Color(0.7f, 0.5f, 0, 1);
                boxSquare[0].SetActive(false);
                boxIcon1[0].SetActive(false);
                boxIcon2[0].SetActive(false);
                boxText[0].transform.localPosition = new Vector3(-30, 0, 0);
                boxText[0].GetComponent<Text>().text = "New\nMarker".Translate();
                boxText[0].GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);
                boxMarker[0].SetActive(true);
            }
            else
            {
                // Hide the edit box if count reached/exceeded max
                boxMarker[0].SetActive(false);
            }

            // Fill in the rest of the boxes, with an index shift if edit box is present
            for (int i = 0; i < Main.maxMarkerAmount; i++)
            {
                int boxIndex = showEditBox ? i + 1 : i;
                if (i < MarkerPool.markerIdInPlanet[planetId].Count && boxIndex < Main.maxMarkerAmount)
                {
                    var num = MarkerPool.markerIdInPlanet[planetId][i];
                    var marker = MarkerPool.markerPool[num];
                    boxMarker[boxIndex].GetComponent<Image>().color = marker.color;
                    boxMarker[boxIndex].SetActive(true);
                    boxSquare[boxIndex].GetComponent<Image>().color = new Color(marker.color.r * 0.3f, marker.color.g * 0.3f, marker.color.b * 0.3f, 1);
                    boxSquare[boxIndex].SetActive(true);
                    string desc = marker.desc.Length > 0 ? marker.desc : i.ToString();
                    boxText[boxIndex].GetComponent<Text>().text = desc;
                    if (marker.icon1ID == 0 && marker.icon2ID == 0)
                    {
                        boxText[boxIndex].GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);
                        boxText[boxIndex].transform.localPosition = new Vector3(-30, 0, 0);
                    }
                    else
                    {
                        boxText[boxIndex].GetComponent<RectTransform>().sizeDelta = new Vector3(60, 35, 0);
                        boxText[boxIndex].transform.localPosition = new Vector3(-30, -15, 0);
                    }
                    boxText[boxIndex].SetActive(true);
                    if (marker.icon1ID == 0)
                        boxIcon1[boxIndex].SetActive(false);
                    else
                    {
                        boxIcon1[boxIndex].GetComponent<Image>().sprite = LDB.signals.IconSprite(marker.icon1ID);
                        boxIcon1[boxIndex].SetActive(true);
                    }
                    if (marker.icon2ID == 0)
                        boxIcon2[boxIndex].SetActive(false);
                    else
                    {
                        boxIcon2[boxIndex].GetComponent<Image>().sprite = LDB.signals.IconSprite(marker.icon2ID);
                        boxIcon2[boxIndex].SetActive(true);
                    }
                    if (marker.icon2ID == 0 && desc == string.Empty)
                    {
                        boxIcon1[boxIndex].GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);
                        boxIcon1[boxIndex].transform.localPosition = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        boxIcon1[boxIndex].GetComponent<RectTransform>().sizeDelta = new Vector3(30, 30, 0);
                        boxIcon1[boxIndex].transform.localPosition = new Vector3(-15, 15, 0);
                    }
                }
                else if (boxIndex < Main.maxMarkerAmount)
                {
                    boxMarker[boxIndex].SetActive(false);
                }
            }
        }
    }
}