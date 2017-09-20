
using UnityEngine;

//  IdleState.cs
//  Auth: Lu Zexi
//  2013-11-21


/// <summary>
/// 空闲状态
/// </summary>
public class IdleState : StateBase
{
    public IdleState(GfxObject obj)
        : base(obj)
    { 
    }

    /// <summary>
    /// 获取状态类型
    /// </summary>
    /// <returns></returns>
    public override STATE_TYPE GetStateType()
    {
        return STATE_TYPE.STATE_IDLE;
    }

    /// <summary>
    /// 进入状态
    /// </summary>
    /// <returns></returns>
    public override bool OnEnter()
    {
        this.m_cObj.m_cAni["idle"].wrapMode = WrapMode.Loop;
        this.m_cObj.m_cAni.Play("idle");
        return true;
    }

    /// <summary>
    /// 逻辑更新
    /// </summary>
    /// <returns></returns>
    public override bool Update()
    {
        return true;
    }

}

