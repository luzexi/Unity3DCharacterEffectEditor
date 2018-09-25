using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


//action event use prefab
public class ActionEvent_Prefab : ActionEvent
{
	public enum CMD
	{
		New = 0,
		Deactivate,
		Activate,
		Destroy,
	}
	public CMD mCMD = 0;
	public string	mFileName	= string.Empty;			//file name of resources.
	public int mId = -1;	//id
	// public float 	mDuration = -1;					//cost time , time is Negative 
	public Vector3	mPosition	= Vector3.zero;		//local postion
	public Quaternion	mRotation	= Quaternion.identity;		//local rotation
	public Vector3	mScale	= Vector3.one;		//local scale
	public string	mParent	= string.Empty;		//parent

	public ActionEvent_Prefab()
		:base()
	{
		mActionType = ActionObject.ActionType.Prefab;
	}

	public object Clone()
	{
		ActionEvent_Prefab ret = new ActionEvent_Prefab();
		ret.mActionType = mActionType;
		ret.mTime = mTime;
		ret.mFileName = mFileName;
		// ret.mDuration = mDuration;
		ret.mPosition	= mPosition;
		ret.mRotation	= mRotation;
		ret.mScale	= mScale;
		ret.mParent	= mParent;
		return ret;
	}

	public override void ReadJson(Dictionary<string,object> _data)
	{
		base.ReadJson(_data);

		if(_data.ContainsKey("mCMD"))
		{
			mCMD = (CMD)int.Parse(_data["mCMD"].ToString());
		}
		if(_data.ContainsKey("mFileName"))
		{
			mFileName = _data["mFileName"].ToString();
		}
		if(_data.ContainsKey("mPositionX"))
		{
			mPosition.x = float.Parse(_data["mPositionX"].ToString());
		}
		if(_data.ContainsKey("mPositionY"))
		{
			mPosition.y = float.Parse(_data["mPositionY"].ToString());
		}
		if(_data.ContainsKey("mPositionZ"))
		{
			mPosition.z = float.Parse(_data["mPositionZ"].ToString());
		}
		if(_data.ContainsKey("mRotationX"))
		{
			mRotation.x = float.Parse(_data["mRotationX"].ToString());
		}
		if(_data.ContainsKey("mRotationY"))
		{
			mRotation.y = float.Parse(_data["mRotationY"].ToString());
		}
		if(_data.ContainsKey("mRotationZ"))
		{
			mRotation.z = float.Parse(_data["mRotationZ"].ToString());
		}
		if(_data.ContainsKey("mRotationW"))
		{
			mRotation.w = float.Parse(_data["mRotationW"].ToString());
		}
		if(_data.ContainsKey("mScaleX"))
		{
			mScale.x = float.Parse(_data["mScaleX"].ToString());
		}
		if(_data.ContainsKey("mScaleY"))
		{
			mScale.y = float.Parse(_data["mScaleY"].ToString());
		}
		if(_data.ContainsKey("mScaleZ"))
		{
			mScale.z = float.Parse(_data["mScaleZ"].ToString());
		}
		if(_data.ContainsKey("mParent"))
		{
			mParent = _data["mParent"].ToString();
		}
	}

	public override void Read(BinaryReader br)
	{
		base.Read(br);

		mCMD = (CMD)br.ReadInt32();
		mFileName = br.ReadString();
		// mDuration = br.ReadSingle();
		mPosition.x = br.ReadSingle();
		mPosition.y = br.ReadSingle();
		mPosition.z = br.ReadSingle();
		mRotation.x = br.ReadSingle();
		mRotation.y = br.ReadSingle();
		mRotation.z = br.ReadSingle();
		mRotation.w = br.ReadSingle();
		mScale.x = br.ReadSingle();
		mScale.y = br.ReadSingle();
		mScale.z = br.ReadSingle();
		mParent = br.ReadString();
	}

	public override void WriteJson(Dictionary<string,object> _data)
	{
		base.WriteJson(_data);
		_data.Add("mCMD",((int)mCMD).ToString());
		_data.Add("mFileName",mFileName);
		_data.Add("mPositionX",mPosition.x.ToString());
		_data.Add("mPositionY",mPosition.y.ToString());
		_data.Add("mPositionZ",mPosition.z.ToString());
		_data.Add("mRotationX",mRotation.x.ToString());
		_data.Add("mRotationY",mRotation.y.ToString());
		_data.Add("mRotationZ",mRotation.z.ToString());
		_data.Add("mRotationW",mRotation.w.ToString());
		_data.Add("mScaleX",mScale.x.ToString());
		_data.Add("mScaleY",mScale.y.ToString());
		_data.Add("mScaleZ",mScale.z.ToString());
		_data.Add("mParent",mParent);
	}

	public override void Write(BinaryWriter bw)
	{
		base.Write(bw);
		
		bw.Write((int)mCMD);
		bw.Write(mFileName);
		// bw.Write(mDuration);
		bw.Write(mPosition.x);
		bw.Write(mPosition.y);
		bw.Write(mPosition.z);
		bw.Write(mRotation.x);
		bw.Write(mRotation.y);
		bw.Write(mRotation.z);
		bw.Write(mRotation.w);
		bw.Write(mScale.x);
		bw.Write(mScale.y);
		bw.Write(mScale.z);
		bw.Write(mParent);
	}

	public override bool Do()
	{
		GameObject obj;
		switch(mCMD)
		{
			case CMD.New:
				obj = GameObject.Instantiate( Resources.Load( mFileName) ) as GameObject;
				obj.transform.parent = mActionObject.mCustomObject.GetTransform().Find(mParent);
				obj.transform.localPosition = mPosition;
				obj.transform.localRotation = mRotation;
				obj.transform.localScale = mScale;
				if(mId > 0)
				{
					mActionObject.AddPrefab(mId,obj);
				}
				break;
			case CMD.Deactivate:
				if(mId > 0)
				{
					obj = mActionObject.GetPrefab(mId);
					obj.SetActive(false);
				}
				break;
			case CMD.Activate:
				if(mId > 0)
				{
					obj = mActionObject.GetPrefab(mId);
					obj.SetActive(true);
				}
				break;
			case CMD.Destroy:
				if(mId > 0)
				{
					obj = mActionObject.GetPrefab(mId);
					GameObject.Destroy(obj);
					mActionObject.RemovePrefab(mId);
				}
				break;
		}
		return true;
	}

#if UNITY_EDITOR

	// private string[] cmd_popup = new string[4]{"New", "Deactivate", "Activate", "Destroy"};
	private Vector4 temp_roation = Vector4.zero;
	public override void Draw(ActionObject aco)
	{
		base.Draw(aco);

		GUILayout.BeginVertical();
		mCMD = (CMD)EditorGUILayout.EnumPopup(mCMD);

		if(mCMD == CMD.New)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("File Name"); 
			this.mFileName = GUILayout.TextField(this.mFileName);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("ID (Ignore, if don't wanna control it)");
			this.mId = EditorGUILayout.IntField(this.mId);
			GUILayout.EndHorizontal();

			// GUILayout.BeginHorizontal();
			// GUILayout.Label("Duration");
			// this.mDuration = EditorGUILayout.FloatField(this.mDuration);
			// GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Parent");
			this.mParent = GUILayout.TextField(mParent);
			if( GUILayout.Button("copy-parent") )
			{
				if( Selection.activeGameObject != null )
				{
					string str = Selection.activeGameObject.transform.name;
					Transform trans = Selection.activeGameObject.transform.parent;
					for(; trans.parent != null;)
					{
						str = trans.parent.name + "/" + str;
						trans = trans.parent;
					}
					this.mParent = str;
				}
			}
			GUILayout.EndHorizontal();

			if( GUILayout.Button("copy-transform") )
			{
				this.mPosition = Selection.activeGameObject.transform.localPosition;
				this.mRotation = Selection.activeGameObject.transform.localRotation;
				this.mScale = Selection.activeGameObject.transform.localScale;
			}
			this.mPosition = EditorGUILayout.Vector3Field("Position" , this.mPosition);
			temp_roation = EditorGUILayout.Vector4Field("Rotation" , temp_roation);
			if(temp_roation.x != mRotation.x || temp_roation.y != mRotation.y || temp_roation.z == mRotation.z || temp_roation.w != mRotation.w)
			{
				mRotation = new Quaternion(temp_roation.x, temp_roation.y, temp_roation.z, temp_roation.w);
			}
			this.mScale = EditorGUILayout.Vector3Field("Scale" , this.mScale);
		}
		else if(mCMD == CMD.Deactivate)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("ID (which set before)");
			this.mId = EditorGUILayout.IntField(this.mId);
			GUILayout.EndHorizontal();
		}
		else if(mCMD == CMD.Activate)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("ID (which set before)");
			this.mId = EditorGUILayout.IntField(this.mId);
			GUILayout.EndHorizontal();
		}
		else if(mCMD == CMD.Destroy)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("ID (which set before)");
			this.mId = EditorGUILayout.IntField(this.mId);
			GUILayout.EndHorizontal();
		}

		GUILayout.EndVertical();
	}
#endif
}