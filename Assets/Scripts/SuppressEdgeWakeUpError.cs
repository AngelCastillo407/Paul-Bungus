using UnityEngine;

public class SuppressEdgeWakeUpError : MonoBehaviour
{
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Ignore only the UnityEditor.Graphs.Edge.WakeUp error
        if (logString.Contains("UnityEditor.Graphs.Edge.WakeUp"))
            return;

        // Forward all other messages to the console
        Debug.unityLogger.Log(type, logString);
    }
}
