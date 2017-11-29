using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour {

	private battlestatemachine BSM;

	public basecharacter _character;

	public enum TurnState{
		Processing,
		addtolist,
		waiting,
		selecting,
		action,
		dead
	}

	public TurnState currentState;
	private float cur_cooldown = 0f;
	private float max_cooldown = 5f;
	public Image progressBar;
	//IeNumerator
	public GameObject enemytoAttack;
	private bool actionStarted = false;
	private Vector3 startPosition;
	private float animSpeed = 10f;
	//dead
	private bool alive = true;
	//character panel
	private characterpanelstats stats;
	public GameObject characterPanel;
	private Transform characterPanelSpacer;

	// Use this for initialization
	void Start () {
		//encontrar spacer
		characterPanelSpacer = GameObject.Find("BattleCanvas").transform.Find("character panel").transform.Find("characterPanelSpacer");
		//crear panel,colocar informacion
		createCharacterPanel();

		startPosition = transform.position;
		cur_cooldown = Random.Range (0, 2.5f);
		BSM = GameObject.Find ("BattleManager").GetComponent<battlestatemachine> ();
		currentState = TurnState.Processing;
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState) {
		case(TurnState.Processing):
			upgradeProgressBar();
			break;
		case(TurnState.addtolist):
			BSM.charactersToManage.Add (this.gameObject);
			currentState = TurnState.waiting;
			break;
		case(TurnState.waiting):
			//idle
			break;
		case(TurnState.selecting):
			break;
		case(TurnState.action):
			StartCoroutine (TimeForAction ());
			break;
		case(TurnState.dead):
			if (!alive) {
				return;
			}else{
				//change tag
				this.gameObject.tag = "deadPlayerC";
				//not attackable by enemy
				BSM.charactersinBattle.Remove(this.gameObject);
				//not managable
				BSM.charactersToManage.Remove(this.gameObject);

				//reset gui
				BSM.attackPanel.SetActive(false);
				BSM.EnemySelect.SetActive (false);
				//remove item from perform list
				if (BSM.charactersinBattle.Count > 0) {
					for (int i = 0; i < BSM.PerformList.Count; i++) {
						if (BSM.PerformList [i].attackersGameObject == this.gameObject) {
							BSM.PerformList.Remove (BSM.PerformList [i]);
						}
						if (BSM.PerformList [i].attackersTarget == this.gameObject) {
							BSM.PerformList [i].attackersTarget = BSM.charactersinBattle [Random.Range (0, BSM.charactersinBattle.Count)];
						}
					}
				}

				//change color / play animation
	
				//reset heroinput
				BSM.battlestates = battlestatemachine.performAction.checkAlive;
				alive = false;
			}
			break;
		}
	}
	void upgradeProgressBar(){
		cur_cooldown = cur_cooldown + Time.deltaTime;
		float calc_cooldown = cur_cooldown / max_cooldown;
		progressBar.fillAmount = Mathf.Clamp (calc_cooldown, 0, 1);
		if (cur_cooldown >= max_cooldown) {
			currentState = TurnState.addtolist;
		}
	}
	private IEnumerator TimeForAction(){
		if (actionStarted) {
			yield break;
		}
		actionStarted = true;

		//animar al personaje cerca del enemigo para atacar
	//	Vector3 enemyPosition = new Vector3(enemytoAttack.transform.position.x+0.5f,enemytoAttack.transform.position.y,enemytoAttack.transform.position.z);
	//	while (MoveTowardsEnemy (enemyPosition)) {yield return null;}
		//esperar
		yield return new WaitForSeconds(0.5f);
		//hacer daño
		DoDamage();
		//animar a la posicion inicial
		Vector3 firstposition = startPosition;
		while (MoveTowardsStart(firstposition)) {yield return null;}
		//remover el performer de la lista en bsm
		BSM.PerformList.RemoveAt(0);
		//resetear bsm ->wait
		if (BSM.battlestates != battlestatemachine.performAction.win && BSM.battlestates != battlestatemachine.performAction.lose) {
			BSM.battlestates = battlestatemachine.performAction.wait;
			//resetear el estado del enemigo
			cur_cooldown = 0f;
			currentState = TurnState.Processing;
		} else {
			currentState = TurnState.waiting;
		}
		//finalizar co-rutina
		actionStarted = false;
	
	}
	private bool MoveTowardsEnemy(Vector3 target){
		return target != (transform.position = Vector3.MoveTowards (transform.position, target, animSpeed * Time.deltaTime));
	}
	private bool MoveTowardsStart(Vector3 target){
		return target != (transform.position = Vector3.MoveTowards (transform.position, target, animSpeed * Time.deltaTime));
	}
	public void TakeDamage (float getDamageAmount)
	{
		_character.curHP -= getDamageAmount;
		if (_character.curHP <= 0) {
			_character.curHP = 0;
			currentState = TurnState.dead;
		}
		UpdateCharacterPanel ();
	}
	void DoDamage(){
		float calc_damage = _character.curAtk + BSM.PerformList [0].choosenAttack.attackDamage;
		enemytoAttack.GetComponent<EnemyStateMachine> ().TakeDamage (calc_damage);
	}

	void createCharacterPanel(){
		characterPanel = Instantiate (characterPanel) as GameObject;
		stats = characterPanel.GetComponent<characterpanelstats> ();
		stats.characterName.text = _character.thename;
		stats.characterHP.text = "HP: " + _character.curHP;
		stats.characterMP.text = "MP: " + _character.CurMP;

		progressBar = stats.progressBar;
		characterPanel.transform.SetParent (characterPanelSpacer, false);
	}
	//actualiza stats del daño / curacion
	void UpdateCharacterPanel(){
		stats.characterHP.text = "HP: " + _character.curHP;
		stats.characterMP.text = "MP: " + _character.CurMP;

	}
}
