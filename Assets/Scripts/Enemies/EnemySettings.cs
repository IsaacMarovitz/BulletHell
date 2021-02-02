using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Settings")]
public class EnemySettings : ScriptableObject {
    [Header("Visibility")]
    [Range(0, 15)]
    public float visibilityRadius = 3;
    [Range(0, 180)]
    public float viewAngle = 150;
    [Header("Seperation")]
    public float seperationForce = 0.5f;
    public float seperationWeight = 0.1f;
    [Header("Alignment")]
    public float alignmentWeight = 0.1f;
    [Header("Cohesion")]
    public float cohesionWeight = 0.5f;

    public float velocityLimit = 2;
}
