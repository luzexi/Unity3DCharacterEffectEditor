

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
	private ActionObject mActionObject = null;

	//temp
	public ICustomObject mTarget = null;	//target
	private ICustomObject mOwner = null;		//owner
	// private HitData m_HitData = null;	//hit data
	private float mStartTime = 0;		//start time
	public bool mIsOver = false;	//is over
	private int mCurrentIndex = 0;

	public void StartAction( ICustomObject obj, ActionObject _actionObj )
	{
		this.mOwner = obj;
		this.mStartTime = Time.time;
		mActionObject = _actionObj;
		mIsOver = false;
		mCurrentIndex = 0;
		Init();
	}

	void Init()
	{
		mActionObject.mCustomObject = mOwner;
		for(int i = 0 ; i<mActionObject.mEvents.Count ; i++)
		{
			mActionObject.mEvents[i].SetActionObject(mActionObject);
			mActionObject.mEvents[i].Reset();
		}
	}

	//update
	public void Update()
	{
		if(mIsOver)
			return;
		if(null == mOwner)
			return;
		if(mActionObject == null)
			return;

		float difTime = Time.time - this.mStartTime;

		if(mActionObject.mTotalTime < difTime )
		{
			mIsOver = true;
			return;
		}

		for(int i = mCurrentIndex ; i<mActionObject.mEvents.Count ; i++)
		{
			// Debug.LogError("mTime is " + mActionObject.mEvents[i].mTime);
			// Debug.LogError("difTime is " + difTime);
			if(mActionObject.mEvents[i].mTime <= difTime)
			{
				if(!mActionObject.mEvents[i].IsInit)
				{
					mActionObject.mEvents[i].OnEnter();
					mActionObject.mEvents[i].IsInit = true;
				}
				if(mActionObject.mEvents[i].Do())
				{
					mActionObject.mEvents[i].OnExit();
					mCurrentIndex++;
				}
			}
			else
			{
				break;
			}
		}

		return;
	}
}


