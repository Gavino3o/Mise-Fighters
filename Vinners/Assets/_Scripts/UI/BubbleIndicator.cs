using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BubbleIndicator : MonoBehaviour
{
    public Image fill;
    public TextMeshProUGUI label;

    public void Check(bool value)
    {
        if (value) 
        {
            Ready();
        } else
        {
            Unready();
        }
    }
    public void Ready()
    {
        fill.color = Color.white;
    }

    public void Unready()
    {
        fill.color = Color.grey;
    }

    public void SetLabel(string str)
    {
        label.text = str;
    }
}
