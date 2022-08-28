using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Camera camara;
    private float ScrollWheelChange = 0f;
    [SerializeField] private float ScrollWheelChangeSpeed = 4f;
    [SerializeField] private float ScrollWheelChangeMinZoom = -1.5f;
    [SerializeField] private float ScrollWheelChangeMaxZoom = -12f;

    private void Update()
    {
        //Move camera to player
        transform.position = new Vector3(player.position.x, player.position.y, Camera.main.transform.position.z);

        //Scroll camera        
        ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");
        //Debug.Log("scroll: " + ScrollWheelChange + "position: " + Camera.main.transform.position.z);

        if (ScrollWheelChange != 0f && Camera.main.transform.position.z <= ScrollWheelChangeMinZoom && Camera.main.transform.position.z >= ScrollWheelChangeMaxZoom)
        {
            Camera.main.transform.position += Camera.main.transform.forward * ScrollWheelChange * ScrollWheelChangeSpeed;
        }
        else if (Camera.main.transform.position.z > ScrollWheelChangeMinZoom)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, ScrollWheelChangeMinZoom);
        }
        else if (Camera.main.transform.position.z < ScrollWheelChangeMaxZoom)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, ScrollWheelChangeMaxZoom);
        }

    }
}
