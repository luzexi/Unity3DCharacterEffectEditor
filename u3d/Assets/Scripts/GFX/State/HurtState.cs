using UnityEngine;

//  HurtState.cs
//  Auth: Lu Zexi
//  2013-11-21



/// <summary>
/// 受击状态
/// </summary>
public class HurtState : StateBase
{

    public HurtState(GfxObject obj)
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
        return STATE_TYPE.STATE_HURT;
    }

    /// <summary>
    /// 进入状态
    /// </summary>
    /// <returns></returns>
    public override bool OnEnter()
    {
        this.m_cObj.m_cAni["hurt"].wrapMode = WrapMode.Once;
        this.m_cObj.m_cAni.Play("hurt");
        return true;
    }

    /// <summary>
    /// 退出状态
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
        if( !this.m_cObj.m_cAni.IsPlaying("hurt") )
        {
            this.m_cObj.IdleState();
            return false;
        }
        return true;
    }

}

