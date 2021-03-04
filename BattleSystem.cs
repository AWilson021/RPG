using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, TARGETSELECT }

public class BattleSystem : MonoBehaviour {


    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject playerPrefab4;

    public GameObject enemyPrefab;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject enemyPrefab4;

    public Canvas canvas;


    [SerializeField]
    public List<Transform> playerBattleStations;

    [SerializeField]
    public List<Button> playerClickButtons;

    [SerializeField]
    public List<Button> enemyClickButtons;

    [SerializeField]
    public List<Transform> enemyBattleStations;


    Unit enemyUnit;


    public Image enemySelectImage;
    public Text enemySelectText;


    public Text move1Button;
    public Text move2Button;
    public Text move3Button;

    public Text attackDmgText;
    public Text attackCostText;
    public Text attackElmText;


    public BattleHUD playerHUD;
    

    public BattleHUD enemyHUD;
    public BattleHUD enemy2HUD;
    public BattleHUD enemy3HUD;
    public BattleHUD enemy4HUD;

    public Image dialogueMenu;
    public Image skillMenu;
    public Image skillInfo;

    public Text dialogueText;

    int playerSelect;
    int prevPlayer;

    int enemySel;

    public List<Unit> playerUnitsList;
    public GameObject[] playerGOList;
    public List<Animation> playerAnimsList;

    public List<Unit> enemyUnitsList;
    public GameObject[] enemyGOList;
    public List<Animation> enemyAnimsList;

    float speed;

    Attacks attack;
    Unit unitSelected;

    int moveNum;

    public int sp;
    public int actions;


    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        
        for(int i = 0; i < playerClickButtons.Count; i++)
        {
            playerClickButtons[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < enemyClickButtons.Count; i++)
        {
            enemyClickButtons[i].gameObject.SetActive(false);
        }


        /////////////////////////////////////////////////////////////////////////////////Players initialization
        playerGOList = new GameObject[4];

        playerGOList[0] = (Instantiate(playerPrefab));
        playerGOList[1] = (Instantiate(playerPrefab2));
        playerGOList[2] = (Instantiate(playerPrefab3));
        playerGOList[3] = (Instantiate(playerPrefab4));

        playerUnitsList = new List<Unit>();

        for(int i = 0; i < playerGOList.Length; i++)
        {
            playerUnitsList.Add(gameObject.AddComponent<Unit>());
            playerGOList[i].transform.GetChild(1).gameObject.SetActive(false);
        }


        for(int i = 0; i < playerGOList.Length; i++) //Sort playerUnitList based on position
        {
            Unit unit = playerGOList[i].GetComponent<Unit>();

            playerUnitsList[unit.pos - 1] = unit;
        }


        ////////////////////////////////////////////////////////////////////////////////////Enemies initialization


        enemyGOList = new GameObject[4];

        enemyGOList[0] = (Instantiate(enemyPrefab));
        enemyGOList[1] = (Instantiate(enemyPrefab2));
        enemyGOList[2] = (Instantiate(enemyPrefab3));
        enemyGOList[3] = (Instantiate(enemyPrefab4));

        enemyUnitsList = new List<Unit>();

        for(int i = 0; i < enemyGOList.Length; i++)
        {
            enemyUnitsList.Add(gameObject.AddComponent<Unit>());
            //enemyGOList[i].transform.GetChild(1).gameObject.SetActive(false);
        }

        for (int i = 0; i < enemyGOList.Length; i++) //Sort enemyUnitList based on position
        {
            Unit unit = enemyGOList[i].GetComponent<Unit>();

            enemyUnitsList[unit.pos - 1] = unit;
        }

        /////////////////////////////////////////////////////////////////////////////////////////Initialize HUD

        sp = 2;
        actions = playerUnitsList.Count;

        playerHUD.SetSP(sp);
        playerHUD.SetActionText(actions);



        playerSelect = 1;
        prevPlayer = 1;
        skillMenu.gameObject.SetActive(false);
        skillInfo.gameObject.SetActive(false);
        playerUnitsList[0].gameObject.transform.GetChild(1).gameObject.SetActive(true);

        state = BattleState.START;
        StartCoroutine(SetupBattle());

        for (int i = 0; i < playerClickButtons.Count; i++)
        {
            playerClickButtons[i].gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        float step = 20f * Time.deltaTime;
        for (int i = 0; i < playerUnitsList.Count; i++)
        {
            playerUnitsList[i].transform.position = Vector3.MoveTowards(playerUnitsList[i].transform.position, playerBattleStations[i].position, step);
            playerUnitsList[i].transform.SetParent(playerBattleStations[i].transform);
            playerUnitsList[i].pos = i + 1;
        }

        
    }

    IEnumerator SetupBattle()
    {

        dialogueText.text = "Choose an action for " + playerUnitsList[0].unitName + ":";

        for (int i = 0; i < playerUnitsList.Count; i++)
        {


            playerUnitsList[i].transform.position = playerBattleStations[i].position;
            playerUnitsList[i].transform.SetParent(playerBattleStations[i].transform);


            playerAnimsList.Add(playerUnitsList[i].GetComponent<Animation>());

            playerUnitsList[i].addAttack(new KeyLunge());  
            playerUnitsList[i].addAttack(new FrontLine());
            playerUnitsList[i].addAttack(new Flamethrower(enemyUnitsList));

            playerUnitsList[i].basicAttack = new Concoction();
            
        }

        Player1Click();


        ////////////////////////////////////////////////////////////////Enemy setup


        for (int i = 0; i < enemyUnitsList.Count; i++)
        {


            enemyUnitsList[i].transform.position = enemyBattleStations[i].position;
            enemyUnitsList[i].transform.SetParent(enemyBattleStations[i].transform);


            enemyAnimsList.Add(enemyUnitsList[i].GetComponent<Animation>());
        }

        /////////////////////////////////////////////////////////////////Player HUD setup

        playerHUD.SetHUD(playerUnitsList[0]);
        

        //////////////////////////////////////////////////////////////////Enemy HUD setup

  
        enemyHUD.SetHUD(enemyUnitsList[0]);
        enemy2HUD.SetHUD(enemyUnitsList[1]);
        enemy3HUD.SetHUD(enemyUnitsList[2]);
        enemy4HUD.SetHUD(enemyUnitsList[3]);


        dialogueText.text = "Choose an action for " + playerUnitsList[0].unitName + ":";

        state = BattleState.PLAYERTURN;
        
        yield return new WaitForSeconds(1f);

    }

    public void AttackChosen()
    {
        unitSelected = playerUnitsList[playerSelect - 1];
        attack = unitSelected.attackList[moveNum - 1];

        if(attack.cost > sp)
        {
            StartCoroutine(SPMessage());
        }
        else if (attack.targetsEnemy)
        {
            EnemySelectMessage();
        }
        else
            StartCoroutine(PlayerAttack());
    }

    public void EnemySelectMessage()
    {
        for (int i = 0; i < enemyClickButtons.Count; i++)
        {
            enemyClickButtons[i].gameObject.SetActive(true);
        }
        skillMenu.gameObject.SetActive(false);
        enemySelectImage.gameObject.SetActive(true);
        enemySelectText.text = "Choose an enemy to attack";
    }


    IEnumerator PlayerAttack()
    {

        int maxPos = playerBattleStations.Count - 1;
        int damage;
        sp -= attack.cost;
        

        if (attack.isNoDamage)
            damage = 0;
        else
        {
            damage = attack.damage + unitSelected.damage;
            playerAnimsList[playerSelect - 1].Play();
        }

        yield return new WaitForSeconds(1f);

        attack.Action();

        if (attack.posMove != 0)
        {
            int currPos = unitSelected.pos - 1;
            int newPos = currPos - attack.posMove;

            if (newPos > playerBattleStations.Count)
            {
                playerUnitsList.RemoveAt(currPos);
                playerUnitsList.Insert(maxPos, unitSelected);
                playerSelect = 4;
            }
            else if (newPos < 0)
            {
                playerUnitsList.RemoveAt(currPos);
                playerUnitsList.Insert(0, unitSelected);
                playerSelect = 1;
            }
            else
            {
                playerUnitsList.RemoveAt(currPos);
                playerUnitsList.Insert(newPos, unitSelected);
                playerSelect = newPos + 1;
            }
        }


        skillMenu.gameObject.SetActive(false);
        skillInfo.gameObject.SetActive(false);
        enemySelectImage.gameObject.SetActive(false);
        dialogueMenu.gameObject.SetActive(true);

        for(int i = 0; i < enemyClickButtons.Count; i++)
        {
            enemyClickButtons[i].gameObject.SetActive(false);
        }

        if (attack.adjAttack) //Alchemist like attack
        {
            if(enemyUnitsList.Count == 1)
            {
                enemyUnitsList[enemySel - 1].TakeDamage(damage);
            }
            else if(enemySel != enemyUnitsList.Count)
            {
                enemyUnitsList[enemySel - 1].TakeDamage(damage);
                enemyUnitsList[enemySel].TakeDamage(damage);
            }
            else
            {
                enemyUnitsList[enemySel - 1].TakeDamage(damage);
                enemyUnitsList[enemySel - 2].TakeDamage(damage);
            }
        }
        else if (attack.targetsAll)
        {
            for(int i = 0; i < enemyUnitsList.Count; i++)
            {
                enemyUnitsList[i].TakeDamage(damage);
            }
        }
        else
        {
            enemyUnitsList[enemySel - 1].TakeDamage(damage);
        }
            

        enemyHUD.SetHP(enemyUnitsList[0].currHP);            //fix
        enemy2HUD.SetHP(enemyUnitsList[1].currHP);
        enemy3HUD.SetHP(enemyUnitsList[2].currHP);
        enemy4HUD.SetHP(enemyUnitsList[3].currHP);

        dialogueText.text = "Used skill " + attack.attackName;

        actions--;

        playerHUD.SetSP(sp);
        playerHUD.SetActionText(actions);

        if (unitSelected.attacked)
        {
            unitSelected.currExhaustion++;
            if(unitSelected.currExhaustion == unitSelected.maxExhaustion)
            {
                yield return new WaitForSeconds(2f);
                dialogueText.text = unitSelected.unitName + " fainted from exhaustion";
                playerUnitsList.RemoveAt(unitSelected.pos - 1);
                Destroy(unitSelected.gameObject);
            }
        }

        yield return new WaitForSeconds(1f);

        for(int i = 0; i < enemyUnitsList.Count; i++)
        {
            if (enemyUnitsList[i].isDead)
            {
                enemyUnitsList.RemoveAt(i);
                i--;
            }
        }
        

        if (enemyUnitsList.Count == 0)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else if(actions == 0)
        {
            dialogueText.text = "Out of actions.";

            for(int i = 0; i < playerUnitsList.Count; i++) //Reset units attacked
            {
                playerUnitsList[i].attacked = false;
            }

            state = BattleState.ENEMYTURN;
            HandleStatusEffects();
            StartCoroutine(EnemyTurn());
        }

        unitSelected.attacked = true;

        PlayerClick(playerSelect);
    }

    
    public void HandleStatusEffects()
    {
        foreach(Unit unit in playerUnitsList)
        {
            unit.UpdateStatusEffects();
        }

        foreach (Unit unit in enemyUnitsList)
        {
            unit.UpdateStatusEffects();
        }

        enemyHUD.SetHP(enemyUnitsList[0].currHP);
        enemy2HUD.SetHP(enemyUnitsList[1].currHP);
        enemy3HUD.SetHP(enemyUnitsList[2].currHP);
        enemy4HUD.SetHP(enemyUnitsList[3].currHP);
    }

    IEnumerator EnemyTurn() ///////////////Update
    {
        dialogueText.text = enemyUnitsList[0].unitName + " attacks!";

        enemyAnimsList[0].Play();

        yield return new WaitForSeconds(1f);

        playerUnitsList[0].TakeDamage(enemyUnitsList[0].damage); //Only attack pos 1

        playerHUD.SetHP(playerUnitsList[0].currHP);

        yield return new WaitForSeconds(1f);

        if (playerUnitsList[0].isDead)
        {
            playerUnitsList.RemoveAt(0); // Temp remove
        }
        else
        {
            state = BattleState.PLAYERTURN;
            HandleStatusEffects();
        }

        sp++;
        playerHUD.SetSP(sp);

        ResetDialogText();
    }

    void ResetDialogText()
    {

        actions = playerUnitsList.Count;
        playerHUD.SetActionText(actions);
        PlayerClick(playerSelect);
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
            dialogueText.text = "You won the battle!";
        else if (state == BattleState.LOST)
            dialogueText.text = "You were defeated.";
    }

    public void OnSkillButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        dialogueMenu.gameObject.SetActive(false);
        skillMenu.gameObject.SetActive(true);
        skillInfo.gameObject.SetActive(true);
    }

    public void BasicAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        attack = unitSelected.basicAttack;
        if (attack.targetsEnemy)
        {
            EnemySelectMessage();
        }
        else
            StartCoroutine(PlayerAttack());
    }

    public void OnMoveButton1()
    {
        moveNum = 1;
        AttackChosen();      
    }

    public void OnMoveButton2()
    {
        moveNum = 2;
        AttackChosen();
    }

    public void OnMoveButton3()
    {
        moveNum = 3;
        AttackChosen();
    }

    public void setMoveText(int moveNum)
    {
        Unit unitSelected = playerUnitsList[playerSelect - 1];
        Attacks attack = unitSelected.attackList[moveNum - 1];
        int damage;
        if (attack.isNoDamage)
            damage = 0;
        else
            damage = attack.damage + unitSelected.damage;

        attackDmgText.text = "Dmg: " + damage;
        attackCostText.text = "Cost: " + playerUnitsList[playerSelect - 1].attackList[moveNum - 1].cost.ToString();
        attackElmText.text = playerUnitsList[playerSelect - 1].attackList[moveNum - 1].element.ToString();
        move1Button.text = playerUnitsList[playerSelect - 1].attackList[0].attackName;
        move2Button.text = playerUnitsList[playerSelect - 1].attackList[1].attackName;
        move3Button.text = playerUnitsList[playerSelect - 1].attackList[2].attackName;
    } 


    public void HoverMoveButton1()
    {
        setMoveText(1);
    }

    public void HoverMoveButton2()
    {
        setMoveText(2);
    }

    public void HoverMoveButton3()
    {
        setMoveText(3);
    }

    public void MovePlayerIcon()
    {
        prevPlayer = playerSelect;
        playerUnitsList[prevPlayer - 1].transform.GetChild(1).gameObject.SetActive(false);
    }

    public void PlayerClick(int player)
    {

        unitSelected = playerUnitsList[player - 1];
        Transform playerUnitChild = unitSelected.transform.GetChild(1);

        playerUnitChild.gameObject.SetActive(true);

        dialogueText.text = "Choose an action for " + unitSelected.unitName + ":";

        move1Button.text = unitSelected.attackList[0].attackName;
        move2Button.text = unitSelected.attackList[1].attackName;
        move3Button.text = unitSelected.attackList[2].attackName;

        playerHUD.SetHUD(unitSelected);
    }


    public void Player1Click()
    {
        MovePlayerIcon();

        playerSelect = 1;

        PlayerClick(playerSelect);
    }

    public void Player2Click()
    {
        MovePlayerIcon();

        playerSelect = 2;

        PlayerClick(playerSelect);
    }

    public void Player3Click()
    {
        MovePlayerIcon();

        playerSelect = 3;

        PlayerClick(playerSelect);
    }

    public void Player4Click()
    {
        MovePlayerIcon();

        playerSelect = 4;

        PlayerClick(playerSelect);
    }

    public bool AttackValid()
    {
        if (attack.targetsAnyPos || attack.targetPositions.Contains(enemySel) || attack.targetsAll)
            return true;
        else
            return false;

    }

    public void EnemyClick(int enemySel)
    {
 
        Debug.Log("Enemy selected");
        if (AttackValid())
        {
            attack.target = enemyUnitsList[enemySel - 1];
            StartCoroutine(PlayerAttack());
        }
        else
            StartCoroutine(NotValidAttackMessage());

    }

    public void Enemy1Button()
    {
        enemySel = 1;

        EnemyClick(enemySel);
    }

    public void Enemy2Button()
    {
        enemySel = 2;

        EnemyClick(enemySel);
    }

    public void Enemy3Button()
    {
        enemySel = 3;

        EnemyClick(enemySel);
    }

    public void Enemy4Button()
    {
        enemySel = 4;

        EnemyClick(enemySel);
    }

    IEnumerator SPMessage()
    {
        skillMenu.gameObject.SetActive(false);
        skillInfo.gameObject.SetActive(false);
        dialogueMenu.gameObject.SetActive(true);

        dialogueText.text = "Not enough SP to use this skill";

        yield return new WaitForSeconds(1f);

        OnSkillButton();
    }

    IEnumerator NotValidAttackMessage()
    {
        for (int i = 0; i < enemyClickButtons.Count; i++)
        {
            enemyClickButtons[i].gameObject.SetActive(false);
        }

        enemySelectText.text = "Not a valid enemy for this attack";


        yield return new WaitForSeconds(1.5f);

        EnemySelectMessage();
    }



}
