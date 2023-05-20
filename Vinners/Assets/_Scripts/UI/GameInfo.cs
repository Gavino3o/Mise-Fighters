using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Displays and updates information relevant to the main gameplay
 */
public class GameInfo : View
{
    /*
     * TODO: (Non-exhaustive non-final list of things that should be included)
     * Health
     * Teammates Health
     * Skill cooldowns
     * Status Effects
     * Current Stage
     * Current Wave
     */
    [SerializeField] private TextMeshProUGUI HP;

    private void Update()
    {
        if (!Initialised) return;

        Player player = Player.LocalInstance;

        if (player == null || player.controlledCharacter == null) return;
       
        HP.text = $"HP: {player.controlledCharacter.currHealth}";
    }
}
