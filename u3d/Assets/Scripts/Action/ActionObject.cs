

using UnityEngine;
using System;
using System.IO;
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
	public bool m_IsLookTarget;

	[SerializeField]
	public float m_Time = 1f;

	[SerializeField]
	public List<Event> m_Events = new List<Event>();

	[SerializeField]
	public bool mHiden = false;	//editor

	private Event mCurrentEvent = null;
	public Event CurrentEvent
	{
		get
		{
			return mCurrentEvent;
		}
		set
		{
			mCurrentEvent = value;
		}
	}
	private int mCurrentEventIndex = -1;
	public int CurrentEventIndex
	{
		get
		{
			return mCurrentEventIndex;
		}
	}

	public void CopyFrom(ActionObject src )
	{
		m_Name = src.m_Name;
		m_IsLookTarget = src.m_IsLookTarget;
		m_Events = new List<Event>();
		foreach( Event ev in src.m_Events )
		{
			Event dest = new Event();
			dest.m_AniName = ev.m_AniName;
			dest.m_AniWarp = ev.m_AniWarp;
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

	public void Read(BinaryReader br)
	{
		m_Name = br.ReadString();
        m_IsLookTarget = (bool)br.ReadBoolean();
        m_Time = br.ReadSingle();
        int count = br.ReadInt32();
        m_Events.Clear();
        for(int i = 0 ; i<count ; i++)
        {
        	Event ev = new Event();
        	ev.Read(br);
        	m_Events.Add(ev);
        }
	}

	public void Write(BinaryWriter bw)
	{
		// FileStream fs = new FileStream(filetowrite, FileMode.Create);
        // BinaryWriter bw = new BinaryWriter(fs, Encoding.Unicode);
        bw.Write(m_Name);
        bw.Write(m_IsLookTarget);
        bw.Write(m_Time);
        bw.Write(m_Events.Count);
        for(int i = 0 ; i<m_Events.Count ; i++)
        {
        	m_Events[i].Write(bw);
        }
	}

#if UNITY_EDITOR
	private Texture2D m_timelineBGTex;
	private Vector2 window_size = new Vector2(521,20);

	private void TimeLine()
	{
		if ( m_timelineBGTex == null )
		{
			m_timelineBGTex = new Texture2D((int)window_size.x, (int)window_size.y);
		}
		int i = 0, j = 0;
		for (i = 0; i < (int)window_size.x; ++i)
		{
			for (j = 0; j < (int)window_size.y; ++j)
			{
				m_timelineBGTex.SetPixel( i, j, Color.white );
			}
		}
	}
	private void TimeLine(float rate, Color _color)
	{
		for(int j=0 ; j<(int)window_size.y ; j++)
		{
			m_timelineBGTex.SetPixel( (int)(rate*window_size.x) , j, _color);
		}
	}

	public void Draw( ActionTable actable )
	{
		GUILayout.BeginVertical();
		TimeLine();
		for(int i = 0 ; i<m_Events.Count ; i++)
		{
			float rate = m_Events[i].time/m_Time;
			TimeLine(rate, Color.red);
		}
		if(CurrentEvent != null)
		{
			float rate = CurrentEvent.time/m_Time;
			TimeLine(rate, Color.blue);
		}
		m_timelineBGTex.Apply();
		GUILayout.Label(m_timelineBGTex);

		GUILayout.BeginHorizontal();
		GUILayout.Label("============== Action " + m_Name + " =================");
		GUILayout.EndHorizontal();
		if(mHiden)
		{
			if (GUILayout.Button(">>>Action"))
			{
				mHiden = false;
			}
			GUILayout.EndVertical();
			return;
		}
		else
		{
			if (GUILayout.Button("<<<Action"))
			{
				mHiden = true;
			}
		}

		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Name");
			this.m_Name = GUILayout.TextField(this.m_Name);
			GUILayout.Label("Time");
			this.m_Time = EditorGUILayout.FloatField(this.m_Time);
			this.m_IsLookTarget = GUILayout.Toggle(this.m_IsLookTarget ,"LookTarget");
			if (GUILayout.Button("add Event"))
			{
				ActionObject.Event ev = new ActionObject.Event();
				this.m_Events.Add(ev);
			}
			if (GUILayout.Button("delete Event"))
			{
				if(EditorUtility.DisplayDialog("Remove Event","Are you sure to remove Event?","Remove","Cancel"))
				{
					m_Events.Remove(mCurrentEvent);
					mCurrentEvent = null;
					mCurrentEventIndex = -1;
				}
			}
			GUILayout.EndHorizontal();
		}
		{
			GUILayout.BeginHorizontal();
			for(int i = 0 ; i<m_Events.Count ; i++ )
			{
				ActionObject.Event ev =this.m_Events[i];
				// ev.Draw(this);
				if (GUILayout.Button("" + i))
				{
					mCurrentEvent = ev;
					mCurrentEventIndex = i;
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginVertical();
			if(mCurrentEvent != null)
			{
				GUILayout.Label("============== Event " + mCurrentEventIndex + " =================");
				mCurrentEvent.Draw(this);
			}
			GUILayout.EndVertical();
			// GUILayout.BeginVertical();
			// for(int i = 0 ; i<m_Events.Count ; i++ )
			// {
			// 	ActionObject.Event ev =this.m_Events[i];
			// 	ev.Draw(this);
			// }
			// GUILayout.EndVertical();
		}
		GUILayout.EndVertical();

	}
#endif

}
