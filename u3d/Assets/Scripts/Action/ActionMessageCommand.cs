using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum ActionMessageCommand
{
	None,
	Move,
	Walkback,
}

public class ActionCommand
{
	public static ActionCommand CallCommand(ICustomObject _obj, ActionMessageCommand _command, string args)
	{
		ActionCommand cmd = null;

		switch(_command)
		{
			case ActionMessageCommand.Move:
				cmd = new ActionCommandMove();
				break;
		}
		string[] _args = args.Split(';');
		cmd.Load(_obj, _args);
		return cmd;
	}

	public virtual void Load(ICustomObject _obj, string[] args)
	{
		//
	}

	public virtual bool Update()
	{
		return true;
	}
}


public class ActionCommandMove : ActionCommand
{
	public ICustomObject mObj;

	public float mDurationTime;
	public float mSpeed;

	private float mStartTime;
	private CharacterController mCharacterController;

	public override void Load(ICustomObject _obj, string[] args)
	{
		mObj = _obj;
		mSpeed = float.Parse(args[0]);
		mDurationTime = float.Parse(args[1]);

		Init();
	}

	void Init()
	{
		mStartTime = Time.time;
		mCharacterController = mObj.GetCharacterController();
	}

	public override bool Update()
	{
		float dif = Time.time - mStartTime;
		if(dif > mDurationTime) return false;

		mCharacterController.Move(mObj.GetTransform().forward*mSpeed*Time.deltaTime);
		return true;
	}
}
