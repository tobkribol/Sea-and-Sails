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
        //Set Compass arrow to sails
        wm.sailRotationArrow.eulerAngles = transform.eulerAngles + new Vector3(0,0,180f);

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
        //https://navalaction.fandom.com/wiki/Sailing_Profile

        float SailWindAngle = GetSailWindAngle();
        if (SailWindAngle < 5)
            //Running | Accually less effective for large ships
            WindSpeedBoost = 0.9f;
        else if (SailWindAngle < 75)
            //Broad Reach | Most effective
            WindSpeedBoost = 1.0f;
        else if (SailWindAngle < 90)
            //Beam Reach
            WindSpeedBoost = 0.80f;
        else if (SailWindAngle < 135)
            //Close Hauled
            WindSpeedBoost = 0.5f;
        else if (SailWindAngle > 150)
            //No Sail zone
            WindSpeedBoost = 0.01f;
        return WindSpeedBoost * wm.windSpeed;

    }



}