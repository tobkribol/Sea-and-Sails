using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManagment : MonoBehaviour
{

    [HideInInspector] public Vector3 windDirection;
    [HideInInspector] private Vector3 rotationToAdd;
    [HideInInspector] public float windFactor;
    public float windChangeIntervall = 10.0f;
    public float windSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //Initiate wind direction
        windDirection = transform.eulerAngles;
        windDirection.z = Random.Range(0.0f, 360.0f);
        transform.eulerAngles = windDirection;

        //start Routine for changing wind direction
        StartCoroutine("ChangeWindDirection");
        StartCoroutine("ChangeWindSpeed");

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ChangeWindDirection()
    {

        while (true)
        {
            yield return new WaitForSeconds(windChangeIntervall);
            rotationToAdd = new Vector3(0, 0, Random.Range(-5.0f, 5.0f));
            transform.Rotate(rotationToAdd);
            windDirection += rotationToAdd;

            Debug.Log("ChangeWindDirection: " + windDirection);
        }

    }

    IEnumerator ChangeWindSpeed()
    {

        while (true)
        {
            yield return new WaitForSeconds(windChangeIntervall);
            windSpeed = GetRandomWindSpeedValue();
            Debug.Log("ChangeWindSpeed: " + windSpeed);
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

}
