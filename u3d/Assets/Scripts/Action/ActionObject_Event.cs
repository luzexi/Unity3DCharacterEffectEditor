

using UnityEngine;
using System;
using System.IO;
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
		public bool mHiden = false;	//editor

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

		public void Read(BinaryReader br)
		{
			onoff = (bool)br.ReadBoolean();
			type = br.ReadString();
			isTarget = (bool)br.ReadBoolean();
			bindEffect = (bool)br.ReadBoolean();
			size.x = br.ReadSingle();
			size.y = br.ReadSingle();
			size.z = br.ReadSingle();
			rotate.x = br.ReadSingle();
			rotate.y = br.ReadSingle();
			rotate.z = br.ReadSingle();
			offset.x = br.ReadSingle();
			offset.y = br.ReadSingle();
			offset.z = br.ReadSingle();
			parent = br.ReadString();
			time = br.ReadSingle();
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(onoff);
			bw.Write(type);
			bw.Write(isTarget);
			bw.Write(bindEffect);
			bw.Write(size.x);
			bw.Write(size.y);
			bw.Write(size.z);
			bw.Write(rotate.x);
			bw.Write(rotate.y);
			bw.Write(rotate.z);
			bw.Write(offset.x);
			bw.Write(offset.y);
			bw.Write(offset.z);
			bw.Write(parent);
			bw.Write(time);
		}

#if UNITY_EDITOR
		public void Draw()
		{
			GUILayout.BeginVertical();
			if(mHiden)
			{
				if (GUILayout.Button(">>>Hit"))
				{
					mHiden = false;
				}
				GUILayout.EndVertical();
				return;
			}
			else if (GUILayout.Button("<<<Hit"))
			{
				mHiden = true;
			}
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
		public string	name	= string.Empty;			//id of resources
		public float 	time = -1;					//cost time , time is Negative 
		public bool		onoff = false;				//on or off
		public Vector3	offset	= Vector3.zero;		//offset pos
		public Vector3	rotate	= Vector3.zero;		//rotate
		public Vector3	scale	= Vector3.one;		//scale
		public string	parent	= string.Empty;		//parent
		public bool bullet = false;					//is bullet
		public bool mHiden = false;	//editor


		public bool		isDependPosition	= true;	//
		public bool		isDependRotation	= true; //
		public bool		isDependScale		= true; //
		
		public object Clone()
		{
			Effect ret = new Effect();
			ret.name = name;
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

		public void Read(BinaryReader br)
		{
			name = br.ReadString();
			time = br.ReadSingle();
			onoff = (bool)br.ReadBoolean();
			offset.x = br.ReadSingle();
			offset.y = br.ReadSingle();
			offset.z = br.ReadSingle();
			rotate.x = br.ReadSingle();
			rotate.y = br.ReadSingle();
			rotate.z = br.ReadSingle();
			scale.x = br.ReadSingle();
			scale.y = br.ReadSingle();
			scale.z = br.ReadSingle();
			parent = br.ReadString();
			bullet = (bool)br.ReadBoolean();
			isDependPosition = (bool)br.ReadBoolean();
			isDependRotation = (bool)br.ReadBoolean();
			isDependScale = (bool)br.ReadBoolean();
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(name);
			bw.Write(time);
			bw.Write(onoff);
			bw.Write(offset.x);
			bw.Write(offset.y);
			bw.Write(offset.z);
			bw.Write(rotate.x);
			bw.Write(rotate.y);
			bw.Write(rotate.z);
			bw.Write(scale.x);
			bw.Write(scale.y);
			bw.Write(scale.z);
			bw.Write(parent);
			bw.Write(bullet);
			bw.Write(isDependPosition);
			bw.Write(isDependRotation);
			bw.Write(isDependScale);
		}

#if UNITY_EDITOR
		public void Draw()
		{
			GUILayout.BeginVertical();
			if(mHiden)
			{
				if (GUILayout.Button(">>>Effect"))
				{
					mHiden = false;
				}
				GUILayout.EndVertical();
				return;
			}
			else if (GUILayout.Button("<<<Effect"))
			{
				mHiden = true;
			}
			GUILayout.Label("------------------- Effect -------------------");
			{
				GUILayout.BeginHorizontal();
				this.onoff = GUILayout.Toggle(this.onoff,"onoff");
				this.bullet = GUILayout.Toggle(this.bullet,"bullet");
				GUILayout.Label("name"); 
				this.name = GUILayout.TextField(this.name);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("time");
				this.time = EditorGUILayout.FloatField(this.time);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("parent");
				this.parent = GUILayout.TextField(parent);
				if( GUILayout.Button("copy-parent") )
				{
					if( Selection.activeGameObject != null )
					{
						string str = Selection.activeGameObject.transform.name;
						Transform trans = Selection.activeGameObject.transform.parent;
						for(; trans.GetComponent<ActionCustomObject>() == null;)
						{
							str = trans.name + "/" + str;
							trans = trans.parent;
						}
						this.parent = str;
					}
				}
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
		public ActionMessageCommand m_Function = ActionMessageCommand.None;
		[SerializeField]
		public string m_Args = string.Empty;

		public object Clone()
		{
			Message msg = new Message();
			msg.m_Function = m_Function;
			msg.m_Args = m_Args;
			return msg;
		}

		public void Read(BinaryReader br)
		{
			m_Function = (ActionMessageCommand)br.ReadInt32();
			m_Args = br.ReadString();
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write((int)m_Function);
			bw.Write(m_Args);
		}

#if UNITY_EDITOR
		public void Draw( ActionObject.Event ev )
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("- Message"))
			{
				ev.messages.Remove(this);
			}
			m_Function = (ActionMessageCommand)EditorGUILayout.EnumPopup("Command",m_Function);
			string desc = "Parameter";
			switch(m_Function)
			{
				case ActionMessageCommand.Move:
					desc = "Parameter:speed;time";
					break;
				case ActionMessageCommand.Walkback:
					desc = "Parameter:speed;time";
					break;
			}
			GUILayout.Label(desc);
			// GUILayout.Label("arg1;arg2;arg3...");
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
		public string m_AniName = string.Empty;

		[SerializeField]
		public WrapMode m_AniWarp = WrapMode.Once;

		[SerializeField]
		public string sound = string.Empty;

		[SerializeField]
		public float time = 0f;
		
		[SerializeField]
		public Hit hit = new Hit();

		[SerializeField]
		public Effect effect = new Effect();

		[SerializeField]
		public List<Message> messages = new List<Message>();

		[SerializeField]
		public bool mHiden = false;	//editor

		public void Read(BinaryReader br)
		{
			m_AniName = br.ReadString();
			m_AniWarp = (WrapMode)br.ReadInt32();
			sound = br.ReadString();
			time = br.ReadSingle();
			hit.Read(br);
			effect.Read(br);
			int count = br.ReadInt32();
			messages.Clear();
			for(int i = 0 ; i<count ; i++)
			{
				Message ms = new Message();
				ms.Read(br);
				messages.Add(ms);
			}
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(m_AniName);
			bw.Write((int)m_AniWarp);
			bw.Write(sound);
			bw.Write(time);
			hit.Write(bw);
			effect.Write(bw);
			bw.Write(messages.Count);
			for(int i = 0 ; i<messages.Count ; i++)
			{
				messages[i].Write(bw);
			}
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

		//draw
		public void Draw( ActionObject aco )
		{
			GUILayout.BeginVertical();
			// GUILayout.Label("****************** event ******************");

			// if (GUILayout.Button("delete Event"))
			// {
			// 	if(EditorUtility.DisplayDialog("Remove Event","Are you sure to remove Event?","Remove","Cancel"))
			// 	{
			// 		aco.m_Events.Remove(this);
			// 		aco.mCurrentEvent = null;
			// 	}
			// }

			float rateTime = time/aco.m_Time;
			TimeLine(rateTime);
			rateTime = GUILayout.HorizontalSlider(rateTime,0,1f,GUILayout.Width(window_size.x));
			time = aco.m_Time * rateTime;

			if(mHiden)
			{
				if (GUILayout.Button(">>>Event"))
				{
					mHiden = false;
				}
				GUILayout.EndVertical();
				return;
			}
			else
			{
				if (GUILayout.Button("<<<Event"))
				{
					mHiden = true;
				}
			}

			GUILayout.BeginHorizontal();
			GUILayout.Label("Animation Name");
			this.m_AniName = GUILayout.TextField(this.m_AniName);
			this.m_AniWarp = (WrapMode)EditorGUILayout.EnumPopup("Warp", m_AniWarp);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("sound");
			this.sound = GUILayout.TextField(this.sound);
			GUILayout.Label("time");
			this.time = EditorGUILayout.FloatField(this.time);
			GUILayout.EndHorizontal();
			

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
