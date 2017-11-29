using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class baseenemy:BaseClass {

	public enum Type{
		piercing,
		cuting,
		fire,
		electricity
	}
	public enum Rarity{
		common,
		uncommon,
		rare,
		superrare
	}
	public Type EnemyWeakness;
	public Rarity rarity;
}
