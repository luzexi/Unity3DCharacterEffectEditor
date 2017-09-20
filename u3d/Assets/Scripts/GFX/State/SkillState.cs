using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;


//  SkillState.cs
//  Author: Lu Zexi
//  2013-11-29

public enum ActingActionMode
{
    None,
    Normal,
    IsCancelable,
}


/// <summary>
/// 技能状态
/// </summary>
public class SkillState : StateBase
{
    public ActingActionMode m_ActionMode;   //action mode

    private ActionTable m_ActionTable;
    private ActionExcute m_cExcute;
    private int _skillLayer;
    private HitData m_HitData;

    //message var
    private int m_iMove = 0;    //move type
    private float m_fMoveSpeed = 0; //move speed
    private float m_fMoveTime = 0;  //move time
    private float m_fMoveStartTime = 0; //move start time

    public SkillState(GfxObject obj)
        : base(obj)
    { 
        //
    }

    /// <summary>
    /// 获取状态
    /// </summary>
    /// <returns></returns>
    public override STATE_TYPE GetStateType()
    {
        return STATE_TYPE.STATE_SKILL;
    }
    public int SkillLayer{
        get{
            return this._skillLayer;
        }
    }

    public void Set( ActionTable act , HitData hitData = null, int skillLayer = 0)
    {
        this.m_ActionTable = act;
        this.m_cExcute = ActionExcute.Create(act , this.m_cObj , hitData);
        this.m_HitData = hitData;
        this.m_ActionMode = ActingActionMode.Normal;
        this._skillLayer = skillLayer;
    }

    //set message
    public void SetMessage( string[] args )
    {
        if(args.Length <= 0)
            return;
        string key = args[0];
        switch(key)
        {
            case "move":
                if(args.Length < 3)
                    break;
                this.m_iMove = 1;
                this.m_fMoveSpeed = float.Parse(args[1]);
                this.m_fMoveTime = float.Parse(args[2]);
                this.m_fMoveStartTime = Time.time;
                break;
        }
    }

    /// <summary>
    /// 进入状态
    /// </summary>
    /// <returns></returns>
    public override bool OnEnter()
    {
        this.m_iMove = 0;    //move type
        this.m_fMoveSpeed = 0; //move speed
        this.m_fMoveTime = 0;  //move time
        this.m_fMoveStartTime = 0; //move start time

        // GfxObject target = this.m_cObj.SelectTarget();
        GfxObject target = null;

        if(this.m_ActionTable.m_IsLookTarget)
        {
            if(target != null)
            {
                this.m_cObj.transform.LookAt(target.transform);
            }
        }
        this.m_cExcute.target = target;

        return base.OnEnter();
    }

    /// <summary>
    /// 退出状态
    /// </summary>
    /// <returns></returns>
    public override bool OnExit()
    {
        if(this.m_cExcute != null)
        {
            GameObject.Destroy(this.m_cExcute.gameObject);
            this.m_cExcute = null;
            this.m_ActionTable = null;
        }
        return base.OnExit();
    }

    private void MessageMove()
    {
        if(this.m_iMove == 1)
        {
            float disTime = Time.time - this.m_fMoveStartTime;
            if(disTime > this.m_fMoveTime)
                return;
            this.m_cObj.m_cCharacterController.Move(this.m_cObj.transform.forward*m_fMoveSpeed);
        }
    }

    /// <summary>
    /// 逻辑更新
    /// </summary>
    /// <returns></returns>
    public override bool Update()
    {
        if (this.m_cExcute == null && this.m_cObj.m_cAni.isPlaying )
        {
            this.m_ActionMode = ActingActionMode.IsCancelable;
        }
        else if (this.m_cExcute == null)
        {
            this.m_ActionMode = ActingActionMode.None;
            this.m_cObj.IdleState();
            return false;
        }

        MessageMove();
        return true;
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public override void Destory()
    {
        base.Destory();
    }
}
