using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battlestatemachine : MonoBehaviour {

	public enum performAction{
		wait,
		takeaction,
		performaction,
		checkAlive,
		win,
		lose
	}

	public performAction battlestates;

	public List<HandleTurn> PerformList = new List<HandleTurn>();
	public List<GameObject> charactersinBattle = new List<GameObject> ();
	public List<GameObject> enemysinBattle = new List<GameObject> ();

	public enum characterGUI
	{
		activate,
		waiting,
		input1,
		input2,
		done
	}

	public characterGUI characterInput;

	public List<GameObject> charactersToManage = new List<GameObject> ();
	private HandleTurn characterChoise;
	public GameObject enemyButton;
	public Transform Spacer;

	private GameObject battlecanvas; 

	public GameObject attackPanel;
	public GameObject EnemySelect;
	public GameObject SkillsPanel;

	public Transform actionSpacer;
	public Transform Skillspacer;
	public GameObject actionButton;
	public GameObject skillButton;
	private List<GameObject> atkBtns = new List<GameObject> ();

	private List<GameObject> enemyBtns = new List<GameObject>();

	// Use this for initialization
	void Start () {
		
		battlecanvas = GameObject.Find ("BattleCanvas");
		Spacer = battlecanvas.transform.GetChild (4).transform.GetChild (0).transform;
		attackPanel = battlecanvas.transform.GetChild (3).gameObject;
		attackPanel.SetActive (false);
		EnemySelect = battlecanvas.transform.GetChild (4).gameObject;
		EnemySelect.SetActive (false);
		SkillsPanel = battlecanvas.transform.GetChild (5).gameObject;
		SkillsPanel.SetActive (false);
		actionSpacer = battlecanvas.transform.GetChild (3).transform.GetChild (0).transform;
		Skillspacer = battlecanvas.transform.GetChild (5).transform.GetChild (0).transform;

		battlestates = performAction.wait;
		charactersinBattle.AddRange (GameObject.FindGameObjectsWithTag ("Player c"));
	}
	
	// Update is called once per frame
	void Update () {
		if (Gamemanager.instance.detectE == true) {
			enemysinBattle.AddRange (GameObject.FindGameObjectsWithTag ("Enemy"));
			characterInput = characterGUI.activate;
			attackPanel.SetActive (false);
			EnemySelect.SetActive (false);
			SkillsPanel.SetActive (false);
			EnemyButtons ();
			Gamemanager.instance.noEnemys = false;
			Gamemanager.instance.detectE = false;
		}	
		
		switch (battlestates) 
		{
		case(performAction.wait):
			if (PerformList.Count > 0) {
				battlestates = performAction.takeaction;
			}
			break;
		case(performAction.takeaction):
			GameObject performer = GameObject.Find (PerformList [0].attacker);
			if (PerformList [0].Type == "Enemy") 
			{
				EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine> ();
				for (int i = 0; i < charactersinBattle.Count; i++) 
				{
					if (PerformList [0].attackersTarget == charactersinBattle [i]) 
					{
						ESM.charactertoattack = PerformList [0].attackersTarget;
						ESM.currentState = EnemyStateMachine.TurnState.action;
						break;
					} else 
					{
						PerformList[0].attackersTarget = charactersinBattle[Random.Range(0,charactersinBattle.Count)];
						ESM.charactertoattack = PerformList [0].attackersTarget;
						ESM.currentState = EnemyStateMachine.TurnState.action;
					}
				}
			}
			if (PerformList [0].Type == "Player c") {
				HeroStateMachine HSM = performer.GetComponent<HeroStateMachine> ();
				HSM.enemytoAttack = PerformList [0].attackersTarget;
				HSM.currentState = HeroStateMachine.TurnState.action;
			}
			battlestates = performAction.performaction;
			break;
		case(performAction.performaction):
			//idle
			break;
		case(performAction.checkAlive):
			if (charactersinBattle.Count < 1) 
			{
				battlestates = performAction.lose;
				//lose
			} else if (enemysinBattle.Count < 1) 
			{
				Gamemanager.instance.noEnemys = true;
				battlestates = performAction.win;
				//win
			} else 
			{
				clearAttackPanel();
				characterInput = characterGUI.activate;
				battlestates = performAction.wait;
			}
			break;
		case (performAction.lose):
			{
				Debug.Log ("lose");
		    }
		break;
		case(performAction.win):
			{
				Debug.Log ("win");
				for (int i = 0; i < charactersinBattle.Count; i++) {
					charactersinBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.waiting;
				}
			}
			break;
		}
		switch(characterInput){
		    case(characterGUI.activate):
			if (charactersToManage.Count>0) 
			{
				characterChoise = new HandleTurn ();

				attackPanel.SetActive (true);

				CreateAttackButtons ();
				characterInput = characterGUI.waiting;
			}
			break;
		case(characterGUI.waiting):
			//Idle
			break;
		case(characterGUI.done):
			characterInputDone ();
			break;
		    }
	}

	public void collectActions(HandleTurn input){
		PerformList.Add (input);
	}
	public void EnemyButtons()
	{
		//clean
		foreach(GameObject enemyBtn in enemyBtns){
			Destroy (enemyBtn);
		}
		enemyBtns.Clear ();
		//create buttons
		foreach (GameObject enemy in enemysinBattle) {
			GameObject newbutton = Instantiate (enemyButton) as GameObject;
			EnemyselectButton button = newbutton.GetComponent<EnemyselectButton> ();

			EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine> ();

			Text buttontext = newbutton.transform.Find ("Text").gameObject.GetComponent<Text> ();
			buttontext.text = cur_enemy._enemy.thename;

			button.enemyPrefab = enemy;

			newbutton.transform.SetParent (Spacer,false);
			enemyBtns.Add (newbutton);
		}
	}
	//charactersToManage [0].GetComponent<characterStateMachine> ().character.name;
	public void Input1()//attack button
	{
		characterChoise.attacker = charactersToManage [0].name;
		characterChoise.attackersGameObject = charactersToManage [0];
		characterChoise.Type = "Player c";
		characterChoise.choosenAttack = charactersToManage [0].GetComponent<HeroStateMachine> ()._character.Attacks [0];
		attackPanel.SetActive (false);
		EnemySelect.SetActive (true);
	}

	public void Input2(GameObject choosenEnemy)//enemy selection
	{
		characterChoise.attackersTarget = choosenEnemy;
		characterInput = characterGUI.done;
	}

	void characterInputDone(){
		PerformList.Add (characterChoise);
		clearAttackPanel ();

		charactersToManage.RemoveAt (0);
		characterInput = characterGUI.activate;
	}

	void clearAttackPanel()
	{
		EnemySelect.SetActive (false);
		attackPanel.SetActive (false);
		SkillsPanel.SetActive (false);
		foreach (GameObject atkBtn in atkBtns){
			Destroy (atkBtn);
		}
		atkBtns.Clear ();
	}

	void CreateAttackButtons(){
		GameObject AttackButton = Instantiate (actionButton) as GameObject;
		Text AttackButtonText = AttackButton.transform.Find ("Text").gameObject.GetComponent<Text> ();
		AttackButtonText.text = "Attack";
		AttackButton.GetComponent<Button> ().onClick.AddListener (() => Input1 ());
		AttackButton.transform.SetParent (actionSpacer, false);
		atkBtns.Add (AttackButton);

		GameObject SkillAttackButton = Instantiate (actionButton) as GameObject;
		Text SkillAttackButtonText = SkillAttackButton.transform.Find ("Text").gameObject.GetComponent<Text> ();
		SkillAttackButtonText.text = "Skill";
		SkillAttackButton.GetComponent<Button> ().onClick.AddListener (() => Input3 ());
		SkillAttackButton.transform.SetParent (actionSpacer, false);
		atkBtns.Add (SkillAttackButton);
		if (charactersToManage[0].GetComponent<HeroStateMachine>()._character.SkillAttacks.Count>0) 
		{
			foreach(BaseAttack skillAtk in charactersToManage[0].GetComponent<HeroStateMachine>()._character.SkillAttacks)
			{
				GameObject SkillsButton = Instantiate (skillButton) as GameObject;
				Text SkillButtonText = SkillsButton.transform.Find ("Text").gameObject.GetComponent<Text> ();
				SkillButtonText.text = skillAtk.attackName;

				AttackButton ATB = SkillsButton.GetComponent<AttackButton> ();
				ATB.SkillAttackToPerform = skillAtk;
				SkillsButton.transform.SetParent (Skillspacer, false);
				atkBtns.Add (SkillsButton);
			}
		}
		else{
			SkillAttackButton.GetComponent<Button> ().interactable = false;
		}
	}
	public void Input4(BaseAttack choosenSkill){
		
		characterChoise.attacker = charactersToManage [0].name;
		characterChoise.attackersGameObject = charactersToManage [0];
		characterChoise.Type = "Player c";

		characterChoise.choosenAttack = choosenSkill;
		SkillsPanel.SetActive (false);
		EnemySelect.SetActive (true);
		
	}
	public void Input3(){
		attackPanel.SetActive (false);
		SkillsPanel.SetActive (true);
	}
}
