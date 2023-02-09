using System.Collections;
using UnityEngine;

public class ConsoleToGUI : MonoBehaviour
{
    string myLog = "*begin log";
    string filename = "";
    bool doShow = false;
    int kChars = 700;
    void OnEnable() { Application.logMessageReceived += Log; InvokeRepeating("LogTicker", 0.5f, 0.5f); }
    void OnDisable() { Application.logMessageReceived -= Log; }
    public void Toggle()
    {
        doShow = !doShow;
    }
    public void Log(string logString, string stackTrace, LogType type)
    {
        if (logString != "Trying to Invoke method: ConsoleToGUI.LogTicker couldn't be called.")
        {
            // for onscreen...
            myLog = type.ToString() + " :: " + myLog + "\n" + logString;
            if (myLog.Length > kChars) { myLog = myLog.Substring(myLog.Length - kChars); }

            // for the file ...
            if (filename == "")
            {
                string d = Application.persistentDataPath + "/YOUR_LOGS";
                System.IO.Directory.CreateDirectory(d);
                string r = System.DateTime.Now.ToString();
                filename = d + "/log-" + r + ".txt";
            }
            try { System.IO.File.AppendAllText(filename, type.ToString() + " :: " + logString + "\n"); }
            catch { }
        }
    }

    void OnGUI()
    {
        if (!doShow) { return; }
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
           new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
        GUI.TextArea(new Rect(10, 10, 540, 370), myLog);
    }

    IEnumerator LogTicker(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        Application.logMessageReceived += Log;
    }
}
