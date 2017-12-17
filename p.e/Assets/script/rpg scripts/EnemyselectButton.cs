using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyselectButton : MonoBehaviour {

	public GameObject enemyPrefab;

	public void SelectEnemy(){
		GameObject.Find ("BattleManager").GetComponent<battlestatemachine> ().Input2(enemyPrefab);
	}
	public void ToggleSelector(){
		enemyPrefab.transform.Find ("Selector").gameObject.SetActive (false);
	//	cameramove = 2;
	}
	public void ToggleSelectordos(){
	//	cameramove = 1;
		enemyPrefab.transform.Find ("Selector").gameObject.SetActive (true);
	}
}
