
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

//event
public class ActionEvent : System.ICloneable
{
	public float mTime = 0f;

	public ActionObject.ActionType mActionType;

	protected ActionObject mActionObject;
	protected bool mIsInit = false;
	public bool IsInit
	{
		get
		{
			return mIsInit;
		}
		set
		{
			mIsInit = value;
		}
	}

	public ActionEvent()
	{
		//
	}

	public void SetActionObject(ActionObject act)
	{
		mActionObject = act;
	}

	public virtual void ReadJson(Dictionary<string,object> _data)
	{
		if(_data.ContainsKey("mActionType"))
		{
			mActionType = (ActionObject.ActionType)int.Parse(_data["mActionType"].ToString());
		}
		if(_data.ContainsKey("mTime"))
		{
			mTime = float.Parse(_data["mTime"].ToString());
		}
	}

	public virtual void Read(BinaryReader br)
	{
		// mActionType = br.ReadInt32() as ActionObject.ActionType;
		mTime = br.ReadSingle();
	}

	public virtual void WriteJson(Dictionary<string, object> _data)
	{
		_data.Add("mActionType", ((int)mActionType).ToString());
		_data.Add("mTime", mTime.ToString());
	}

	public virtual void Write(BinaryWriter bw)
	{
		bw.Write((int)mActionType);
		bw.Write(mTime);
	}

	public object Clone()
	{
		return null;
	}

	public virtual void Reset()
	{
		mIsInit = false;
	}

	public virtual void OnEnter()
	{
		return;
	}

	public virtual bool Do()
	{
		return true;
	}

	public virtual void OnExit()
	{
		return;
	}

#if UNITY_EDITOR
	private Texture2D m_timelineBGTex;
	private Vector2 window_size = new Vector2(521,20);

	private void TimeLine( float rate )
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
		for (j = 0; j < (int)window_size.y; ++j)
		{
			m_timelineBGTex.SetPixel( (int)(rate*window_size.x) , j, Color.red );
		}
		m_timelineBGTex.Apply();
		GUILayout.Label(m_timelineBGTex);
	}

	//draw editor gui
	public virtual void Draw( ActionObject aco )
	{
		GUILayout.BeginVertical();
		float rateTime = mTime/aco.mTotalTime;
		TimeLine(rateTime);
		rateTime = GUILayout.HorizontalSlider(rateTime,0,1f,GUILayout.Width(window_size.x));
		mTime = aco.mTotalTime * rateTime;
		float parentTime = aco.GetParentEventTime(this);
		if(parentTime > mTime)
		{
			mTime = parentTime;
		}
		GUILayout.EndVertical();
	}
#endif
}