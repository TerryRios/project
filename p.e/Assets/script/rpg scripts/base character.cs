using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class basecharacter:BaseClass{

	public int stamina;
	public int intellect;
	public int dexterity;
	public int agility;

	public List<BaseAttack> SkillAttacks = new List<BaseAttack> ();
}
