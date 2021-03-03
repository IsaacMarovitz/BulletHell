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
    public Vector2 targetVector;
    public Vector2 targetDistanceVector;
    public Vector2 positionLimitVector;
    public int numPerceivedEnemies;
    public bool debug = false;
    public EnemyManager enemyManager;
    public GameObject player;

    /*
    This is the base enemy class that all other enemies inherit from
    it contains all the vectors and calculations for the boids logic
    as well as some debug tools and gizmos
    */

    void Start() {
        position = Vector3ToVector2(this.transform.position);
        float startSpeed = (enemySettings.minSpeed + enemySettings.maxSpeed) / 2;
        velocity = transform.forward * startSpeed;
    }

    void OnDrawGizmos() {
        if (enemySettings != null && debug) {
            Gizmos.color = Color.green;
            foreach (var enemy in nearbyEnemies) {
                Gizmos.DrawLine(this.transform.position, enemy.transform.position);
            }
            Gizmos.color = Color.red;
            Vector3 viewAngleLine1 = new Vector3(this.transform.position.x + enemySettings.viewDistance * Mathf.Cos((enemySettings.viewAngle - transform.eulerAngles.y) * Mathf.Deg2Rad), 0, this.transform.position.z + enemySettings.viewDistance * Mathf.Sin((enemySettings.viewAngle - transform.eulerAngles.y) * Mathf.Deg2Rad));
            Vector3 viewAngleLine2 = new Vector3(this.transform.position.x + enemySettings.viewDistance * Mathf.Cos((-enemySettings.viewAngle - transform.eulerAngles.y) * Mathf.Deg2Rad), 0, this.transform.position.z + enemySettings.viewDistance * Mathf.Sin((-enemySettings.viewAngle - transform.eulerAngles.y)  * Mathf.Deg2Rad));
            Gizmos.DrawLine(this.transform.position, viewAngleLine1);
            Gizmos.DrawLine(this.transform.position, viewAngleLine2);
            if (Application.isPlaying) {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(Vector2ToVector3(seperationVector), 0.1f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(this.transform.position, Vector2ToVector3(alignmentVector) + this.transform.position);
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(this.transform.position, Vector2ToVector3(positionLimitVector) + this.transform.position);
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(Vector2ToVector3(cohesionVector), 0.1f);
            }
        }
    }
    
    // This function is called by the EnemyManager when it gets the values back from
    // the compute shader. The vectors that it get's back are then error checked, and
    // then applied to the position.
    public void UpdateEnemy() {
        Vector2 acceleration = Vector2.zero;

        if (numPerceivedEnemies > 0) {
            seperationVector /= numPerceivedEnemies;
            acceleration += CheckNullAndTimes(seperationVector.normalized, enemySettings.seperationWeight);
            alignmentVector /= numPerceivedEnemies;
            acceleration += CheckNullAndTimes(alignmentVector.normalized, enemySettings.alignmentWeight);
            cohesionVector /= numPerceivedEnemies;
            cohesionVector -= position;
            acceleration += CheckNullAndTimes(cohesionVector.normalized, enemySettings.cohesionWeight);
        }
        targetVector = Vector3ToVector2(player.transform.position) - position;
        acceleration += CheckNullAndTimes(targetVector.normalized, enemySettings.targetWeight);
        targetDistanceVector = TargetDistance();
        acceleration += CheckNullAndTimes(targetDistanceVector.normalized, enemySettings.targetDistanceWeight);

        positionLimitVector = LimitPosition();
        acceleration += CheckNullAndTimes(positionLimitVector, enemySettings.positionLimitWeight);

        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector2 dir = velocity / speed;
        speed = Mathf.Clamp(speed, enemySettings.minSpeed, enemySettings.maxSpeed);
        velocity = CheckNullAndTimes(dir * speed, 1);

        position += velocity * Time.deltaTime;
        this.transform.position = Vector2ToVector3(position);
        if (debug)
            Debug.DrawLine(this.transform.position, Vector2ToVector3(velocity) + this.transform.position, Color.white);
        this.transform.LookAt((Vector2ToVector3(velocity) + this.transform.position));
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y -90, 0);
    }

    public Vector2 TargetDistance() {
        Vector2 targetDistanceVector = Vector2.zero;
        float distance = Vector2.Distance(position, Vector3ToVector2(player.transform.position));
        if (distance < enemySettings.targetDistance) {
            targetDistanceVector = (position - Vector3ToVector2(player.transform.position)) * enemySettings.targetDistanceWeight;
        }
        return targetDistanceVector;
    }

    // Limits the position of enemies within the bounds of the map by applying a force that moves
    // them back into the main area.
    public Vector2 LimitPosition() {
        Vector2 positionLimitVector = Vector2.zero;

        if (position.x < -enemySettings.size.x) {
            positionLimitVector.x = enemySettings.positionLimitForce;
        }
        if (position.x > enemySettings.size.x) {
            positionLimitVector.x = -enemySettings.positionLimitForce;
        }

        if (position.y < -enemySettings.size.y) {
            positionLimitVector.y = enemySettings.positionLimitForce;
        }
        if (position.y > enemySettings.size.y) {
            positionLimitVector.y = -enemySettings.positionLimitForce;
        }

        return positionLimitVector;
    }

    public Vector3 Vector2ToVector3(Vector2 vector) {
        return new Vector3(vector.x, 0, vector.y);
    }

    public Vector2 Vector3ToVector2(Vector3 vector) {
        return new Vector2(vector.x, vector.z);
    }

    public Vector2 CheckNullAndTimes(Vector2 vector, float weight) {
        if (float.IsNaN(vector.x) || float.IsNaN(vector.x)) {
            return Vector2.zero;
        } else {
            return vector * weight;
        }
    }
}
