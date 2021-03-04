using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackElement { FIRE, VOID, HEAVEN, NORMAL, ICE  }

[System.Serializable]
public class Attacks
{
    public string attackName;
    public int cost;
    public int damage;
    public AttackElement element;
    public int posMove;
    public bool isNoDamage;
    public bool targetsEnemy;
    public bool targetsFriendly; //Healing and Frozen allies
    public bool targetsAnyPos;
    public bool adjAttack;   //Alchemist basic attack for example
    public bool targetsAll;   //Flamethrower
    public Unit target;
    public List<int> targetPositions = new List<int>();
    public List<Unit> targetList = new List<Unit>();

    public virtual void Action()
    {

    }
};



public class KeyLunge : Attacks
{

    public KeyLunge()
    {
        attackName = "Key Lunge";
        cost = 2;
        damage = 1;
        element = AttackElement.NORMAL;
        posMove = 1;
        isNoDamage = false;
        targetsEnemy = true;
        targetsFriendly = false;
    }
}

public class Firebrand : Attacks
{

    public Firebrand()
    {
        attackName = "Firebrand";
        cost = 3;
        damage = -2;
        element = AttackElement.FIRE;
        posMove = 0;
        isNoDamage = false;
    }
}

public class FrontLine : Attacks
{

    public FrontLine()
    {
        attackName = "Front Line";
        cost = 2;
        damage = 0;
        element = AttackElement.NORMAL;
        posMove = 1;
        isNoDamage = true;
    }
}


public class PoisonDart : Attacks
{
    public PoisonDart()
    {
        attackName = "PoisonDart";
        cost = 2;
        damage = 0;
        element = AttackElement.VOID;
        posMove = 0;
        isNoDamage = true;
        targetsFriendly = false;
        targetsEnemy = true;
    }

    public override void Action()
    {
        Debug.Log(target.unitName);
        Poison poison = new Poison(target, 3);
        target.AddStatusEffect(poison);
    }
}

public class Flamethrower : Attacks
{
    public Flamethrower(List<Unit> targetList)
    {
        attackName = "Flamethrower";
        cost = 0;
        damage = -3;
        element = AttackElement.FIRE;
        posMove = 0;
        isNoDamage = false;
        targetsFriendly = false;
        targetsEnemy = false;
        targetsAll = true;

        this.targetList = targetList;
    }

    public override void Action()
    {
        if(targetList.Count >= 2) 
        {
            Unit unit = targetList[0];
            Unit unit2 = targetList[1];
            Burn burn = new Burn(unit, 3);
            Burn burn2 = new Burn(unit2, 3);
            unit.AddStatusEffect(burn);
            unit2.AddStatusEffect(burn2);
        }
        else
        {
            Unit unit = targetList[0];
            Burn burn = new Burn(unit, 3);
            unit.AddStatusEffect(burn);
        }
    }
}

