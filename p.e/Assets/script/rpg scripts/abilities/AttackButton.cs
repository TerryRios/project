using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour {

	public BaseAttack SkillAttackToPerform;

	public void CastSkillAttack(){
		GameObject.Find ("BattleManager").GetComponent<battlestatemachine> ().Input4 (SkillAttackToPerform);
	}
}
