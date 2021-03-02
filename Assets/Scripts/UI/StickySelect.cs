using UnityEngine;
using UnityEngine.EventSystems;

public class StickySelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler {

    public void OnPointerEnter(PointerEventData eventData) {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData) {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnDeselect(BaseEventData data) {
        if (!EventSystem.current.alreadySelecting) {
            EventSystem.current.SetSelectedGameObject(data.selectedObject);
        }
    }
}
