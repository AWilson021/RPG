using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{

    public Text nameText;
    public Text levelText;
    public Text hpValueText;
    public Slider hpSlider;

    public Text spText;
    public Text actionText;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLvl;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currHP;
        hpValueText.text = unit.currHP + "/" + unit.maxHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
        hpValueText.text = hpSlider.value + "/" + hpSlider.maxValue;
    }

    public void SetSP(int sp)
    {
        spText.text = "SP: " + sp.ToString();
    }

    public void SetActionText(int action)
    {
        actionText.text = "Actions: " + action.ToString();
    }
}
