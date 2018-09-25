
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

//event camera
public class ActionEvent_Camera : ActionEvent
{
	public enum CMD
	{
		Shake = 0,
	}

	public CMD mCMD = 0;

	public ActionEvent_Camera()
		:base()
	{
		mActionType = ActionObject.ActionType.Camera;
	}
	
	public object Clone()
	{
		ActionEvent_Camera ret = new ActionEvent_Camera();
		ret.mActionType = mActionType;
		ret.mTime = mTime;
		ret.mCMD = mCMD;
		return ret;
	}

	public override void ReadJson(Dictionary<string,object> _data)
	{
		base.ReadJson(_data);

		if(_data.ContainsKey("mCMD"))
		{
			mCMD = (CMD)int.Parse(_data["mCMD"].ToString());
		}
	}

	public override void Read(BinaryReader br)
	{
		base.Read(br);
		mCMD = (CMD)br.ReadInt32();
	}

	public override void WriteJson(Dictionary<string,object> _data)
	{
		base.WriteJson(_data);
		_data.Add("mCMD",((int)mCMD).ToString());
	}

	public override void Write(BinaryWriter bw)
	{
		base.Write(bw);
		bw.Write((int)mCMD);
	}

	public override bool Do()
	{
		//
		return true;
	}

#if UNITY_EDITOR
	//draw
	// private string[] cmd_popup = new string[1]{"Shake"};
	public override void Draw( ActionObject aco )
	{
		base.Draw(aco);

		GUILayout.BeginVertical();
		mCMD = (CMD)EditorGUILayout.EnumPopup(mCMD);
		GUILayout.EndVertical();
	}
#endif
}