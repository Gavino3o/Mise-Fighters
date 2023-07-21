using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class RebindControls : View
{
    [SerializeField] private Button backButton;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private KeyRebinder[] keyRebinders;

    void Start()
    {
        backButton.onClick.AddListener(() => {
            SaveKeybinds();
            OfflineUIManager.LocalInstance.Show<MainMenu>();
        });

        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);

        if (string.IsNullOrEmpty(rebinds)) return;

        playerInput.actions.LoadBindingOverridesFromJson(rebinds);
    }

    public void SaveKeybinds()
    {
        string rebinds = playerInput.actions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString("rebinds", rebinds);

    }

    private bool RebindingInProgress()
    {
        foreach (KeyRebinder rebinder in keyRebinders)
        {
            if (rebinder.inProgress) return true;
        }
        return false;
    }

    void Update()
    {
        bool value = !RebindingInProgress();
        backButton.interactable = value;
    }
   

}
