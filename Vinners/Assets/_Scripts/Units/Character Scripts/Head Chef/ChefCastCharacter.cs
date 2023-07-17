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
    [SerializeField] private AudioClip skillSpellSoundEffect;
    [SerializeField] private AudioClip dashSpellSoundEffect;
    [SerializeField] private AudioClip ultimateSpellSoundEffect;
    public void OnSkill()
    {
        if (!IsOwner) return;

        if (base.canCast[0])
        {
            StartCoroutine(Cooldown(0));
            characterAnimator.PlaySkill();
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
        Quaternion rotationOffset1 = Quaternion.Euler(0, 0, sliceAngle);
        GameObject firstSlice = Instantiate(sliceSpellPrefab, transform.position + transform.up, input.rotation * rotationOffset1);

        firstSlice.GetComponent<Lifetime>().lifetime = spellData[0].duration;
        SetupDamager(firstSlice.GetComponent<EnemyDamager>(), 0);
        ServerManager.Spawn(firstSlice);
        //ServerManager.Spawn(secondSlice);
        AudioManager.Instance.PlaySoundEffect(skillSpellSoundEffect);
        Debug.Log($"{spellData[0].spellName} casted");
    }

    #endregion

    #region Chef Blink
    [Header("Blink Skill")]

    public float blinkDistance = 10f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private GameObject stunPrefab;
    public void OnDash()
    {
        if (!IsOwner) return;
        if (canCast[1])
        {
            StartCoroutine(Cooldown(1));
            characterAnimator.PlayDash();
            StartCoroutine(Blink());
            CastBlinkSkill();
            Debug.Log($"{spellData[1].spellName} casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }


    [ServerRpc]
    public void CastBlinkSkill()
    {
        GameObject stunCircle = Instantiate(stunPrefab, transform.position, Quaternion.identity);
        SetupDamager(stunCircle.GetComponent<EnemyDamager>(), 1);
        stunCircle.GetComponent<Lifetime>().lifetime = spellData[1].duration;
        ServerManager.Spawn(stunCircle);
        AudioManager.Instance.PlaySoundEffect(dashSpellSoundEffect);
    }

    public IEnumerator Blink()
    {
        Vector2 blinkDirection = input.targetDirection;
        
        RaycastHit2D hit = Physics2D.Raycast((Vector2) transform.position, blinkDirection, blinkDistance, obstacleLayer);
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
                var shortenedBlinkDistance = obstacleDistance - 1f;
                if (shortenedBlinkDistance <= 0f)
                {
                    shortenedBlinkDistance = 0f;
                } 
                Vector2 newPosition = (Vector2) transform.position + blinkDirection * (shortenedBlinkDistance);
                transform.position = newPosition;
                Debug.Log(obstacleDistance.ToString());
                Debug.Log("C3");
            }
        }
        yield return new WaitForSeconds(spellData[1].duration);
        movement.interrupted = false;
    }
    #endregion

    #region Ultimate skill

    [Header("Ultimate Skill")]
    [SerializeField] private NetworkObject julienneSpellPrefab;
    public void OnUltimate()
    {
        if (!IsOwner) return;
        if (canCast[2])
        {
            StartCoroutine(Cooldown(1));
            characterAnimator.PlayUltimate(spellData[2].duration);
            StartCoroutine(Julienne());
            SpendUltimate(ULT_METER);
        }
        else
        {
            Debug.Log("Not enough charge");
        }
    }

    [ServerRpc]
    public void CastUltimateSkill()
    {
        Vector3 direction = new Vector3(input.targetDirection.x, input.targetDirection.y, 0);
        NetworkObject obj = Instantiate(julienneSpellPrefab, transform.position + direction * 5f, input.rotation);
        SetupDamager(obj.GetComponent<EnemyDamager>(), 2);
        obj.GetComponent<Lifetime>().lifetime = spellData[2].duration;
        ServerManager.Spawn(obj);
        AudioManager.Instance.PlaySoundEffect(ultimateSpellSoundEffect);
        Debug.Log($"{spellData[2].spellName} casted");
    }

    public IEnumerator Julienne()
    {
        movement.interrupted = true;
        character.isInvicible = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        CastUltimateSkill();
        yield return new WaitForSeconds(spellData[2].duration);
        movement.interrupted = false;
        character.isInvicible = false;
    }

    #endregion

}
