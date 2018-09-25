

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


//action excute
public class ActionExcuteManager : MonoBehaviour
{
	private static ActionExcuteManager sInstance;
	public static ActionExcuteManager instance
	{
		get
		{
			if(sInstance == null)
			{
				GameObject obj = new GameObject("ActionExcuteManager");
				sInstance = obj.AddComponent<ActionExcuteManager>();
				DontDestroyOnLoad(obj);
			}
			return sInstance;
		}
	}

	public List<ActionExcute> mListExcute = new List<ActionExcute>(16);

	void Awake()
	{
		if(sInstance == null)
		{
			sInstance = this;
		}
	}

	void OnDestroy()
	{
		if( sInstance == this )
		{
			sInstance = null;
		}
	}

	public ActionExcute StartAction( ActionObject actObj , ICustomObject o )
	{
		ActionExcute ac = new ActionExcute();
		ac.StartAction(o, actObj);
		mListExcute.Add(ac);
		return ac;
	}

	void Update()
	{
		for(int i = 0 ; i<mListExcute.Count ;)
		{
			mListExcute[i].Update();
			if(mListExcute[i].mIsOver)
			{
				mListExcute.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	public static bool IsValid()
	{
		return ( sInstance != null ) ;
	}
}