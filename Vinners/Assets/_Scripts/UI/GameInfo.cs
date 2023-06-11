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
    [SerializeField] private TextMeshProUGUI SkillReady;
    [SerializeField] private TextMeshProUGUI DashReady;
    [SerializeField] private TextMeshProUGUI UltiReady;

    private Player player;
    private Character character;

    private void Awake()
    {
        player = Player.LocalInstance;
        character = player.controlledCharacter;
    }

    private void Update()
    {
        if (!Initialised) return;

        if (player == null || player.controlledCharacter == null) return;
       
        HP.text = $"HP: {player.controlledCharacter.currHealth}";
        SkillReady.text = $"Skill Ready: {player.controlledCharacter.caster.canCast[0]}";
        DashReady.text = $"Dash Ready: {player.controlledCharacter.caster.canCast[1]}";
        UpdateUltText();
       
    }

    private void UpdateUltText()
    {
       UltiReady.text = $"Charge: {player.controlledCharacter.caster.ultimate}/50 Ulti Ready:{player.controlledCharacter.caster.canCast[2]}";
    }
}
