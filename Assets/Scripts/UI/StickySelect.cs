using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class StickySelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler {

    public TMP_Text selectedText;
    public Color32 selectedColor = new Color32(180, 180, 180, 255);

    public void Update() {
        if (selectedText != null) {
            if (EventSystem.current.currentSelectedGameObject == gameObject) {
                selectedText.color = selectedColor;
            } else {
                selectedText.color = Color.white;
            }
        }
    }

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
