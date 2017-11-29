using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : BaseAttack{

	public Charge(){
		attackName = "Charge";
		attackDescription = "charges against the enemy";
		attackDamage = 30f;
		skillCost = 10f;
	}
}
