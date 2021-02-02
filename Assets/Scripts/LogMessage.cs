using UnityEngine;

public static class LogMessage {
    public static void Send(GameObject gameObject, string message) {
        Debug.Log($"<b>{gameObject.name}:</b> {message}");
    }

    public static void Send(string title, string message) {
        Debug.Log($"<b>{title}:</b> {message}");
    }
}