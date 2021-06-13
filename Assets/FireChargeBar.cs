using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireChargeBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Slider slider;
    [SerializeField] Image fillImage;
    [SerializeField] Color colorMax;
    [SerializeField] Color colorMin;

    public void SetMaxValue(float maxValue)
    {
        slider.maxValue = maxValue;
        fillImage.color = Color.Lerp(colorMin, colorMax, Mathf.PingPong(slider.value / slider.maxValue, 1));

    }
    public void SetValue(float value)
    {
        slider.value = value;
        fillImage.color = Color.Lerp(colorMin, colorMax, Mathf.PingPong(slider.value / slider.maxValue, 1));
    }
}
