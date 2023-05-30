using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class ChefCastCharacter : CastCharacter
{
    #region Slice skill
    [Header("Slice skill")]
    [SerializeField] private GameObject sliceSpellPrefab;

    public void onSkill()
    {
        if (!IsOwner) return;
    }

    [ServerRpc]
    public void CastSliceSkill()
    {
        //Hardcoded everything for now, decide injection later.
        //Consider using same slice for both slices.
        Vector3 positionOffset1 = Vector3.zero;
        Vector3 positionOffset2 = Vector3.zero;
        Quaternion rotationOffset1 = Quaternion.identity; //120 degrees
        Quaternion rotationOffset2 = Quaternion.identity;

        GameObject slice1 = Instantiate(sliceSpellPrefab, transform.position + positionOffset1, transform.rotation * rotationOffset1);
        GameObject slice2 = Instantiate(sliceSpellPrefab, transform.position + positionOffset2, transform.rotation * rotationOffset2);

        slice1.GetComponent<Lifetime>().lifetime = spellData[0].duration;
        slice2.GetComponent<Lifetime>().lifetime = spellData[0].duration;
        slice1.GetComponent<EnemyDamager>().damage = spellData[0].damage * character.currAttack;
        slice2.GetComponent<EnemyDamager>().damage = spellData[0].damage * character.currAttack;

        ServerManager.Spawn(slice1);
        ServerManager.Spawn(slice2);
        Debug.Log($"{spellData[0].spellName} casted");
    }

    #endregion
}
