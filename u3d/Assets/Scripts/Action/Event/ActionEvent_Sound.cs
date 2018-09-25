
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

//event
public class ActionEvent_Sound : ActionEvent
{
	public enum CMD
	{
		Play = 0,	//play
		Stop,	//stop
	}

	public CMD mCMD = 0;

	public string mSoundName = string.Empty;

	public ActionEvent_Sound()
		:base()
	{
		mActionType = ActionObject.ActionType.Sound;
	}

	public object Clone()
	{
		ActionEvent_Sound ret = new ActionEvent_Sound();
		ret.mActionType = mActionType;
		ret.mTime = mTime;
		ret.mCMD = mCMD;
		ret.mSoundName = mSoundName;
		return ret;
	}

	public override void ReadJson(Dictionary<string,object> _data)
	{
		base.ReadJson(_data);

		if(_data.ContainsKey("mCMD"))
		{
			mCMD = (CMD)int.Parse(_data["mCMD"].ToString());
		}
		if(_data.ContainsKey("mSoundName"))
		{
			mSoundName = _data["mSoundName"].ToString();
		}
	}

	public override void Read(BinaryReader br)
	{
		base.Read(br);

		mCMD = (CMD)br.ReadInt32();
		mSoundName = br.ReadString();
	}

	public override void WriteJson(Dictionary<string,object> _data)
	{
		base.WriteJson(_data);
		_data.Add("mCMD",((int)mCMD).ToString());
		_data.Add("mSoundName",mSoundName);
	}

	public override void Write(BinaryWriter bw)
	{
		base.Write(bw);

		bw.Write((int)mCMD);
		bw.Write(mSoundName);
	}

	public override bool Do()
	{
		switch(mCMD)
		{
			case CMD.Play:
				// SoundEffect.instance.PlaySfx(mSoundName);
				break;
			case CMD.Stop:
				// SoundEffect.instance.StopSfx();
				break;
		}
		return true;
	}

#if UNITY_EDITOR
	//draw
	// private string[] cmd_popup = new string[2]{"Play","Stop"};
	public override void Draw( ActionObject aco )
	{
		base.Draw(aco);

		GUILayout.BeginVertical();
		mCMD = (CMD)EditorGUILayout.EnumPopup(mCMD);

		switch(mCMD)
		{
			case CMD.Play:
				GUILayout.BeginHorizontal();
				GUILayout.Label("Sound Name");
				this.mSoundName = GUILayout.TextField(this.mSoundName);
				GUILayout.EndHorizontal();
				break;
			case CMD.Stop:
				break;
		}
		GUILayout.EndVertical();
	}
#endif

}