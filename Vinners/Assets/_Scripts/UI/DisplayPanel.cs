using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayPanel : MonoBehaviour
{

    public Image splash;
    public TextMeshProUGUI skillDesc;
    public TextMeshProUGUI dashDesc;
    public TextMeshProUGUI ultDesc;

    public Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(() => Hide());
    }


    public void Show(Character character)
    {
        splash.sprite = character.characterSplash;
        
        SpellData[] spldata = character.caster.spellData;
        skillDesc.text = spldata[0].description;
        dashDesc.text = spldata[1].description;
        ultDesc.text = spldata[2].description;

    }

    public void Show(EnemyAI enemy)
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
