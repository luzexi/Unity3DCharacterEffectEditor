

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

//	ActionObject.cs
//	Author: Lu Zexi
//	2015-05-01



//effect object
[System.Serializable]
public partial class ActionObject
{
	[SerializeField]
	public string m_Name = string.Empty;

	[SerializeField]
	public string m_AniName = string.Empty;

	[SerializeField]
	public List<Event> m_Events = new List<Event>();

	[SerializeField]
	public float m_Time = 1f;

	[SerializeField]
	public bool m_Loop = false;

	public void CopyFrom(ActionObject src )
	{
		m_Name = src.m_Name;
		m_AniName = src.m_AniName;
		m_Loop = src.m_Loop;
		m_Events = new List<Event>();
		foreach( Event ev in src.m_Events )
		{
			Event dest = new Event();
			dest.time = ev.time;
			dest.hit = (Hit)ev.hit.Clone();
			dest.effect = (Effect)ev.effect.Clone();
			dest.sound = ev.sound;
			dest.messages = new List<Message>();
			foreach( Message msg in ev.messages )
			{
				dest.messages.Add((Message)(msg.Clone()));
			}
			m_Events.Add( dest );
		}
		m_Time = src.m_Time;
	}

#if UNITY_EDITOR
	public void Draw( ActionTable actable )
	{
		GUILayout.BeginVertical();
		GUILayout.Label("============== Action =================");
		if (GUILayout.Button("- Action"))
		{
			actable.m_ActionObjects.Remove(this);
		}
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Name");
			this.m_Name = GUILayout.TextField(this.m_Name);
			GUILayout.Label("AniName");
			this.m_AniName = GUILayout.TextField(this.m_AniName);
			GUILayout.Label("Time");
			this.m_Time = EditorGUILayout.FloatField(this.m_Time);
			this.m_Loop = GUILayout.Toggle(this.m_Loop,"Loop");
			if (GUILayout.Button("+ Event"))
			{
				ActionObject.Event ev = new ActionObject.Event();
				this.m_Events.Add(ev);
			}
			GUILayout.EndHorizontal();
		}
		{
			GUILayout.BeginVertical();
			for(int i = 0 ; i<m_Events.Count ; i++ )
			{
				ActionObject.Event ev =this.m_Events[i];
				ev.Draw(this);
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndVertical();

	}
#endif

}
