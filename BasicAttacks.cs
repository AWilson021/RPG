using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasicAttacks : Attacks
{
    
}

public class KeyThrust : BasicAttacks
{

    public KeyThrust()
    {
        attackName = "Key Thrust";
        cost = 0;
        damage = 0;
        element = AttackElement.NORMAL;
        posMove = 0;
        isNoDamage = false;
        targetsEnemy = true;
        targetsFriendly = false;
        targetPositions.Add(1);
    }
}

public class BroadSwing : BasicAttacks
{
    public BroadSwing()
    {
        attackName = "Broad Swing";
        cost = 0;
        damage = 0;
        element = AttackElement.NORMAL;
        posMove = 0;
        isNoDamage = false;
        targetsEnemy = true;
        targetsFriendly = false;
        targetPositions.Add(1);
        targetPositions.Add(2);
    }
}

public class WhipCrack : BasicAttacks
{
    public WhipCrack()
    {
        attackName = "Whip Crack";
        cost = 0;
        damage = 0;
        element = AttackElement.NORMAL;
        posMove = 0;
        isNoDamage = false;
        targetsEnemy = true;
        targetsFriendly = false;
        targetPositions.Add(2);
        targetPositions.Add(3);
        targetPositions.Add(4);
    }
}

public class Concoction : BasicAttacks
{
    public Concoction()
    {
        attackName = "Concotion";
        cost = 0;
        damage = 0;
        element = AttackElement.NORMAL;
        posMove = 0;
        isNoDamage = false;
        targetsEnemy = true;
        targetsFriendly = false;
        targetsAnyPos = true;
        adjAttack = true;
    }
}
