
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

//event animation
public class ActionEvent_Animation : ActionEvent
{
	public enum CMD
	{
		Play = 0,	//play
		CrossFade,	//fade cross
	}

	public enum AnimationType
	{
		Animator = 0,
		Animation,
	}

	public CMD mCMD = 0;
	public AnimationType mAnimationType = 0;

	public string mAniName = string.Empty;
	public WrapMode mAniWarp = WrapMode.Once;
	 
	public ActionEvent_Animation()
		:base()
	{
		mActionType = ActionObject.ActionType.Animation;
	}
	
	public object Clone()
	{
		ActionEvent_Animation ret = new ActionEvent_Animation();
		ret.mActionType = mActionType;
		ret.mTime = mTime;
		ret.mCMD = mCMD;
		ret.mAnimationType = mAnimationType;
		ret.mAniName = mAniName;
		ret.mAniWarp = mAniWarp;
		return ret;
	}

	public override void ReadJson(Dictionary<string,object> _data)
	{
		base.ReadJson(_data);

		if(_data.ContainsKey("mCMD"))
		{
			mCMD = (CMD)int.Parse(_data["mCMD"].ToString());
		}

		if(_data.ContainsKey("mAnimationType"))
		{
			mAnimationType = (AnimationType)int.Parse(_data["mAnimationType"].ToString());
		}

		if(_data.ContainsKey("mAniName"))
		{
			mAniName = _data["mAniName"].ToString();
		}

		if(_data.ContainsKey("mAniWarp"))
		{
			mAniWarp = (WrapMode)int.Parse(_data["mAniWarp"].ToString());
		}
	}

	public override void Read(BinaryReader br)
	{
		base.Read(br);
		mCMD = (CMD)br.ReadInt32();
		mAnimationType = (AnimationType)br.ReadInt32();
		mAniName = br.ReadString();
		mAniWarp = (WrapMode)br.ReadInt32();
	}

	public override void WriteJson(Dictionary<string,object> _data)
	{
		base.WriteJson(_data);
		_data.Add("mCMD",((int)mCMD).ToString());
		_data.Add("mAnimationType",((int)mAnimationType).ToString());
		_data.Add("mAniName",mAniName);
		_data.Add("mAniWarp",((int)mAniWarp).ToString());
	}

	public override void Write(BinaryWriter bw)
	{
		base.Write(bw);
		bw.Write((int)mCMD);
		bw.Write((int)mAnimationType);
		bw.Write(mAniName);
		bw.Write((int)mAniWarp);
	}

	public override bool Do()
	{
		if(mAnimationType == AnimationType.Animator)
		{
			Animator animator = mActionObject.mCustomObject.GetAnimator();
			if(animator == null)
			{
				Debug.LogError("Animator can't be found.");
				return true;
			}
			switch(mCMD)
			{
				case CMD.Play:
					animator.Play(mAniName);
					break;
				case CMD.CrossFade:
					animator.CrossFade(mAniName,0.3f);
					break;
			}
		}
		else if(mAnimationType == AnimationType.Animation)
		{
			Animation animation = mActionObject.mCustomObject.GetAnimation();
			if(animation == null)
			{
				Debug.LogError("Animator can't be found.");
				return true;
			}
			AnimationState ani_state = animation[mAniName];
			ani_state.wrapMode = mAniWarp;
			switch(mCMD)
			{
				case CMD.Play:
					animation.Play(mAniName);
					break;
				case CMD.CrossFade:
					animation.CrossFade(mAniName,0.3f);
					break;
			}
		}
		return true;
	}

#if UNITY_EDITOR
	//draw
	// private string[] cmd_popup = new string[3]{"Play","CrossFade","Stop"};
	// private string[] animationType_popup = new string[2]{"Animator","Animation"};
	public override void Draw( ActionObject aco )
	{
		base.Draw(aco);

		GUILayout.BeginVertical();
		mCMD = (CMD)EditorGUILayout.EnumPopup(mCMD);
		mAnimationType = (AnimationType)EditorGUILayout.EnumPopup(mAnimationType);
		if(mAnimationType == AnimationType.Animation)
		{
			mAniWarp = (WrapMode)EditorGUILayout.EnumPopup("WrapMode", mAniWarp);
		}

		switch((CMD)mCMD)
		{
			case CMD.Play:
			case CMD.CrossFade:
				GUILayout.BeginHorizontal();
				GUILayout.Label("Animation Name");
				this.mAniName = GUILayout.TextField(this.mAniName);
				GUILayout.EndHorizontal();
				break;
		}
		GUILayout.EndVertical();
	}
#endif
}