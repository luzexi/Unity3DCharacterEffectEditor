using System;
using System.Collections.Generic;
using UnityEngine;


//  DieState.cs
//  Author: Lu Zexi
//  2013-12-02



/// <summary>
/// 死亡状态
/// </summary>
public class DieState : StateBase
{
    public DieState(GfxObject obj)
        : base(obj)
    { 
        //
    }

    /// <summary>
    /// 获取状态类型
    /// </summary>
    /// <returns></returns>
    public override STATE_TYPE GetStateType()
    {
        return STATE_TYPE.STATE_DIE;
    }

    /// <summary>
    /// 进入事件
    /// </summary>
    /// <returns></returns>
    public override bool OnEnter()
    {
        this.m_cObj.m_cAni["die"].wrapMode = WrapMode.Once;
        this.m_cObj.m_cAni.Play("die");
        this.m_cObj.m_cCharacterController.enabled = false;
        return base.OnEnter();
    }


    /// <summary>
    /// 退出事件
    /// </summary>
    /// <returns></returns>
    public override bool OnExit()
    {
        return base.OnExit();
    }

    /// <summary>
    /// 逻辑更新
    /// </summary>
    /// <returns></returns>
    public override bool Update()
    {
        if( !this.m_cObj.m_cAni.IsPlaying("die") )
        {
            return false;
        }
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
