using UnityEngine;

public class PositionCameraToPlayer : MonoBehaviour {

    public Transform playerTransform;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    void Update() {
        this.transform.position = playerTransform.position + positionOffset;
        this.transform.rotation = Quaternion.Euler(new Vector3(rotationOffset.x, rotationOffset.y + (playerTransform.eulerAngles.y + 90), rotationOffset.z));
    }
}
