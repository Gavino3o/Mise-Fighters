using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FishNet;

/*
 * Contains all functionalities related to the character select lobby. 
 */
public class CharacterSelect : View
{
    
    [SerializeField] private Button nextCharacterButton;
    [SerializeField] private Button prevCharacterButton;
    [SerializeField] private Button lockInButton;

    [SerializeField] private Button startGameButton;

    [SerializeField] private Image characterDisplayPanel;
    [SerializeField] private Button leaveButton;

    [SerializeField] private List<GameObject> characterList;
    private int currCharacterIndex;

    public override void Initialise()
    {
        currCharacterIndex = 0;
        DisplayCurrentCharacter();
        
        nextCharacterButton.onClick.AddListener(() =>
        {
            currCharacterIndex = (currCharacterIndex + 1) % characterList.Count;
            DisplayCurrentCharacter();
            Player.LocalInstance.ServerChooseCharacter(characterList[currCharacterIndex]);
        });

        prevCharacterButton.onClick.AddListener(() =>
        { 
            currCharacterIndex = (currCharacterIndex + characterList.Count - 1) % characterList.Count;
            DisplayCurrentCharacter();
            Player.LocalInstance.ServerChooseCharacter(characterList[currCharacterIndex]);
        });

        /*
         * Assigns the currently hovered character to the player and locks in for the player.
         */
        lockInButton.onClick.AddListener(() =>
        {
            Player.LocalInstance.ServerSetLockIn(!Player.LocalInstance.isLockedIn);

        });
        
        /*
         * Only the Host should have access to the start button
         */
        if (InstanceFinder.IsHost)
        {
            if (startGameButton != null) startGameButton.gameObject.SetActive(true);

            startGameButton.onClick.AddListener(() => {
                GameManager.Instance.StartGame();
            });

            leaveButton.onClick.AddListener(() =>
            {
                InstanceFinder.ServerManager.StopConnection(false);
                InstanceFinder.ClientManager.StopConnection();
               
            });

        } else
        {
            if (startGameButton != null) startGameButton.gameObject.SetActive(false);

            leaveButton.onClick.AddListener(() => {
                InstanceFinder.ClientManager.StopConnection();
            });
            
        }

        base.Initialise();
        currCharacterIndex = 0;

    }

    private void Update()
    {
        if (!Initialised) return;

        lockInButton.image.color = Player.LocalInstance.isLockedIn ? Color.grey : Color.white;

        // Should only be able to start game if all players in the lobby are ready.
        startGameButton.interactable = GameManager.Instance.canStart;
    }

    private void DisplayCurrentCharacter()
    {
        characterDisplayPanel.sprite = characterList[currCharacterIndex].GetComponent<Character>().characterSplash;
    }



}
