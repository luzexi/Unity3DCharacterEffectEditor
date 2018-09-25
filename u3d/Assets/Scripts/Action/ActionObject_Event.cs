

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
	public enum ActionType
	{
		DoNothing = 0,	// do nothing
		Prefab,	// put some prefab into game, or diable it, or destroy it
		Transform,	// change self or new prefab's postion or rotation or local position or local rotation or parent
		Material,	// use another material or set Material color or texture
		Animation,	// play animation, fadecross animation, stop animation
		Sound,	// play sound
		Camera,	// camera function, move, shake, rotation, fade in or fade out
		WaitTime,	// wait random second time, or wait custom second time
		CharacterControl,	// use character controller to move
		Tween,	// use tween to move or rotate or stop
		LaunchAmmo,	// launch ammo
		MoveToPosition,	// use A star to move
	}
}
