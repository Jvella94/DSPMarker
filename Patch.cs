using HarmonyLib;
using UnityEngine;

namespace DSPMarker
{
    [HarmonyPatch]
    internal class Patch
    {
        //オプション変更後の情報ウインドウ位置修正
        [HarmonyPostfix, HarmonyPatch(typeof(UIOptionWindow), "ApplyOptions")]
        public static void UIOptionWindowa_ApplyOptions_Postfix(UIOptionWindow __instance)
        {
            //UI解像度計算
            int UIheight = DSPGame.globalOption.uiLayoutHeight;
            int UIwidth = UIheight * Screen.width / Screen.height;

            //位置の調整
            MarkerList.markerList.transform.localPosition = new Vector3(UIwidth / 2 - 60, UIheight / 2 - 70, 0);
            int maxRow = (UIheight - 270 - 115) / MarkerList.boxSize;
            for (int i = 0; i < Main.maxMarkerAmount; i++)
            {
                float scale = (float)MarkerList.boxSize / 70;
                MarkerList.boxMarker[i].transform.localScale = new Vector3(scale, scale, 1);
                int x = 20 - (MarkerList.boxSize + 3) * (i / maxRow);
                int y = -68 - (MarkerList.boxSize + 3) * (i % maxRow);
                MarkerList.boxMarker[i].transform.localPosition = new Vector3(x, y, 0);

            }
        }

        //他の惑星に到着したら再表示
        [HarmonyPostfix, HarmonyPatch(typeof(GameData), "ArrivePlanet")]
        public static void UIStarDetail_ArrivePlanet_Postfix()
        {
            MarkerPrefab.markerGroup.SetActive(true);
            MarkerList.listBase.SetActive(true);

            MarkerPool.Update();
            ArrowPool.Update();
            MarkerList.Refresh();
            MarkerPool.Refresh();

        }

        //惑星を去ったら
        [HarmonyPostfix, HarmonyPatch(typeof(GameData), "LeavePlanet")]
        public static void UIStarDetail_LeavePlanet_Postfix()
        {
            MarkerPrefab.markerGroup.SetActive(false);
            MarkerList.listBase.SetActive(false);
        }
    }
}
