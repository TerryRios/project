using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyselectButton : MonoBehaviour {

	private GameObject _Camera;
	private GameObject _cameraT;
	public GameObject enemyPrefab;
	private int cameramove;

	void Start (){
		_Camera = GameObject.FindGameObjectWithTag ("MainCamera");
		_cameraT = GameObject.Find ("punto");
	}

	public void SelectEnemy(){
		GameObject.Find ("BattleManager").GetComponent<battlestatemachine> ().Input2(enemyPrefab);
	}
	public void ToggleSelector(){
		enemyPrefab.transform.Find ("Selector").gameObject.SetActive (false);
		_Camera.transform.LookAt ( _cameraT.transform.position);
	//	cameramove = 2;
	}
	public void ToggleSelectordos(){
	//	cameramove = 1;
		enemyPrefab.transform.Find ("Selector").gameObject.SetActive (true);
		_Camera.transform.LookAt (enemyPrefab.transform.position);
	}
}
