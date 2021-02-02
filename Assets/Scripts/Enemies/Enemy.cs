using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public EnemySettings enemySettings;

    public List<Enemy> nearbyEnemies = new List<Enemy>();
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 seperationVector;
    public Vector2 alignmentVector;
    public Vector2 cohesionVector;
    public bool debug = false;

    void Start() {
        position = new Vector2(this.transform.position.x, this.transform.position.z);
    }

    void OnDrawGizmos() {
        if (enemySettings != null) {
            Gizmos.color = Color.green;
            foreach (var enemy in nearbyEnemies) {
                Gizmos.DrawLine(this.transform.position, enemy.transform.position);
            }
            Gizmos.color = Color.red;
            Vector3 viewAngleLine1 = new Vector3(this.transform.position.x + enemySettings.visibilityRadius * Mathf.Cos((enemySettings.viewAngle - transform.eulerAngles.y) * Mathf.Deg2Rad), 0, this.transform.position.z + enemySettings.visibilityRadius * Mathf.Sin((enemySettings.viewAngle - transform.eulerAngles.y) * Mathf.Deg2Rad));
            Vector3 viewAngleLine2 = new Vector3(this.transform.position.x + enemySettings.visibilityRadius * Mathf.Cos((-enemySettings.viewAngle - transform.eulerAngles.y) * Mathf.Deg2Rad), 0, this.transform.position.z + enemySettings.visibilityRadius * Mathf.Sin((-enemySettings.viewAngle - transform.eulerAngles.y)  * Mathf.Deg2Rad));
            Gizmos.DrawLine(this.transform.position, viewAngleLine1);
            Gizmos.DrawLine(this.transform.position, viewAngleLine2);
            if (Application.isPlaying) {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(new Vector3(seperationVector.x, 0, seperationVector.y), 0.1f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(this.transform.position, new Vector3(alignmentVector.x, 0, alignmentVector.y) + this.transform.position);
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(new Vector3(cohesionVector.x, 0, cohesionVector.y), 0.1f);
            }
        }
    }

    void Update() {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, enemySettings.visibilityRadius);
        nearbyEnemies = new List<Enemy>();
        foreach (var collider in hitColliders) {
            if (Vector3.Angle(-transform.right, this.transform.position - collider.transform.position) < enemySettings.viewAngle) {
                Enemy enemy = collider.GetComponentInParent<Enemy>();
                if (!nearbyEnemies.Contains(enemy) && enemy != this && enemy != null) {
                    nearbyEnemies.Add(collider.GetComponentInParent<Enemy>());
                }
            }
        }

        Vector2 acceleration = Vector2.zero;

        seperationVector = Seperation() * enemySettings.seperationWeight;
        acceleration += seperationVector;
        alignmentVector = Alignment() * enemySettings.alignmentWeight;
        acceleration += alignmentVector;
        cohesionVector = Cohesion() * enemySettings.cohesionWeight;
        acceleration += cohesionVector;

        velocity += acceleration * Time.deltaTime;
        velocity = LimitVelocity(velocity);
        position += velocity;
        this.transform.position = new Vector3(position.x, 0, position.y);
        if (velocity != Vector2.zero) {
            Debug.DrawLine(this.transform.position, new Vector3(velocity.x, 0, velocity.y) + this.transform.position, Color.white);
            this.transform.LookAt((new Vector3(velocity.x, 0, velocity.y) + this.transform.position));
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y -90, 0);
        }
    }

    public Vector2 Seperation() {
        Vector2 seperationVelocity = Vector2.zero;

        foreach (var enemy in nearbyEnemies) {
            Vector2 neighbourToEnemy = position - enemy.position;
            seperationVelocity += neighbourToEnemy * enemySettings.seperationForce;
        }

        seperationVelocity /= nearbyEnemies.Count;

        if (debug)
            Debug.Log(seperationVelocity.normalized);

        return seperationVelocity.normalized;
    }

    public Vector2 Alignment() {
        Vector2 alignmentVelocity = Vector2.zero;

        foreach (var enemy in nearbyEnemies) {
            alignmentVelocity += enemy.velocity;
        }

        alignmentVelocity /= nearbyEnemies.Count;

        if (debug)
            Debug.Log(alignmentVelocity.normalized);

        return alignmentVelocity.normalized;
    }

    public Vector2 Cohesion() {
        Vector2 centreOfNeighbours = Vector2.zero;
        
        foreach (var enemy in nearbyEnemies) {
            centreOfNeighbours += enemy.position;
        }

        if (nearbyEnemies.Count > 0) {
            centreOfNeighbours /= nearbyEnemies.Count;
        }
        //centreOfNeighbours -= position;

        if (debug)
            Debug.Log(centreOfNeighbours);

        return centreOfNeighbours.normalized;
    }

    public Vector2 LimitVelocity(Vector2 velocity) {
        Vector2 returnVelocity = Vector2.zero;

        if (Mathf.Abs(velocity.x) > enemySettings.velocityLimit || Mathf.Abs(velocity.y) > enemySettings.velocityLimit) {
            returnVelocity = (velocity / new Vector2(Mathf.Abs(velocity.x), Mathf.Abs(velocity.y))) * enemySettings.velocityLimit;
        } else {
            returnVelocity = velocity;
        }

        return returnVelocity;
    }
}
