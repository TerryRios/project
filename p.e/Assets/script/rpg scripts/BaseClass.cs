using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseClass {

	public string thename;
	public float baseHP;
	public float curHP;

	public float baseMP;
	public float CurMP;

	public float baseAtk;
	public float curAtk;
	public float baseDEF;
	public float curDEF;

	public List<BaseAttack> Attacks = new List<BaseAttack> ();
}
