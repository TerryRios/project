using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour {

	private battlestatemachine BSM;
	public baseenemy _enemy;

	public enum TurnState{
		Processing,
		chooseaction,
		waiting,
		action,
		dead
	}

	public TurnState currentState;
	private float cur_cooldown = 0f;
	private float max_cooldown = 10f;
	private Vector3 startposition;
	public GameObject Selector;
	private bool actionStarted = false;
	public GameObject charactertoattack;
	private float animSpeed = 5f;
	//alive
	private bool alive = true;

	void OnEnable(){
		if (Gamemanager.instance.Atacado == true) {
			currentState = TurnState.Processing;
			Selector.SetActive (false);
			BSM = GameObject.Find ("BattleManager").GetComponent<battlestatemachine> ();
			startposition = transform.position;
		}
	}
	void OnDisable(){
		this.gameObject.tag = "Enemy";
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState) {
		case(TurnState.Processing):
			upgradeProgressBar();
			break;
		case(TurnState.chooseaction):
			ChooseAction ();
			currentState = TurnState.waiting;
			break;
		case(TurnState.waiting):
			//idle
			break;
		case(TurnState.action):
			StartCoroutine (TimeForAction ());
			break;
		case(TurnState.dead):
			if (!alive) {
				return;
			} 
			else 
			{
				//change tag
				this.gameObject.tag = "deadEnemy";
				//not attackable
				BSM.enemysinBattle.Remove(this.gameObject);
				//disable selector
				Selector.SetActive (false);
				if (BSM.enemysinBattle.Count > 0) {
			
					for (int i = 0; i < BSM.PerformList.Count; i++) {
						if (i != 0) {
							if (BSM.PerformList [i].attackersGameObject == this.gameObject) {
								BSM.PerformList.Remove (BSM.PerformList [i]);
							}
							if (BSM.PerformList [i].attackersTarget == this.gameObject) {
								BSM.PerformList [i].attackersTarget = BSM.enemysinBattle [Random.Range (0, BSM.enemysinBattle.Count)];
							}
						}
					}
				}
				// change color / animation

				//set alive false
				alive = false;
				//reset enemybuttons
				BSM.EnemyButtons ();
				//check alive
				BSM.battlestates = battlestatemachine.performAction.checkAlive;
			}
			break;
		}
	}

	void upgradeProgressBar(){
		cur_cooldown = cur_cooldown + Time.deltaTime;

		if (cur_cooldown >= max_cooldown) {
			currentState = TurnState.chooseaction;
		}
	}

	void ChooseAction(){
		HandleTurn myAttack = new HandleTurn ();
		myAttack.attacker = _enemy.thename;
		myAttack.Type = "Enemy";
		myAttack.attackersGameObject = this.gameObject;
		myAttack.attackersTarget = BSM.charactersinBattle [Random.Range (0, BSM.charactersinBattle.Count)];

		int num = Random.Range (0, _enemy.Attacks.Count);
		myAttack.choosenAttack = _enemy.Attacks [num];

		BSM.collectActions (myAttack);
	}

	private IEnumerator TimeForAction(){
		if (actionStarted) {
			yield break;
		}
		actionStarted = true;

		//animar al enemigo cerca del personaje para atacar
		Vector3 characterPosition = new Vector3(charactertoattack.transform.position.x-0.5f,charactertoattack.transform.position.y,charactertoattack.transform.position.z);
		while (MoveTowardsEnemy (characterPosition)) {
			yield return null;
		}
		//esperar
		yield return new WaitForSeconds(0.5f);
		//hacer daño
		DoDamage();
		//animar a la posicion inicial
		Vector3 firstposition = startposition;
		while (MoveTowardsStart(firstposition)) {
			yield return null;
		}
		//remover el performer de la lista en bsm
		BSM.PerformList.RemoveAt(0);
		//resetear bsm ->wait
		BSM.battlestates = battlestatemachine.performAction.wait;
		//finalizar co-rutina
		actionStarted = false;
		//resetear el estado del enemigo
		cur_cooldown = 0f;
		currentState = TurnState.Processing;
	}

	private bool MoveTowardsEnemy(Vector3 target){
		return target != (transform.position = Vector3.MoveTowards (transform.position, target, animSpeed * Time.deltaTime));
	}
	private bool MoveTowardsStart(Vector3 target){
		return target != (transform.position = Vector3.MoveTowards (transform.position, target, animSpeed * Time.deltaTime));
	}
	void DoDamage(){
		float calc_damage = _enemy.curAtk + BSM.PerformList [0].choosenAttack.attackDamage;
		charactertoattack.GetComponent<HeroStateMachine> ().TakeDamage(calc_damage);
	}

	public void TakeDamage(float getDamageAmount){
		_enemy.curHP -= getDamageAmount;
		if (_enemy.curHP <= 0) {
			_enemy.curHP = 0;
			currentState = TurnState.dead;
		}
	}
}
