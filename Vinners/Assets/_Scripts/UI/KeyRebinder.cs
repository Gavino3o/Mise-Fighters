using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class KeyRebinder : MonoBehaviour
{
    [SerializeField] private InputActionReference refAction;

    [SerializeField] TMP_Text bindingDisplayNameText;
    [SerializeField] public Button startRebindObject;
    [SerializeField] private GameObject awaitingInputObject;
    [SerializeField] private TMP_Text awaitingInputText;

    public bool inProgress;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public event Action BindingStarted;
    public event Action BindingEnded;

    private void Start()
    {
        startRebindObject.onClick.AddListener(() => StartRebinding());

        UpdateDisplayText();
    }

    public void StartRebinding()
    {
        BindingStarted?.Invoke();


        inProgress = true;
        var action = refAction.action;

        if (action.bindings[0].isComposite)
        {

            var firstPartIndex = 1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                PerformInteractiveRebind(action, firstPartIndex, allCompositeParts: true);

        } else
        {
            PerformInteractiveRebind(action, 0);
        }

    }


    private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
    {
        rebindingOperation = action.PerformInteractiveRebinding(bindingIndex)
                .WithControlsExcluding("Mouse")
                .OnComplete(operation => {
                    RebindComplete();
                    UpdateDisplayText();
                    if (allCompositeParts)
                    {
                        var nextBindingIndex = bindingIndex + 1;
                        if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                            PerformInteractiveRebind(action, nextBindingIndex, true);

                    } 

                });

        var partName = default(string);
        if (action.bindings[bindingIndex].isPartOfComposite)
            partName = $"Binding '{action.bindings[bindingIndex].name}.'";
        startRebindObject.gameObject.SetActive(false);
        awaitingInputObject.SetActive(true);

        var text = $"{partName} Awaiting input...";

        awaitingInputText.text = text;

        rebindingOperation.Start();
    }

    private void RebindComplete()
    {
        rebindingOperation.Dispose();
        startRebindObject.gameObject.SetActive(true);
        awaitingInputObject.SetActive(false);
        inProgress = false;

        BindingEnded?.Invoke();
    }

    private void UpdateDisplayText()
    {

        bindingDisplayNameText.text = refAction.action.GetBindingDisplayString(0);
    }
}
