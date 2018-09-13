
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 让所有拥有iCustomObject接口的类都具备action能力
public interface ICustomObject
{
	Transform GetTransform();

	GameObject GetGameObject();

	Animation GetAnimation();

	Animator GetAnimator();

	CharacterController GetCharacterController();
}