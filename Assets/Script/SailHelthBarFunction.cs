using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SailHelthBarFunction : MonoBehaviour
{
    private static Image HealthBarImage;

    private void Start()
    {
        HealthBarImage = GetComponent<Image>();
    }

    public static void SetHealthBarValue(float value)
    {
        //Debug.Log(HealthBarImage.fillAmount);
        HealthBarImage.fillAmount = value;

        if (GetHealthBarValue() <= 0.25f)
        {
            SetHealthBarColor(Color.red);

        }

        else if (GetHealthBarValue() <= 0.75f)
        {
            SetHealthBarColor(Color.yellow);

        }

        else
        {
            SetHealthBarColor(Color.green);
        }
    }

    public static float GetHealthBarValue()
    {
        return HealthBarImage.fillAmount;
    }

    /// <summary>
    /// Sets the health bar color
    /// </summary>
    /// <param name="healthColor">Color </param>
    public static void SetHealthBarColor(Color healthColor)
    {
        HealthBarImage.color = healthColor;
    }

    /// <summary>
    /// Initialize the variable
    /// </summary>

}
