using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class StickySelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler {

    public TMP_Text selectedText;
    public Color32 selectedColor = new Color32(180, 180, 180, 255);

    // This class makes sure that the currently selected object always remains selected until the
    // next object is selected so that people using keyboard controls to navigate the menu are never stuck

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
