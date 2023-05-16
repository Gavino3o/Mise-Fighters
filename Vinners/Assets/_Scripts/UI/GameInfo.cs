using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private void Update()
    {
        if (!Initialised) return;
    }

    public override void Initialise()
    {
        // Debug.Log("UI View changed to Game Info");
        base.Initialise();
    }
}
