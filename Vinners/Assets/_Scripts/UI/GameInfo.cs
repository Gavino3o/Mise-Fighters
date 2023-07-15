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
    [SerializeField] private HealthBar HP;
    [SerializeField] private BubbleIndicator SkillReady;
    [SerializeField] private BubbleIndicator DashReady;
    [SerializeField] private TextMeshProUGUI UltiReady;

    private Player player;
    private Character character;

    private void Awake()
    {
        player = Player.LocalInstance;
        character = player.controlledCharacter;

    }
    private void Start()
    {
        HP.Setup(player.controlledCharacter.baseStats.maxHealth, player.controlledCharacter.currHealth);
        SetupUI();
    }

    private void Update()
    {
        if (!Initialised) return;

        if (player == null || player.controlledCharacter == null) return;

        HP.SetHP(player.controlledCharacter.currHealth);
        // UpdateHealth(player.controlledCharacter.currHealth);
        UpdateSkill();
        UpdateDash();
        UpdateUltText();
       
    }

    private void SetupUI()
    {
        
        SkillReady.SetLabel("SKILL");
        DashReady.SetLabel("DASH");

    }

    private void UpdateHealth(float hp)
    {
        HP.SetHP(hp);
    } 

    private void UpdateSkill()
    {
        SkillReady.Check(player.controlledCharacter.caster.canCast[0]);

    }

    private void UpdateDash()
    {
        DashReady.Check(player.controlledCharacter.caster.canCast[1]);

    }
    private void UpdateUltText()
    {
       UltiReady.text = $"Charge: {player.controlledCharacter.caster.ultimate}/50 Ulti Ready:{player.controlledCharacter.caster.canCast[2]}";
    }
}
