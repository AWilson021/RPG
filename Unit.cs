using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public string unitName;
    public int unitLvl;

    public int damage;

    public int maxHP;
    public int currHP;

    public int pos;
    public int currExhaustion;
    public int maxExhaustion = 3;

    public bool attacked = false;

    public bool isDead;

    public List<StatusEffects> statusEffects = new List<StatusEffects>();

    [SerializeField]
    public List<Attacks> attackList = new List<Attacks>();

    [SerializeField]
    public BasicAttacks basicAttack;

    public void TakeDamage(int dmg)
    {
        currHP -= dmg;

        if (currHP <= 0)
            isDead = true;

    }

    public void Rest(int amount)
    {
        currHP += amount;
        if (currHP >= maxHP)
            currHP = maxHP;
    }

    public void addAttack(Attacks attack)
    {
        attackList.Add(attack);
    }

    public void AddStatusEffect(StatusEffects effect)
    {
        statusEffects.Add(effect);
    }

    public void UpdateStatusEffects()
    {
        if (statusEffects.Count == 0)
            return;

        for(int i = 0; i < statusEffects.Count; i++)
        {
            StatusEffects effect = statusEffects[i];

            Debug.Log(effect.name);

            effect.EndOfAction();
            effect.EndOfTurn();

            if(effect.isDone)
            {
                statusEffects.RemoveAt(i);
                i--;
            }
        }
    }
}
