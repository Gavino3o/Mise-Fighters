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

    private void Awake()
    {
        foreach (KeyRebinder rebinder in keyRebinders)
        {
            rebinder.BindingStarted += RebindingInProgress;
            rebinder.BindingEnded += RebindingFinished;
        }
    }

    private void OnDestroy()
    {
        foreach (KeyRebinder rebinder in keyRebinders)
        {
            rebinder.BindingStarted -= RebindingInProgress;
            rebinder.BindingEnded -= RebindingFinished;
        }
    }

    public void SaveKeybinds()
    {
        string rebinds = playerInput.actions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString("rebinds", rebinds);

    }

    private void RebindingInProgress()
    {
        UpdateRebinders(false);
    }

    private void RebindingFinished()
    {
        UpdateRebinders(true);
    }


    private void UpdateRebinders(bool value)
    {
        foreach (KeyRebinder rebinder in keyRebinders)
        {
            rebinder.startRebindObject.interactable = value;
        }
        backButton.interactable = value;
    }


}
