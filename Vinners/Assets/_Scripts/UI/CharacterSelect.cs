using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FishNet;

public class CharacterSelect : View
{
    
    [SerializeField] private TextMeshProUGUI lockInButtonText;
    [SerializeField] private Button nextCharacterButton;
    [SerializeField] private Button prevCharacterButton;
    [SerializeField] private Button lockInButton;

    [SerializeField] private Button startGameButton;

    [SerializeField] private Image characterDisplayPanel;
    /* figure out how to get the character sprites from this later
     * [SerializeField] List<Character> characterList;
     * OR
     * we can have character splash arts
     * [SerializeField] List<Image> characterSprites;
     */

    //temp
    [SerializeField] private List<GameObject> characterList;

    private int currCharacterIndex;

    public override void Initialise()
    {
        nextCharacterButton.onClick.AddListener(() =>
        {
            currCharacterIndex = (currCharacterIndex + 1) % characterList.Count;
            // characterDisplayPanel = characterList[currCharacterIndex].characterSprite
        });

        prevCharacterButton.onClick.AddListener(() =>
        {
            // math here might be wrong
            currCharacterIndex = (currCharacterIndex + characterList.Count - 1) % characterList.Count;
            // characterDisplayPanel = characterList[currCharacterIndex].characterSprite
        });
        lockInButton.onClick.AddListener(() => 
        {
            Player.LocalInstance.ServerSetLockIn(!Player.LocalInstance.isLockedIn);
            // assign the currently hovered character to the player
            Player.LocalInstance.ChooseCharacter(characterList[currCharacterIndex]);
            
                     
        });


        if (InstanceFinder.IsHost)
        {
            startGameButton.onClick.AddListener(() => {
                GameManager.Instance.StartGame();
            });

            startGameButton.gameObject.SetActive(true);
        } else
        {
            startGameButton.gameObject.SetActive(false);
        }

        base.Initialise();
        currCharacterIndex = 0;

    }

    private void Update()
    {
        if (!Initialised) return;

        lockInButtonText.color = Player.LocalInstance.isLockedIn ? Color.green : Color.red;
        startGameButton.interactable = GameManager.Instance.canStart;
    }





}
