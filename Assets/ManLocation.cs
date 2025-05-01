using UnityEngine;

public class MoveCylinder : MonoBehaviour
{
    private WebSocketClient wsClient;

    void Start()
    {
        // Find the WebSocketClient in the scene
        wsClient = FindObjectOfType<WebSocketClient>();
    }

    void Update()
    {
        double lon = (wsClient.currentLongitude + 111.64613294330745d)*100000;
        double lat = (wsClient.currentLatitude - 40.24766580572292)*100000; //000.00002
            
        Debug.Log($"CONV LAT: {(float)lat}, CONV LON: {(float)lon}");


        // Map lat/lon to Unity position (basic example)
        Vector3 newPosition = new Vector3((float)lon, 1, (float)lat);

            MoveToPosition(newPosition);

    }

    void MoveToPosition(Vector3 position)
    {
        transform.position = position;
    }
}

