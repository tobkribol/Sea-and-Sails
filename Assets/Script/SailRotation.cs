using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailRotation : MonoBehaviour
{
    [SerializeField] Transform objectwind;
    [SerializeField] float rotationSpeedSails = 45.0f;
    WindManagment wm;
    public float WindSpeedBoost;
    private float windSpeed;



    // Start is called before the first frame update
    void Start()
    {
        wm = objectwind.GetComponent<WindManagment>();
    }

    private void Update()
    {

        //Rotation
        if (Input.GetKey(KeyCode.Q))
        {
            Vector3 rotationToAdd = new Vector3(0, 0, rotationSpeedSails * Time.deltaTime);
            transform.Rotate(rotationToAdd);
            //transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.z + rotationSpeed));
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Vector3 rotationToAdd = new Vector3(0, 0, -rotationSpeedSails * Time.deltaTime);
            transform.Rotate(rotationToAdd);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Change ship sail to automatic match wind direction
        //transform.eulerAngles = wm.windDirection + new Vector3(0,0,-180);

        //https://www.playatlas.com/index.php?/forums/topic/105894-wind-mechanics-rotation-and-sail-types-a-comprehensive-guide/

    }

    public float GetSailWindAngle()
    {
        return Mathf.Abs(180f - Mathf.Abs(wm.windDirection.z - transform.eulerAngles.z));
    }

    public float GetWindSpeed()
    {
        return wm.windSpeed;
    }
    
    public float GetWindSpeedBoost()
    {
        float SailWindAngle = GetSailWindAngle();
        if (SailWindAngle < 10)
            WindSpeedBoost = 1.0f;
        else if (SailWindAngle < 20)
            WindSpeedBoost = 0.8f;
        else if (SailWindAngle < 40)
            WindSpeedBoost = 0.4f;
        else if (SailWindAngle < 100)
            WindSpeedBoost = 0.2f;
        else if (SailWindAngle > 100)
            WindSpeedBoost = 0.01f;
        return WindSpeedBoost * wm.windSpeed;

    }



}