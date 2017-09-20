

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


//	ActionObject_Event.cs
//	Author: Lu Zexi
//	2015-06-03



//effect excuter
public partial class ActionObject
{
	//hit
	[System.Serializable]
	public class Hit : System.ICloneable
	{
		public bool onoff = false;	//bind effect
		public string type = string.Empty;	//hit type
		public bool isTarget = false;	//is target
		public bool bindEffect = false;	//bind effect
		public Vector3 size = Vector3.one; //size
		public Vector3 rotate = Vector3.zero;	//rotate
		public Vector3 offset = Vector3.zero;	//offset pos
		public string parent = string.Empty;	//parent
		public float time = 0.1f;			//time

		public object Clone()
		{
			Hit ret = new Hit();
			ret.onoff = onoff;
			ret.type = type;
			ret.isTarget = isTarget;
			ret.bindEffect = bindEffect;
			ret.size = size;
			ret.rotate = rotate;
			ret.offset	= offset;
			ret.parent	= parent;
			ret.time	= time;
			return ret;
		}

#if UNITY_EDITOR
		public void Draw()
		{
			GUILayout.BeginVertical();
			GUILayout.Label("------------------- Hit -------------------");

			{
				GUILayout.BeginHorizontal();
				this.onoff =  GUILayout.Toggle(this.onoff,"onoff");
				this.isTarget =  GUILayout.Toggle(this.isTarget,"isAOE");

				GUILayout.Label("type");
				this.type = GUILayout.TextField(this.type);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				this.bindEffect =  GUILayout.Toggle(this.bindEffect,"bind effect");
				GUILayout.Label("parent");
				this.parent = GUILayout.TextField(this.parent);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("time");
				this.time = EditorGUILayout.FloatField(this.time);
				GUILayout.EndHorizontal();
			}

			{
				if( GUILayout.Button("copy-transform") )
				{
					if( Selection.activeGameObject != null )
					{
						this.offset = Selection.activeGameObject.transform.localPosition;
						this.rotate = Selection.activeGameObject.transform.localRotation.eulerAngles;
						this.size = Selection.activeGameObject.transform.localScale;
					}
				}
				this.offset = EditorGUILayout.Vector3Field("position" , this.offset);
				this.rotate = EditorGUILayout.Vector3Field("rotation" , this.rotate);
				this.size = EditorGUILayout.Vector3Field("size" , this.size);
			}

			GUILayout.EndVertical();
		}
#endif
	}
	
	//effect
	[System.Serializable]
	public class Effect : System.ICloneable
	{
		public string	id	= string.Empty;			//id of resources
		public float 	time = -1;					//cost time , time is Negative 
		public bool		onoff = false;				//on or off
		public Vector3	offset	= Vector3.zero;		//offset pos
		public Vector3	rotate	= Vector3.zero;		//rotate
		public Vector3	scale	= Vector3.one;		//scale
		public string	parent	= string.Empty;		//parent
		public bool bullet = false;					//is bullet


		public bool		isDependPosition	= true;	//
		public bool		isDependRotation	= true; //
		public bool		isDependScale		= true; //
		
		public object Clone()
		{
			Effect ret = new Effect();
			ret.id		= id;
			ret.time = time;
			ret.onoff	= onoff;
			ret.offset	= offset;
			ret.rotate	= rotate;
			ret.scale	= scale;
			ret.parent	= parent;
			ret.bullet = bullet;

			ret.isDependPosition	= isDependPosition;
			ret.isDependRotation	= isDependRotation;
			ret.isDependScale		= isDependScale;
			return ret;
		}
#if UNITY_EDITOR
		public void Draw()
		{
			GUILayout.BeginVertical();
			GUILayout.Label("------------------- Effect -------------------");
			{
				GUILayout.BeginHorizontal();
				this.onoff = GUILayout.Toggle(this.onoff,"onoff");
				this.bullet = GUILayout.Toggle(this.bullet,"bullet");
				GUILayout.Label("id"); 
				this.id = GUILayout.TextField(this.id);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("time");
				this.time = EditorGUILayout.FloatField(this.time);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("parent");
				this.parent = GUILayout.TextField(parent);
				GUILayout.EndHorizontal();
			}

			{
				if( GUILayout.Button("copy-transform") )
				{
					this.offset = Selection.activeGameObject.transform.localPosition;
					this.rotate = Selection.activeGameObject.transform.localRotation.eulerAngles;
					this.scale = Selection.activeGameObject.transform.localScale;
				}
				this.offset = EditorGUILayout.Vector3Field("position" , this.offset);
				this.rotate = EditorGUILayout.Vector3Field("rotation" , this.rotate);
				this.scale = EditorGUILayout.Vector3Field("scale" , this.scale);
			}

			GUILayout.EndVertical();
		}
#endif
	}

	//message
	[System.Serializable]
	public class Message : System.ICloneable
	{
		[SerializeField]
		public string m_Function = string.Empty;
		[SerializeField]
		public string m_Args = string.Empty;

		public object Clone()
		{
			Message msg = new Message();
			msg.m_Function = m_Function;
			msg.m_Args = m_Args;
			return msg;
		}

#if UNITY_EDITOR
		public void Draw( ActionObject.Event ev )
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("- Message"))
			{
				ev.messages.Remove(this);
			}
			string desc = "method:move";
			switch(m_Function)
			{
				case "move":
					desc = "Parameter:speed;time";
					break;
			}
			GUILayout.Label(desc);
			GUILayout.Label("Function");
			this.m_Function = GUILayout.TextField(this.m_Function);
			GUILayout.Label("arg1;arg2;arg3");
			this.m_Args = GUILayout.TextField(this.m_Args);
			GUILayout.EndHorizontal();
		}
#endif
	}

	//event
	[System.Serializable]
	public class Event
	{
		[SerializeField]
		public string sound = "";

		[SerializeField]
		public float time = 0f;

		[SerializeField]
		public List<Message> messages = new List<Message>();
		
		[SerializeField]
		public Hit hit = new Hit();

		[SerializeField]
		public Effect effect = new Effect();

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

		//draw
		public void Draw( ActionObject aco )
		{
			GUILayout.BeginVertical();
			GUILayout.Label("****************** event ******************");
			if (GUILayout.Button("- Event"))
			{
				aco.m_Events.Remove(this);
			}

			float rateTime = time/aco.m_Time;
			TimeLine(rateTime);
			rateTime = GUILayout.HorizontalSlider(rateTime,0,1f,GUILayout.Width(window_size.x));
			time = aco.m_Time * rateTime;

			{
				GUILayout.BeginHorizontal();
				GUILayout.Label("sound");
				this.sound = GUILayout.TextField(this.sound);
				GUILayout.Label("time");
				this.time = EditorGUILayout.FloatField(this.time);
				GUILayout.EndHorizontal();
			}
			{
				hit.Draw();
			}
			{
				effect.Draw();
			}
			{
				GUILayout.BeginVertical();
				GUILayout.Label("------------------- Message -------------------");
				if(GUILayout.Button("+ Message"))
				{
					Message msg = new Message();
					this.messages.Add(msg);
				}
				for( int i = 0 ; i<messages.Count ;i++ )
				{
					messages[i].Draw(this);
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndVertical();
		}
#endif
	}
}
