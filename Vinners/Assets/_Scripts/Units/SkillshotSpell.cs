using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillshotSpell : Spell
{
    [SerializeField] private GameObject projectile;

    public override void Cast(Vector2 mousePos)
    {
        if (canCast)
        {
            GameObject obj = Instantiate(projectile, transform.position, transform.rotation);
            Vector2 targetDir = mousePos - new Vector2(transform.position.x, transform.position.y);
            obj.GetComponent<SkillshotMotion>().SetDirection(targetDir);
            ServerManager.Spawn(obj);
            canCast = false;
            lastCast = Time.time;
            Debug.Log("Spell casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }
}
