

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
public class ActionExcute : MonoBehaviour
{
	public ActionTable m_Table = null;

	//temp
	public GfxObject target = null;	//target
	private GfxObject owner = null;		//owner
	private HitData m_HitData = null;	//hit data
	private float m_StartTime = 0;		//start time
	private ActionObject currentAction = null;
	private float cur_old_time = -0.1f;
	private float cur_now_time = -0.1f;

	public static ActionExcute Create( ActionTable table , GfxObject o , HitData hitdata = null )
	{
		GameObject obj = new GameObject("ActionExcute");
		ActionExcute ac = obj.AddComponent<ActionExcute>();
		ac.m_Table = table;
		ac.m_HitData = hitdata;
		ac.StartAction(o);
		return ac;
	}


	public void StartAction( GfxObject obj )
	{
		this.owner = obj;
		this.m_StartTime = Time.time;
	}

	//update time
	private void UpdateTime( float update_time )
	{
		cur_old_time = cur_now_time;
		cur_now_time = update_time;
	}

	//update animation
	public void UpdateAnime( ActionObject ac )
	{
		if ( string.IsNullOrEmpty( ac.m_AniName ) )
			return;

		if ( cur_old_time < 0f || cur_now_time < cur_old_time )
		{
			owner.m_cAni.Stop();
			owner.m_cAni.Play( ac.m_AniName );
			AnimationState anista = owner.m_cAni[ ac.m_AniName ];
			if ( anista != null )
			{
				anista.time = cur_now_time;
			}
			else
			{
				Debug.LogError( "AnimeClip not Found! : " + ac.m_AniName );
			}
		}
	}

	//get event
	public List<ActionObject.Event> GetEvents( ActionObject ac , float old_time, float now_time )
	{
		List<ActionObject.Event> events = new List<ActionObject.Event>();
		if ( old_time <= now_time )
		{
			foreach( ActionObject.Event ev in ac.m_Events )
			{//|xxxxx old -> oooooo <- now xxxxxxx|
				if ( old_time < ev.time && ev.time <= now_time )
				{
					events.Add( ev );
				}
			}
		}
		else
		{
			//|ooo <- now xxxxxxxxxxxxxx old -> ooo|
			foreach( ActionObject.Event ev in ac.m_Events )
			{
				if ( old_time < ev.time )
				{
					events.Add( ev );
				}
			}
			foreach( ActionObject.Event ev in ac.m_Events )
			{
				if ( ev.time <= now_time )
				{
					events.Add( ev );
				}
			}
		}
		return events;
	}

	//get action object
	private float GetCurrentAction( float time , out ActionObject currentAction )
	{
		currentAction = null;
		foreach( ActionObject act in m_Table.m_ActionObjects )
		{
			if ( time < act.m_Time )
			{
				currentAction = act;
				break;
			}
			time -= act.m_Time;
		}
        return time;
	}


	//update
	void Update()
	{
		if(null == owner)
			return;
		if(m_Table == null)
		{
			GameObject.Destroy(this.gameObject);
			return;
		}

		float time = Time.time - this.m_StartTime;

		ActionObject actionOld = currentAction;
		ActionObject ac =null;
		time = GetCurrentAction(time,out ac);
		currentAction = ac;

		List<ActionObject.Event> events = new List<ActionObject.Event>();

		if(ac == null)
		{
			//over
			GameObject.Destroy(this.gameObject);
			return;
		}

		if(ac != actionOld)	//add old unexcute event
		{
			if( actionOld != null )
			{
				events.AddRange(GetEvents(actionOld,cur_old_time,cur_now_time));
			}
		}

		UpdateTime(time);	//update time
		UpdateAnime(ac);	//update animation
		events.AddRange(GetEvents(ac , cur_old_time , cur_now_time));	//add current event
		if(events.Count > 0)
		{
			foreach( ActionObject.Event ev in events )
			{
				if( ev.messages.Count > 0 )
				{
					for(int i = 0 ; i<ev.messages.Count ; i++ )
					{
						owner.SendMessage( "SkillMessage" ,ev.messages[i].m_Function + ";" + ev.messages[i].m_Args);
					}
				}
				EffectController effectController = null;
				if(ev.effect.id != string.Empty)
				{
					if(ev.effect.onoff)
					{
						//create effect
						effectController = EffectManager.I.Create( owner , ev.effect , this.m_HitData , target );
						if( effectController != null )
						{
							//todo
						}
					}
				}
				if ( ev.hit.onoff )
				{
					GfxObject tmpTarget = null;
					if( ev.hit.isTarget )
					{
						tmpTarget = target;
					}
					HitController hitController = HitController.Create( owner , ev.hit , this.m_HitData , effectController , tmpTarget );
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


