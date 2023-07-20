using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuideMenu : View
{
    public Character[] characters;
    public EnemyAI[] enemies;
    
    [SerializeField] private Button[] characterButtons;
    [SerializeField] private Button[] enemyButtons;
    [SerializeField] private Button backButton;

    public DisplayPanel display;

    void Start()
    {
        SetupCharacters();
        SetupEnemies();
        backButton.onClick.AddListener(() => {
            display.gameObject.SetActive(false);
            OfflineUIManager.LocalInstance.Show<MainMenu>();
        });
        display.gameObject.SetActive(false);
    }

    private void SetupCharacters()
    {
        characterButtons[0].onClick.AddListener(() => OpenDisplay(characters[0]));
        characterButtons[1].onClick.AddListener(() => OpenDisplay(characters[1]));
        characterButtons[2].onClick.AddListener(() => OpenDisplay(characters[2]));
        characterButtons[3].onClick.AddListener(() => OpenDisplay(characters[3]));
    }

    private void OpenDisplay(Character character)
    {
        display.gameObject.SetActive(true);
        display.Show(character);
    }

    private void SetupEnemies()
    {

    }

}
