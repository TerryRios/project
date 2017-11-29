using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurn {

	public string attacker;
	public string Type;
	public GameObject attackersGameObject;
	public GameObject attackersTarget;

	public BaseAttack choosenAttack;
}
