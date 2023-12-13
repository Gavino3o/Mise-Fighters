using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayHealth : MonoBehaviour
{
    public Unit unit;
    [SerializeField] private TextMeshProUGUI HP;

    private void Update()
    {
        HP.text = unit.currHealth.ToString();
    }
}
