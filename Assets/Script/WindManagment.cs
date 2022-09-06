using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindManagment : MonoBehaviour
{
    [Header("Wind Text input")]
    [HideInInspector] public Vector3 windDirection;
    [HideInInspector] private Vector3 rotationToAdd;
    [HideInInspector] public float windFactor;
    [SerializeField] private Text windDirText; //need UnityEngine.UI
    [SerializeField] private Text windSpeedText; //need UnityEngine.UI

    [Header("Arrow")]
    [SerializeField] public Transform sailRotationArrow;
    [SerializeField] public Transform windRotationArrow;

    [Header("Wind Parameters")]
    public float windChangeIntervall = 10.0f;
    [SerializeField] private bool constatnWindSpeed = true;
    public float windSpeed = 6.0f;
    private float windSpeedSetpoint;
    private float windDirectionSetpoint;

    // Start is called before the first frame update
    void Start()
    {
        //Initiate wind direction
        windDirection = windRotationArrow.transform.eulerAngles;
        windDirection.z = Random.Range(0.0f, 360.0f);
        windRotationArrow.transform.eulerAngles = windDirection;

        //start Routine for changing wind direction
        StartCoroutine("ChangeWindDirection");
        StartCoroutine("ChangeWindSpeed");

        windSpeedSetpoint = windSpeed;
        windDirectionSetpoint = windDirection.z;

    }

    // Update is called once per frame
    void Update()
    {
        //Wind direction change
        rotationToAdd = new Vector3(0, 0, SmoothWindChange(windDirection.z, windDirectionSetpoint, 5.0f));

        //transform.rotation.SetEulerRotation(0, 0, 0);
        windRotationArrow.transform.rotation = Quaternion.Euler(0, 0, rotationToAdd.z);
        windDirection = rotationToAdd;
        windDirText.text = "Dir: " + (360.0f - Mathf.Round(windDirection.z)) + " °";

        //Wind Speed change
        if (!constatnWindSpeed)
        {
            windSpeed = SmoothWindChange(windSpeed, windSpeedSetpoint, 0.1f);
        }
        windSpeedText.text = string.Format("Speed: {0:F1} m/s", windSpeed);
    }

    IEnumerator ChangeWindDirection()
    {

        while (true)
        {
            yield return new WaitForSeconds(windChangeIntervall);
            //rotationToAdd = new Vector3(0, 0, Random.Range(-5.0f, 5.0f));
            //transform.Rotate(rotationToAdd);
            //windDirection += rotationToAdd;
            windDirectionSetpoint = GetRandomWindDirectionChange();
            windDirText.text = "Dir: " + Mathf.Round(windDirection.z);
            Debug.Log("ChangeWindDirectionTo: " + windDirectionSetpoint);
        }

    }

    IEnumerator ChangeWindSpeed()
    {
        if (!constatnWindSpeed)
        {
            while (true)
            {
                yield return new WaitForSeconds(windChangeIntervall);
                windSpeedSetpoint = GetRandomWindSpeedValue();
                Debug.Log("ChangeWindSpeedTo: " + windSpeedSetpoint);
            }
        }
    }

    float GetRandomWindSpeedValue()
    //Weibull Distribution https://www.wind-energy-the-facts.org/the-annual-variability-of-wind-speed-7.html
    {
        float rand = Random.value;
        if (rand <= .222f)
            return Random.Range(0.0f, 3.0f);
        else if (rand <= .633)
            return Random.Range(3.0f, 6.0f);
        else if (rand <= .895)
            return Random.Range(6.0f, 9.0f);
        else if (rand <= .981)
            return Random.Range(9.0f, 12.0f);
        else if (rand <= .998)
            return Random.Range(12.0f, 15.0f);
        else
            return Random.Range(15.0f, 20.0f);
    }

    float GetRandomWindDirectionChange()
    //Probability distribution https://www.researchgate.net/figure/Probability-distribution-a-top-measured-rate-of-change-wind-direction-and-b-lower_fig1_245525320
    {
        int flipFactor = 1;
        float windDir;
        float rand = Random.value;

        if (rand <= .50f)
        {
            flipFactor = -1;
        }

        if (rand <= .75f)
            windDir = Random.Range(0.0f, 5.0f);
        else if (rand <= .95)
            windDir = Random.Range(5.0f, 10.0f);
        else if (rand <= .975)
            windDir = Random.Range(10.0f, 20.0f);
        else if (rand <= .98)
            windDir = Random.Range(20.0f, 45.0f);
        else if (rand <= .995)
            windDir = Random.Range(45.0f, 60.0f);
        else
            windDir = Random.Range(60.0f, 90.0f);

        return windDir * flipFactor;
    }



    private float SmoothWindChange(float currentvalue, float newvalue, float stepSize)
    {
        float currentWind = currentvalue;

        if(newvalue < currentvalue)
        {
            currentWind -= stepSize * Time.deltaTime;
        }

        else if (newvalue > currentvalue)
        {
            currentWind += stepSize * Time.deltaTime;
        }
        else
        {
            currentWind = currentvalue;
        }

        return currentWind;
    }

}
