using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Settings")]
public class EnemySettings : ScriptableObject {
    [Header("Visibility")]
    [Range(0, 15)]
    public float viewDistance = 8;
    [Range(0, 180)]
    public float viewAngle = 150;
    [Header("Seperation")]
    public float seperationWeight = 25;
    [Header("Alignment")]
    public float alignmentWeight = 14;
    [Header("Cohesion")]
    public float cohesionWeight = 13;
    [Header("Target")]
    public float targetWeight = 16;
    public float targetDistance = 0;
    public float targetDistanceWeight = 20;
    [Header("Movement")]
    public float maxSpeed = 30;
    public float minSpeed = 10;
    public float positionLimitForce = 20;
    public float positionLimitWeight = 1;
    public Vector2 size = new Vector2(100, 100);
}
