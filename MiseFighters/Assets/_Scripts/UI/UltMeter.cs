using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UltMeter : MonoBehaviour
{
    public Slider slider;
    public Image img;
    public TextMeshProUGUI label;
    public TextMeshProUGUI charge;


    public void Setup(float max, float curr)
    {
        slider.maxValue = max;
        SetMeter(curr);

    }

    public void SetLabel(string str)
    {
        label.text = str;
    }

    public void SetMeter(float value)
    {
        slider.value = value;
        if (slider.value >= slider.maxValue)
        {
            img.color = Color.white;
            charge.text = "READY";
        } else
        {
            img.color = Color.grey;
            charge.text = value.ToString();
        }
    }
}
