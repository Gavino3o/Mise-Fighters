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
public class Spell : NetworkBehaviour
{

    [SerializeField] private float cooldown;
    [SerializeField] private GameObject projectile;
    public bool canCast;
    private float lastCast;

    private void Update()
    {
        if (Time.time - lastCast < cooldown)
        {
            return;
        }
        canCast = true;
    }

    public void Cast(Vector2 castDirection)
    {
        if (canCast)
        {
            GameObject obj = Instantiate(projectile, transform.position, transform.rotation);
            obj.GetComponent<CharacterDamager>().SetDirection(castDirection);
            ServerManager.Spawn(obj);
            canCast = false;
            lastCast = Time.time;
            Debug.Log("Spell casted");
        } else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    public void CastOn(GameObject obj)
    {
        // to implement (for movement spell/ targetted spell  maybe?)
    }
}
