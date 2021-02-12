using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if (GUILayout.Button("Die")) {
            foreach (var item in Selection.gameObjects) {
                item.GetComponent<EnemyHealth>().Die();
            }
        }
    }
}