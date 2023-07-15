using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;

public class HealthBar : NetworkBehaviour
{
    public Slider slider;
    
    public void Setup(float max, float curr)
    {
        slider.maxValue = max;
        slider.value = curr;
    }

    public void SetHP(float health)
    {
        slider.value = health;
    } 
}
