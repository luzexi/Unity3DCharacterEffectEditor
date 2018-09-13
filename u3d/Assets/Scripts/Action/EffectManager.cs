
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//	EffectManager.cs
//	Author: Lu Zexi
//	2015-06-03



//effect manager
public class EffectManager
{
	private static EffectManager s_Instance;
	public static EffectManager I
	{
		get
		{
			if( s_Instance == null )
			{
				s_Instance = new EffectManager();
			}
			return s_Instance;
		}
	}

	//create effect
	public EffectController Create( ICustomObject owner , ActionObject.Effect effect, ICustomObject target )
	{
		Transform go = owner.GetTransform();
		GameObject obj = GameObject.Instantiate(Resources.Load("Effect/" + effect.name)) as GameObject;
		EffectController effectController = obj.AddComponent<EffectController>();
		effectController.time = effect.time;
		effectController.bullet = effect.bullet;
		effectController.target = target;
		effectController.owner = owner;
		// effectController.hitData = hitData;

		Transform trans = go.Find(effect.parent);
		if( trans != null )
		{
			obj.transform.parent = trans;
			obj.transform.localPosition = effect.offset;
			obj.transform.localRotation = Quaternion.Euler(effect.rotate);
			obj.transform.localScale = effect.scale;
		}

		if( effect.parent == "null" )
		{
			obj.transform.parent = go.parent;
			obj.transform.localPosition = go.localPosition + effect.offset;
			obj.transform.localRotation = Quaternion.Euler(go.localRotation.eulerAngles + effect.rotate);
			obj.transform.localScale = effect.scale;
		}

		return effectController;
	}
}
