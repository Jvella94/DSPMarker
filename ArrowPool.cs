using UnityEngine;
using System.Collections.Generic;

namespace DSPMarker
{
    internal class ArrowPool : MonoBehaviour
    {
        public static GameObject guideArrowBase;
        public static GameObject[] guideArrow;

        public static void Update()
        {
            int planetId = GameMain.localPlanet.id;
            if (!MarkerPool.markerIdInPlanet.ContainsKey(planetId))
            {
                List<int> list = new List<int>();
                MarkerPool.markerIdInPlanet.Add(planetId, list);
            }

            if (!MarkerList.showList)
            {
                guideArrowBase.gameObject.SetActive(false);
                return;
            }

            if (guideArrowBase)
            {
                if (!MarkerList.showList || GameMain.localPlanet != null && !GameMain.data.mainPlayer.sailing)
                {
                    GameObject Player = GameMain.data.mainPlayer.gameObject;
                    if (Player.activeSelf)
                    {
                        Plane plane = new Plane(Player.transform.up, Player.transform.position);
                        guideArrowBase.gameObject.SetActive(true);

                        for (int i = 0; i < Main.maxMarkerAmount; i++)
                        {
                            if (i < MarkerPool.markerIdInPlanet[planetId].Count)
                            {
                                var num = MarkerPool.markerIdInPlanet[planetId][i];
                                if (MarkerPool.markerPool[num].ShowArrow)
                                {
                                    var point = MarkerPool.markerPool[num].pos;
                                    var planePoint = plane.ClosestPointOnPlane(point);

                                    guideArrow[i].transform.LookAt(planePoint, Player.transform.up);
                                    guideArrow[i].GetComponent<MeshRenderer>().material.color = MarkerPool.markerPool[num].color;
                                    guideArrow[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", MarkerPool.markerPool[num].color);
                                    guideArrow[i].gameObject.SetActive(true);
                                }
                                else guideArrow[i].gameObject.SetActive(false);
                            }
                            else guideArrow[i].gameObject.SetActive(false);
                        }
                    }
                }
                else guideArrowBase.gameObject.SetActive(false);
            }
            else
            {
                guideArrow = new GameObject[Main.maxMarkerAmount];

                GameObject Player = GameMain.data.mainPlayer.gameObject;

                guideArrowBase = new GameObject("guideArrowBase");
                guideArrowBase.transform.parent = Player.transform;
                guideArrowBase.transform.localPosition = new Vector3(0, 0.8f, 0);
                guideArrowBase.transform.localScale = new Vector3(1, 1, 1);

                for (int i = 0; i < Main.maxMarkerAmount; i++)
                {
                    guideArrow[i] = new GameObject("guideArrow" + i);
                    guideArrow[i].transform.parent = guideArrowBase.transform;
                    guideArrow[i].AddComponent<CreateTriangleMesh>();
                    guideArrow[i].AddComponent<MeshRenderer>();
                    guideArrow[i].AddComponent<MeshFilter>();
                    guideArrow[i].transform.localPosition = new Vector3(0, 0, 0);
                }
            }
        }
    }

    public class CreateTriangleMesh : MonoBehaviour
    {
        void Start()
        {
            var mesh = new Mesh();

            var Vertices = new List<Vector3> {
                      new Vector3 (-0.4f,0, 2.4f),
                      new Vector3 (0, 0, 3f),
                      new Vector3 (0.4f, 0, 2.4f),
                      new Vector3 (0, 0, 2.6f),
               };
            mesh.SetVertices(Vertices);
            var triangles = new List<int> { 0, 1, 3, 1, 2, 3, 3, 2, 1, 3, 1, 0 };
            mesh.SetTriangles(triangles, 0);

            var meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            var renderer = GetComponent<MeshRenderer>();
            renderer.material.color = Color.blue; // new Color(1, 0.7f, 0, 1);
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", Color.blue);// new Color(1, 0.7f, 0, 1));
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }
    }
}
