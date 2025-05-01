using System;
using UnityEngine;
using NativeWebSocket;
using Unity.VisualScripting.FullSerializer;



[System.Serializable]
public class GpsData
{
    public float latitude;
    public float longitude;
    public float compass;
}

public class WebSocketClient : MonoBehaviour
{
    WebSocket websocket;
    public double currentLatitude { get; private set; }
    public double currentLongitude { get; private set; }
    public double currentCompass { get; private set; }
    async void Start()
    {
        websocket = new WebSocket("ws://10.37.110.184:8080");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received OnMessage! Data: " + message);

            try
            {
                // Parse JSON into GpsData
                GpsData gps = JsonUtility.FromJson<GpsData>(message);

                // Assign to variables
                currentLatitude = gps.latitude;
                currentLongitude = gps.longitude;
                currentCompass = gps.compass;

                // Log the parsed values
                Debug.Log($"Latitude: {currentLatitude}, Longitude: {currentLongitude}, Compass: {currentCompass}");
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to parse GPS data: " + ex.Message);
            }
        };

        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
#endif
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}