using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAttack:MonoBehaviour {

	public string attackName;//name
	public string attackDescription;
	public float attackDamage;//base damage
	public float skillCost;//skill cost
}
