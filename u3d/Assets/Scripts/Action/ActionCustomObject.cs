
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionCustomObject : MonoBehaviour, ICustomObject
{
	public Animation mAnimation;
	public Animator mAnimator;
	public CharacterController mCharacterController;

	void Awake()
	{
		if(mAnimation == null)
		{
			mAnimation = GetComponent<Animation>();
		}
		if(mAnimator == null)
		{
			mAnimator = GetComponent<Animator>();
		}
		if(mCharacterController == null)
		{
			mCharacterController = GetComponent<CharacterController>();
		}
	}

	public Transform GetTransform()
	{
		return this.transform;
	}

	public GameObject GetGameObject()
	{
		return this.gameObject;
	}

	public Animation GetAnimation()
	{
		return mAnimation;
	}

	public Animator GetAnimator()
	{
		return mAnimator;
	}

	public CharacterController GetCharacterController()
	{
		return mCharacterController;
	}
}