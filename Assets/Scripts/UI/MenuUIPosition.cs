using UnityEngine;

public class MenuUIPosition : MonoBehaviour {

    public float fourByThreePosition;
    public float wideRatioPosition;

    // Adjusts the position of the UI so that it stays properly in view at different aspect ratios
    void Update() {
        if (Camera.main.aspect < 1.4) {
            this.transform.localPosition = new Vector3(fourByThreePosition, this.transform.localPosition.y, this.transform.localPosition.z);
        } else {
            this.transform.localPosition = new Vector3(wideRatioPosition, this.transform.localPosition.y, this.transform.localPosition.z);
        }
    }
}
