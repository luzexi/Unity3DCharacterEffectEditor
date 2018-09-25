

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
public partial class ActionObject
{
	// public string mName = string.Empty;

	public float mTotalTime = 1f;

	public List<ActionEvent> mEvents = new List<ActionEvent>();

	public ActionEvent mCurrentEvent = null;
	public int mCurrentEventIndex = -1;

	public ICustomObject mCustomObject;

	private Dictionary<int, GameObject> mDicPrefab = new Dictionary<int, GameObject>();

	public float GetParentEventTime(ActionEvent _ev)
	{
		int index = mEvents.IndexOf(_ev);
		if(index > 0)
		{
			return mEvents[index - 1].mTime;
		}
		return 0;
	}

	public void AddPrefab(int id, GameObject obj)
	{
		mDicPrefab.Add(id, obj);
	}

	public void RemovePrefab(int id)
	{
		mDicPrefab.Remove(id);
	}

	public GameObject GetPrefab(int id)
	{
		return mDicPrefab[id];
	}

	public void CopyFrom(ActionObject src )
	{
		// mName = src.mName;
		mTotalTime = src.mTotalTime;
		mEvents = new List<ActionEvent>();
		foreach( ActionEvent ev in src.mEvents )
		{
			ActionEvent dest = (ActionEvent)ev.Clone();
			dest.SetActionObject(this);
			mEvents.Add(dest);
		}
	}

	public void ReadJson(Dictionary<string,object> _jsonObj)
	{
		if(_jsonObj.ContainsKey("mTotalTime"))
		{
			mTotalTime = int.Parse(_jsonObj["mTotalTime"].ToString());
		}
		mEvents.Clear();
		if(_jsonObj.ContainsKey("mEvents"))
		{
			List<object> lst = _jsonObj["mEvents"] as List<object>;
			if(lst != null)
			{
				for(int i = 0 ; i<lst.Count ; i++)
				{
					Dictionary<string, object> action_json = lst[i] as Dictionary<string, object>;
					ActionObject.ActionType actioinType = ActionObject.ActionType.DoNothing;
					if(action_json.ContainsKey("mActionType"))
					{
						actioinType = (ActionObject.ActionType)int.Parse(action_json["mActionType"].ToString());
					}
					ActionEvent ev = ActionFactory.CreateActionEvent(actioinType);
					if(ev != null)
					{
						ev.ReadJson(action_json);
					}
					mEvents.Add(ev);
				}
			}
		}
	}

	public void Read(BinaryReader br)
	{
		// mName = br.ReadString();
        mTotalTime = br.ReadSingle();
        int count = br.ReadInt32();
        mEvents.Clear();
        for(int i = 0 ; i<count ; i++)
        {
        	ActionObject.ActionType actioinType = (ActionObject.ActionType)br.ReadInt32();
        	ActionEvent ev = ActionFactory.CreateActionEvent(actioinType);
        	if(ev != null)
        	{
	        	ev.SetActionObject(this);
	        	ev.Read(br);
	        	mEvents.Add(ev);
	        }
        }
	}

	public void WriteJson(Dictionary<string,object> _json)
	{
		_json.Add("mTotalTime",mTotalTime.ToString());
		List<object> lst = new List<object>();
		for(int i = 0 ; i<mEvents.Count ; i++)
		{
			Dictionary<string,object> action_data = new Dictionary<string,object>();
			mEvents[i].WriteJson(action_data);
			lst.Add(action_data);
		}
		_json.Add("mEvents",lst);
	}

	public void Write(BinaryWriter bw)
	{
		// FileStream fs = new FileStream(filetowrite, FileMode.Create);
        // BinaryWriter bw = new BinaryWriter(fs, Encoding.Unicode);
        // bw.Write(mName);
        bw.Write(mTotalTime);
        bw.Write(mEvents.Count);
        for(int i = 0 ; i<mEvents.Count ; i++)
        {
        	mEvents[i].Write(bw);
        }
	}

#if UNITY_EDITOR
	private Texture2D mTotalTimelineBGTex;
	private Vector2 window_size = new Vector2(521,20);

	private void TimeLine()
	{
		if ( mTotalTimelineBGTex == null )
		{
			mTotalTimelineBGTex = new Texture2D((int)window_size.x, (int)window_size.y);
		}
		int i = 0, j = 0;
		for (i = 0; i < (int)window_size.x; ++i)
		{
			for (j = 0; j < (int)window_size.y; ++j)
			{
				mTotalTimelineBGTex.SetPixel( i, j, Color.white );
			}
		}
	}

	private void TimeLine(float rate, Color _color)
	{
		for(int j=0 ; j<(int)window_size.y ; j++)
		{
			mTotalTimelineBGTex.SetPixel( (int)(rate*window_size.x) , j, _color);
		}
	}

	private ActionObject.ActionType mActionEventType;
	public void Draw( ActionTable actable )
	{
		GUILayout.BeginVertical();
		TimeLine();
		for(int i = 0 ; i<mEvents.Count ; i++)
		{
			float rate = mEvents[i].mTime/mTotalTime;
			TimeLine(rate, Color.red);
		}
		if(mCurrentEvent != null)
		{
			float rate = mCurrentEvent.mTime/mTotalTime;
			TimeLine(rate, Color.blue);
		}
		mTotalTimelineBGTex.Apply();
		GUILayout.Label(mTotalTimelineBGTex);

		GUILayout.BeginHorizontal();
		GUILayout.Label("============== Action =================");
		GUILayout.EndHorizontal();

		{
			GUILayout.BeginHorizontal();

			GUILayout.BeginHorizontal();
			// this.mName = EditorGUILayout.TextField("Name",this.mName);
			this.mTotalTime = EditorGUILayout.FloatField("Total Time", this.mTotalTime);
			mActionEventType = (ActionObject.ActionType)EditorGUILayout.EnumPopup("Event Type",mActionEventType);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Change Event"))
			{
				if(mCurrentEventIndex >= 0 && mCurrentEventIndex < mEvents.Count)
				{
					ActionEvent ev = ActionFactory.CreateActionEvent(mActionEventType);
					if(ev != null)
					{
						ev.SetActionObject(this);
						this.mEvents[mCurrentEventIndex] = ev;
						this.mCurrentEvent = ev;
					}
				}
			}
			if (GUILayout.Button("Add Event"))
			{
				ActionEvent ev = ActionFactory.CreateActionEvent(mActionEventType);
				if(ev != null)
				{
					ev.SetActionObject(this);
					mCurrentEvent = ev;
					mCurrentEventIndex = mEvents.Count;
					this.mEvents.Add(ev);
				}
			}
			if (GUILayout.Button("Delete Event"))
			{
				if(EditorUtility.DisplayDialog("Remove Event","Are you sure to remove Event?","Remove","Cancel"))
				{
					if(mCurrentEvent != null)
					{
						mEvents.Remove(mCurrentEvent);
						mCurrentEvent = null;
						mCurrentEventIndex = -1;
					}
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.EndHorizontal();
		}
		{
			GUILayout.BeginHorizontal();
			for(int i = 0 ; i<mEvents.Count ; i++ )
			{
				ActionEvent ev =this.mEvents[i];
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
		}
		GUILayout.EndVertical();

	}
#endif

}
