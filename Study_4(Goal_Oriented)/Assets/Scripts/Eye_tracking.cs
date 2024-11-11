using System.IO;
using Tobii.XR;
using UnityEngine;

public class Eye_tracking : MonoBehaviour
{
    private StreamWriter _dataFile;
    private bool _receivedData;

    private void Start()
    {
        // Create a file to record eye tracking data to
        var filePath = Path.Combine(Application.persistentDataPath, "recording.json");
        _dataFile = new StreamWriter(filePath);
        _dataFile.WriteLine("[");
    }

    private void Update()
    {
        // Dequeue all new data received since last frame
        while (TobiiXR.Advanced.QueuedData.Count > 0)
        {
            var data = TobiiXR.Advanced.QueuedData.Dequeue();

            // Serialize to json and append to file
            if (_receivedData) _dataFile.WriteLine(",");
            var json = JsonUtility.ToJson(data);
            _dataFile.Write(json);
            _receivedData = true;
        }
    }

    private void OnDestroy()
    {
        _dataFile.Write("]");

        // Make sure to close the file before terminating the application
        _dataFile.Close();
    }
}