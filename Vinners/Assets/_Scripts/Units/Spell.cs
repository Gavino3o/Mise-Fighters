using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The spell is responsible for keeping track of its own cooldown and casting itself. The characters/units are
 * responsible for invoking the cast.
 * 
 * TODO: Refactor this class! Spell should be an abstract class with promise that it can be casted.
 * AreaSpell and SkillShotSpell will inherit from this class (behaviour of Cast() is different from both)
 * 
 * Ultimates should be separate classes entirely.
 */
public abstract class Spell : NetworkBehaviour
{

    [SerializeField] private float cooldown;
    public bool canCast;
    protected float lastCast;

    private void Start()
    {
        canCast = true;
    }

    private void Update()
    {
        if (Time.time - lastCast < cooldown)
        {
            return;
        }
        canCast = true;
    }

    // refactor to public void Cast(Character character)
    public abstract void Cast(Vector2 v);
    
}
