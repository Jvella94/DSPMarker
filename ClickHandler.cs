using UnityEngine;
using UnityEngine.EventSystems;

namespace DSPMarker
{
    public class UIClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                MarkerList.OnRightClick(eventData.pointerPress);
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
            }
        }
    }
}