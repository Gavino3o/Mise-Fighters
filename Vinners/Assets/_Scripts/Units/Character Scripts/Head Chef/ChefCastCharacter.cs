using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class ChefCastCharacter : CastCharacter
{
    #region Slice skill
    [Header("Slice skill")]
    [SerializeField] private GameObject sliceSpellPrefab;
    [SerializeField] private float sliceAngle = 30;
    public void OnSkill()
    {
        if (!IsOwner) return;

        if (base.canCast[0])
        {
            StartCoroutine(Cooldown(0));
            CastSliceSkill();
            Debug.Log("Spell casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    [ServerRpc]
    public void CastSliceSkill()
    {
        // Consider implementing using one slice and using animation destroy.
        Quaternion rotationOffset1 = Quaternion.Euler(0, 0, sliceAngle);
        Quaternion rotationOffset2 = Quaternion.Euler(0, 0 , -sliceAngle);
        GameObject firstSlice = Instantiate(sliceSpellPrefab, transform.position + transform.up, transform.rotation * rotationOffset1);
        GameObject secondSlice = Instantiate(sliceSpellPrefab, transform.position + transform.up, transform.rotation * rotationOffset2);

        firstSlice.GetComponent<Lifetime>().lifetime = spellData[0].duration;
        secondSlice.GetComponent<Lifetime>().lifetime = spellData[0].duration;
        firstSlice.GetComponent<EnemyDamager>().damage = spellData[0].damage * character.currAttack;
        secondSlice.GetComponent<EnemyDamager>().damage = spellData[0].damage * character.currAttack;
        ServerManager.Spawn(firstSlice);
        ServerManager.Spawn(secondSlice);
        Debug.Log($"{spellData[0].spellName} casted");
    }

    #endregion

    #region Chef Blink
    [Header("Blink Skill")]

    public float blinkDistance = 10f;
    public void OnDash()
    {
        if (!IsOwner) return;
        if (canCast[1])
        {
            StartCoroutine(Cooldown(1));
            StartCoroutine(Blink());
            Debug.Log($"{spellData[1].spellName} casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    public IEnumerator Blink()
    {
        Vector2 blinkDirection = input.targetDirection;
        Vector2 rayOffset = blinkDirection * 1.5f; //Origin of ray cannot be within a collider.

        // TODO: Add layering for obstacle layer.
        RaycastHit2D hit = Physics2D.Raycast((Vector2) transform.position + rayOffset, blinkDirection);
        movement.interrupted = true;
        if (hit.collider == null)
        {
            Vector2 newPosition = (Vector2) transform.position + blinkDirection * blinkDistance;
            transform.position = newPosition;
            Debug.Log("C1");
        } 
        else
        {
            float obstacleDistance = hit.distance;
            if (obstacleDistance > blinkDistance)
            {
                Vector2 newPosition = (Vector2) transform.position + blinkDirection * blinkDistance;
                transform.position = newPosition;
                Debug.Log("C2");
            }
            else
            {
                Vector2 newPosition = (Vector2) transform.position + blinkDirection * (obstacleDistance - 1f);
                transform.position = newPosition;
                Debug.Log(obstacleDistance.ToString());
                Debug.Log("C3");
            }
        }
        yield return new WaitForSeconds(spellData[1].duration);
        movement.interrupted = false;
    }
    #endregion
}