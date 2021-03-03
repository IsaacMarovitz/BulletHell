using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyManager))]
public class EnemyManagerEditor : Editor {

    public override void OnInspectorGUI() {
        serializedObject.Update();

        SerializedProperty droneEnemySettings_prop = serializedObject.FindProperty("droneEnemySettings");
        SerializedProperty droneEnemyPrefab_prop = serializedObject.FindProperty("droneEnemyPrefab");
        SerializedProperty bulletPrefab_prop = serializedObject.FindProperty("bulletPrefab");
        SerializedProperty bulletParent_prop = serializedObject.FindProperty("bulletParent");
        SerializedProperty numOfDronesToSpawn_prop = serializedObject.FindProperty("numOfDronesToSpawn");

        EditorGUILayout.LabelField("Drone Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(droneEnemySettings_prop, new GUIContent("Settings"));
        EditorGUILayout.PropertyField(droneEnemyPrefab_prop, new GUIContent("Prefab"));
        EditorGUILayout.PropertyField(bulletPrefab_prop, new GUIContent("Bullet Prefab"));
        EditorGUILayout.PropertyField(bulletParent_prop, new GUIContent("Bullet Parent"));
        EditorGUILayout.PropertyField(numOfDronesToSpawn_prop, new GUIContent("No. to Spawn"));

        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(10);
        
        SerializedProperty cannonEnemySettings_prop = serializedObject.FindProperty("cannonEnemySettings");
        SerializedProperty cannonEnemyPrefab_prop = serializedObject.FindProperty("cannonEnemyPrefab");
        SerializedProperty numOfCannonsToSpawn_prop = serializedObject.FindProperty("numOfCannonsToSpawn");

        EditorGUILayout.LabelField("Cannon Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(cannonEnemySettings_prop, new GUIContent("Settings"));
        EditorGUILayout.PropertyField(cannonEnemyPrefab_prop, new GUIContent("Prefab"));
        EditorGUILayout.PropertyField(numOfCannonsToSpawn_prop, new GUIContent("No. to Spawn"));
        
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(10);

        SerializedProperty fighterEnemySettings_prop = serializedObject.FindProperty("fighterEnemySettings");
        SerializedProperty fighterEnemyPrefab_prop = serializedObject.FindProperty("fighterEnemyPrefab");
        SerializedProperty missilePrefab_prop = serializedObject.FindProperty("missilePrefab");
        SerializedProperty missileParent_propt = serializedObject.FindProperty("missileParent");
        SerializedProperty numOfFightersToSpawn_prop = serializedObject.FindProperty("numOfFightersToSpawn");

        EditorGUILayout.LabelField("Fighter Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(fighterEnemySettings_prop, new GUIContent("Settings"));
        EditorGUILayout.PropertyField(fighterEnemyPrefab_prop, new GUIContent("Prefab"));
        EditorGUILayout.PropertyField(missilePrefab_prop, new GUIContent("Missile Prefab"));
        EditorGUILayout.PropertyField(missileParent_propt, new GUIContent("Missile Parent"));
        EditorGUILayout.PropertyField(numOfFightersToSpawn_prop, new GUIContent("No. to Spawn"));
        
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(10);

        SerializedProperty compute_prop = serializedObject.FindProperty("compute");
        EditorGUILayout.PropertyField(compute_prop, new GUIContent("Compute Shader"));
        SerializedProperty player_prop = serializedObject.FindProperty("player");
        EditorGUILayout.PropertyField(player_prop, new GUIContent("Player"));
        SerializedProperty menuUI_prop = serializedObject.FindProperty("menuUI");
        EditorGUILayout.PropertyField(menuUI_prop, new GUIContent("Menu UI"));

        serializedObject.ApplyModifiedProperties();
    }
}