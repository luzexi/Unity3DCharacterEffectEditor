
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

//event character control
public class ActionEvent_CharacterControl : ActionEvent
{
	public enum CMD
	{
		MoveForward = 0,	//move forward
		MoveRight,
		Jump,
	}

	public enum EaseType
	{
		None = 0,
		Linear,
		EaseInCubic,
		EaseOutCubic,
		EaseInOutCubic,
	}

	public CMD mCMD = 0;
	public float mSpeed = 1;
	public float mJumpSpeed = 1;
	public float mDuration = 1;
	public EaseType mEaseType = 0;

	private CharacterController mCharacterController;
	private float mStartTime;

	public ActionEvent_CharacterControl()
		:base()
	{
		mActionType = ActionObject.ActionType.CharacterControl;
	}
	
	public object Clone()
	{
		ActionEvent_CharacterControl ret = new ActionEvent_CharacterControl();
		ret.mActionType = mActionType;
		ret.mTime = mTime;
		ret.mCMD = mCMD;
		ret.mSpeed = mSpeed;
		ret.mJumpSpeed = mJumpSpeed;
		ret.mDuration = mDuration;
		ret.mEaseType = mEaseType;
		return ret;
	}

	public override void ReadJson(Dictionary<string,object> _data)
	{
		base.ReadJson(_data);

		if(_data.ContainsKey("mCMD"))
		{
			mCMD = (CMD)int.Parse(_data["mCMD"].ToString());
		}
		if(_data.ContainsKey("mSpeed"))
		{
			mSpeed = float.Parse(_data["mSpeed"].ToString());
		}
		if(_data.ContainsKey("mJumpSpeed"))
		{
			mJumpSpeed = float.Parse(_data["mJumpSpeed"].ToString());
		}
		if(_data.ContainsKey("mDuration"))
		{
			mDuration = float.Parse(_data["mDuration"].ToString());
		}
		if(_data.ContainsKey("mEaseType"))
		{
			mEaseType = (EaseType)int.Parse(_data["mEaseType"].ToString());
		}
	}

	public override void Read(BinaryReader br)
	{
		base.Read(br);

		mCMD = (CMD)br.ReadInt32();
		mSpeed = br.ReadSingle();
		mJumpSpeed = br.ReadSingle();
		mDuration = br.ReadSingle();
		mEaseType = (EaseType)br.ReadInt32();
	}

	public override void WriteJson(Dictionary<string,object> _data)
	{
		base.WriteJson(_data);
		_data.Add("mCMD",((int)mCMD).ToString());
		_data.Add("mSpeed",mSpeed.ToString());
		_data.Add("mJumpSpeed",mJumpSpeed.ToString());
		_data.Add("mDuration",mDuration.ToString());
	}

	public override void Write(BinaryWriter bw)
	{
		base.Write(bw);

		bw.Write((int)mCMD);
		bw.Write(mSpeed);
		bw.Write(mJumpSpeed);
		bw.Write(mDuration);
		bw.Write((int)mEaseType);
	}

	public override void OnEnter()
	{
		mCharacterController = mActionObject.mCustomObject.GetCharacterController();
		if(mCharacterController == null)
		{
			Debug.LogError("CharacterController can't be found.");
			return;
		}
		mStartTime = Time.time;
	}

	public override bool Do()
	{
		float difTime= Time.time - mStartTime;
		if(difTime > mDuration)
		{
			return true;
		}
		float rate = difTime/mDuration;
		float speed = mSpeed;
		switch(mEaseType)
		{
			case EaseType.None:
				speed = mSpeed;
				break;
			// case EaseType.Linear:
			// 	speed = mSpeed - CMathCurve.Linear(rate,0,1,1)*mSpeed;
			// 	break;
			// case EaseType.EaseInCubic:
			// 	speed = mSpeed - CMathCurve.CubicIn(rate,0,1,1)*mSpeed;
			// 	break;
			// case EaseType.EaseOutCubic:
			// 	speed = mSpeed - CMathCurve.CubicOut(rate,0,1,1)*mSpeed;
			// 	break;
			// case EaseType.EaseInOutCubic:
			// 	speed = mSpeed - CMathCurve.CubicInOut(rate,0,1,1)*mSpeed;
			// 	break;
		}
		// Debug.LogError("in character move speed is " + speed);
		switch(mCMD)
		{
			case CMD.MoveForward:
				mCharacterController.Move((mCharacterController.transform.forward*mSpeed + Vector3.up *-1f) * Time.deltaTime);
				break;
			case CMD.MoveRight:
				mCharacterController.Move((mCharacterController.transform.right*mSpeed + Vector3.up * -1f) * Time.deltaTime);
				break;
			case CMD.Jump:
				if(rate < 0.5f)
				{
					mCharacterController.Move((mCharacterController.transform.forward*mSpeed + Vector3.up * mJumpSpeed * 1f) * Time.deltaTime);
				}
				else
				{
					mCharacterController.Move((mCharacterController.transform.forward*mSpeed + Vector3.up * mJumpSpeed * -1f) * Time.deltaTime);
				}
				break;
		}
		return false;
	}

#if UNITY_EDITOR
	//draw
	// private string[] cmd_popup = new string[3]{"MoveForward","MoveRight","Jump"};
	// private string[] ease_type_popup = new string[5]{"None","Line", "EaseInCubic", "EaseOutCubic", "EaseInOutCubic"};
	public override void Draw( ActionObject aco )
	{
		base.Draw(aco);

		GUILayout.BeginVertical();

		mCMD = (CMD)EditorGUILayout.EnumPopup(mCMD);

		GUILayout.BeginHorizontal();
		GUILayout.Label("Speed");
		this.mSpeed = EditorGUILayout.FloatField(this.mSpeed);
		GUILayout.EndHorizontal();

		if(mCMD == CMD.Jump)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Jump Speed");
			this.mJumpSpeed = EditorGUILayout.FloatField(this.mJumpSpeed);
			GUILayout.EndHorizontal();
		}

		GUILayout.BeginHorizontal();
		GUILayout.Label("Duration");
		this.mDuration = EditorGUILayout.FloatField(this.mDuration);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Ease Type");
		this.mEaseType = (EaseType)EditorGUILayout.EnumPopup(mEaseType);
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
	}
#endif
}