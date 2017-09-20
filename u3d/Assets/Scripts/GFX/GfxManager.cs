
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//	GfxManager.cs
//	Author: Lu Zexi
//	2015-06-08



//GfxManager
public class GfxManager
{
	private static GfxManager s_Instance;
	public static GfxManager I
	{
		get
		{
			if(s_Instance == null)
			{
				s_Instance = new GfxManager();
			}
			return s_Instance;
		}
	}

	public List<GfxObject> m_List = new List<GfxObject>();


	public GfxObject CreateGFX( string path)
	{
		GameObject obj = GameObject.Instantiate(Resources.Load(path)) as GameObject;
		GfxObject gfx = null;
		gfx = obj.AddComponent<GfxObject>();
		Add(gfx);
		return gfx;
	}

	// public AIObject CreateAI( string path)
	// {
	// 	Debug.Log("create ai path " + path);
	// 	GameObject obj = GameObject.Instantiate(Resources.Load(path)) as GameObject;
	// 	AIObject ai = null;
	// 	ai = obj.AddComponent<AIObject>();
	// 	Add(ai);
	// 	return ai;
	// }


	//add
	public void Add( GfxObject obj )
	{
		this.m_List.Add(obj);
	}

	//remove
	public void Remove( GfxObject obj )
	{
		this.m_List.Remove(obj);
	}

	//clear
	public void Clear()
	{
		for(int i = 0 ; i<this.m_List.Count ; i++)
		{
			if(this.m_List[i] != null)
			{
				GameObject.Destroy(this.m_List[i]);
			}
		}
		this.m_List.Clear();
	}
}

