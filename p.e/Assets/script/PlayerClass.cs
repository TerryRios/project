using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass {

	public string Playername;
	public int level;
	public int curHealth;
	public int Health;
	public int curSp;
	public int Sp;
	public int def;
	public int atk;
	public int xp;
	public int xpRequired;

	public GameObject attack;

	public int stamina;
	public int intellect;
	public int dexerity;
	public int agility;

	public List<BaseAttack> skillAttack = new List<BaseAttack>();
}
