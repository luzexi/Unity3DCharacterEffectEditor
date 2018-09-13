

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif


//	ActionExcute.cs
//	Author: Lu Zexi
//	2015-06-03



//action excute
public class ActionExcute
{
	public ActionTable m_Table = null;
	private List<ActionCommand> mListActionCommand = new List<ActionCommand>(4);
	private List<ActionCommand> mListActionCommandRemove = new List<ActionCommand>(4);

	//temp
	public ICustomObject target = null;	//target
	private ICustomObject owner = null;		//owner
	// private HitData m_HitData = null;	//hit data
	private float m_StartTime = 0;		//start time
	public bool mIsOver = false;	//is over
	private List<ActionObject.Event> mListCurrentEvents = new List<ActionObject.Event>(4);

	public void StartAction( ICustomObject obj )
	{
		this.owner = obj;
		this.m_StartTime = Time.time;
		mIsOver = false;
	}

	//update
	public void Update()
	{
		if(mIsOver)
			return;
		if(null == owner)
			return;
		if(m_Table == null)
			return;

		float time = Time.time - this.m_StartTime;

		if(m_Table.m_ActionObject.m_Time < time )
		{
			mIsOver = true;
			return;
		}

		mListCurrentEvents.Clear();
		mListActionCommandRemove.Clear();

		if(mListActionCommand.Count > 0)
		{
			for(int i = 0 ; i<mListActionCommand.Count ; i++)
			{
				ActionCommand cmd = mListActionCommand[i];
				if(!cmd.Update())
				{
					mListActionCommandRemove.Add(cmd);
				}
			}
			for(int i = 0 ; i<mListActionCommandRemove.Count ; i++)
			{
				mListActionCommand.Remove(mListActionCommandRemove[i]);
			}
		}
		
		// foreach( ActionObject.Event ev in m_Table.m_ActionObject.m_Events )
		for(int i = 0; i<m_Table.m_ActionObject.m_Events.Count ; i++)
		{
			if(m_Table.m_ActionObject.m_Events[i].time <= Time.time)
			{
				mListCurrentEvents.Add( m_Table.m_ActionObject.m_Events[i] );
			}
		}
		if(mListCurrentEvents.Count > 0)
		{
			for(int i = 0 ; i<mListCurrentEvents.Count ; i++)
			{
				ActionObject.Event ev = mListCurrentEvents[i];
				if(ev.messages.Count > 0)
				{
					for(int j = 0 ; j<ev.messages.Count ; j++)
					{
						mListActionCommand.Add(ActionCommand.CallCommand(owner, ev.messages[j].m_Function, ev.messages[j].m_Args));
					}
				}
				if(ev.m_AniName != string.Empty)
				{
					Animation ani = owner.GetAnimation();
					AnimationState anista = ani[ ev.m_AniName ];
					anista.wrapMode = ev.m_AniWarp;
					ani.CrossFade(ev.m_AniName);
				}
				EffectController effectController = null;
				if(ev.effect.name != string.Empty)
				{
					if(ev.effect.onoff)
					{
						//create effect
						effectController = EffectManager.I.Create( owner , ev.effect, target );
						if( effectController != null )
						{
							//todo
						}
					}
				}
				if ( ev.hit.onoff )
				{
					ICustomObject tmpTarget = null;
					if( ev.hit.isTarget )
					{
						tmpTarget = target;
					}
					HitController hitController = HitController.Create( owner , ev.hit, effectController , tmpTarget );
					if(hitController != null)
					{
						//todo
#if UNITY_EDITOR
						//action editor
						if( EditorSceneManager.GetActiveScene().name == "Assets/Scripts/Editor/CharacterEffectEditor.unity")
						{
							int damage_num = (int)(UnityEngine.Random.value*1000f);
							// HitNumber.SetData(hitController.transform.position, damage_num, HitNumberType.HpDown);
						}
#endif
					}
				}
				if ( !string.IsNullOrEmpty( ev.sound ) )
				{
					//play sound
				}
			}
		}

		return;
	}
}


