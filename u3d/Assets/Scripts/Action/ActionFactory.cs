

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif


//action factory
public class ActionFactory
{
	public static ActionEvent CreateActionEvent(ActionObject.ActionType _actionType)
	{
		ActionEvent ae = null;
		switch(_actionType)
		{
			case ActionObject.ActionType.Prefab:
				ae = new ActionEvent_Prefab();
				break;
			case ActionObject.ActionType.Transform:
				break;
			case ActionObject.ActionType.Material:
				break;
			case ActionObject.ActionType.Animation:
				ae = new ActionEvent_Animation();
				break;
			case ActionObject.ActionType.Sound:
				ae = new ActionEvent_Sound();
				break;
			case ActionObject.ActionType.Camera:
				ae = new ActionEvent_Camera();
				break;
			case ActionObject.ActionType.WaitTime:
				break;
			case ActionObject.ActionType.CharacterControl:
				ae = new ActionEvent_CharacterControl();
				break;
			case ActionObject.ActionType.Tween:
				break;
			case ActionObject.ActionType.LaunchAmmo:
				break;
			case ActionObject.ActionType.MoveToPosition:
				break;
		}
		return ae;
	}
}