using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyManager))]
public class EnemyManagerEditor : Editor {

    public bool showDroneMenu = false;
    public bool showCannonMenu = false;
    public bool showFighterMenu = false;

    public override void OnInspectorGUI() {
        serializedObject.Update();

        SerializedProperty droneEnemySettings_prop = serializedObject.FindProperty("droneEnemySettings");
        SerializedProperty droneEnemyPrefab_prop = serializedObject.FindProperty("droneEnemyPrefab");
        SerializedProperty cannonEnemySettings_prop = serializedObject.FindProperty("cannonEnemySettings");
        SerializedProperty cannonEnemyPrefab_prop = serializedObject.FindProperty("cannonEnemyPrefab");
        SerializedProperty fighterEnemySettings_prop = serializedObject.FindProperty("fighterEnemySettings");
        SerializedProperty fighterEnemyPrefab_prop = serializedObject.FindProperty("fighterEnemyPrefab");

        showDroneMenu = EditorGUILayout.BeginFoldoutHeaderGroup(showDroneMenu, "Drone Settings");
        if (showDroneMenu) {
            EditorGUILayout.PropertyField(droneEnemySettings_prop, new GUIContent("Settings"));
            EditorGUILayout.PropertyField(droneEnemyPrefab_prop, new GUIContent("Prefab"));
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(10);
        
        showCannonMenu = EditorGUILayout.BeginFoldoutHeaderGroup(showCannonMenu, "Cannon Settings");
        if (showCannonMenu) {
            EditorGUILayout.PropertyField(cannonEnemySettings_prop, new GUIContent("Settings"));
            EditorGUILayout.PropertyField(cannonEnemyPrefab_prop, new GUIContent("Prefab"));
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(10);

        showFighterMenu = EditorGUILayout.BeginFoldoutHeaderGroup(showFighterMenu, "Fighter Settings");
        if (showFighterMenu) {
            EditorGUILayout.PropertyField(fighterEnemySettings_prop, new GUIContent("Settings"));
            EditorGUILayout.PropertyField(fighterEnemyPrefab_prop, new GUIContent("Prefab"));
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(10);

        SerializedProperty compute_prop = serializedObject.FindProperty("compute");
        EditorGUILayout.PropertyField(compute_prop, new GUIContent("Compute Shader"));
        SerializedProperty numEnemiesToSpawn_prop = serializedObject.FindProperty("numEnemiesToSpawn");
        EditorGUILayout.PropertyField(numEnemiesToSpawn_prop, new GUIContent("No. of Enemies to Spawn"));
        SerializedProperty cage_prop = serializedObject.FindProperty("cage");
        EditorGUILayout.PropertyField(cage_prop, new GUIContent("Cage"));
        SerializedProperty player_prop = serializedObject.FindProperty("player");
        EditorGUILayout.PropertyField(player_prop, new GUIContent("Player"));
        SerializedProperty bulletPrefab_prop = serializedObject.FindProperty("bulletPrefab");
        EditorGUILayout.PropertyField(bulletPrefab_prop, new GUIContent("Bullet Prefab"));
        SerializedProperty bulletParent_prop = serializedObject.FindProperty("bulletParent");
        EditorGUILayout.PropertyField(bulletParent_prop, new GUIContent("Bullet Parent"));

        serializedObject.ApplyModifiedProperties();
    }
}