using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects
{

    public string name;
    public Unit targetUnit;
    public int turns;
    public bool isDone;

    public StatusEffects(Unit targetUnit, int turns)
    {
        this.targetUnit = targetUnit;
        this.turns = turns;

        this.isDone = false;
    }

    public virtual void EndOfTurn()
    {
        
    }


    public virtual void EndOfAction()
    {

    }

    public void DecrementTurns()
    {
        turns--;

        if (turns <= 0)
        {
            this.isDone = true;
        }
    }
}

public class Poison : StatusEffects
{
    public Poison(Unit targetUnit, int turns) 
        : base(targetUnit, turns)
    {
        this.name = "Poison";

        Debug.Log(targetUnit.name + " has had the Poison status effect applied to them for " + turns + " turns!");
    }

    public override void EndOfTurn()
    {
        targetUnit.TakeDamage(1);

        Debug.Log(targetUnit.unitName + " has been dealt 1 damage due to poison!");

        DecrementTurns();
    }
}



public class Burn : StatusEffects
{
    public Burn(Unit targetUnit, int turns)
    : base(targetUnit, turns)
    {
        this.name = "Burn";

        Debug.Log(targetUnit.name + " has had the Burn status effect applied to them for " + turns + " turns!");
    }

    public override void EndOfAction()
    {
        targetUnit.TakeDamage(1);
    }

    public override void EndOfTurn()
    {
        targetUnit.TakeDamage(1);

        DecrementTurns();
    }
}

