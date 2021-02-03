using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Settings")]
public class EnemySettings : ScriptableObject {
    [Header("Visibility")]
    [Range(0, 15)]
    public float visibilityRadius = 3;
    [Range(0, 180)]
    public float viewAngle = 150;
    [Header("Seperation")]
    public float seperationWeight = 0.1f;
    [Header("Alignment")]
    public float alignmentWeight = 0.1f;
    [Header("Cohesion")]
    public float cohesionWeight = 0.5f;
    [Header("Movement")]
    public float maxSpeed = 0.1f;
    public float minSpeed = 0.05f;
    public float positionLimitForce = 10;
    public float positionLimitWeight = 1;
    public Vector2 size;
}
