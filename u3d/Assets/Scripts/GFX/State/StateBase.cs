
using UnityEngine;

//  StateBase.cs
//  Auth: Lu Zexi
//  2013-11-21

/// <summary>
/// 状态类型
/// </summary>
public enum STATE_TYPE
{
    STATE_NONE = 0,
    STATE_IDLE = 1,
    STATE_MOVE = 2,
    STATE_SKILL = 3,
    STATE_ATTACK = 4,
    STATE_HURT = 5,
    STATE_MOVE_BACK = 6,
    STATE_DIE = 7,
}

/// <summary>
/// 状态基类
/// </summary>
public abstract class StateBase
{
    protected GfxObject m_cObj;   //物体

    public StateBase(GfxObject obj)
    {
        this.m_cObj = obj;
    }

    /// <summary>
    /// 获取状态类型
    /// </summary>
    /// <returns></returns>
    public abstract STATE_TYPE GetStateType();

    /// <summary>
    /// 进入事件
    /// </summary>
    /// <returns></returns>
    public virtual bool OnEnter()
    {
        return true;
    }

    /// <summary>
    /// 退出事件
    /// </summary>
    /// <returns></returns>
    public virtual bool OnExit()
    {
        return true;
    }

    /// <summary>
    /// 逻辑更新
    /// </summary>
    /// <returns></returns>
    public abstract bool Update();

    /// <summary>
    /// 销毁
    /// </summary>
    public virtual void Destory()
    {
    }

}
