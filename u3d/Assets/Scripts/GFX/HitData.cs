using System.Collections;
using UnityEngine;

//	HitData.cs
//	Author: Lu Zexi
//	2015-06-09



//hit data
public class HitData
{
	public string type;
	public int minAttack;
	public int maxAttack;
	public float crit;
	public int recoverHp;
	public int recoverMp;
	public int attackArmor;
	public int selfBuffId;
	public int targetBuffId;

	public int Attack{
		get{
			int at = Random.Range(minAttack,maxAttack);
			return at;
		}
	}


}

