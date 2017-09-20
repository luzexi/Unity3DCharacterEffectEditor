
using UnityEngine;
using System.Collections.Generic;

//  MoveState.cs
//  Auth: Lu Zexi
//  2013-11-21


/// <summary>
/// 移动状态
/// </summary>
public class MoveState : StateBase
{
    private Vector3 m_vecTargetPos;     //目标点
    private float m_fCostTime;          //花费时间
    private float m_fLastTime;          //最近时间
    private Vector3 m_vecLastPos;       //最近坐标
    private float _moveSpeed = 2f;
    private List<Vector3> _pathList = new List<Vector3>();

    public MoveState(GfxObject obj)
        : base(obj)
    { 
    }

    /// <summary>
    /// 获取状态类型
    /// </summary>
    /// <returns></returns>
    public override STATE_TYPE GetStateType()
    {
        return STATE_TYPE.STATE_MOVE;
    }

    /// <summary>
    /// 设置参数
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="costTime"></param>
    public void Set( Vector3 _from , Vector3 to, float costTime)
    {
        this.m_vecLastPos = _from;
        this.m_vecTargetPos = to;
        this.m_fCostTime = costTime;
        this.m_fLastTime = Time.time;
    }
    /// <summary>
    /// 设置参数 寻路
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="costTime"></param>
    public void Set( Vector3 to , bool isAstar)    
    {
        
        this.m_vecLastPos = this.m_cObj.transform.position;
        this.m_vecTargetPos = to;
        this._moveSpeed = this.m_cObj.m_MoveSpeed;
        this.m_fLastTime = Time.time;
    }



    /// <summary>
    /// 进入状态
    /// </summary>
    /// <returns></returns>
    public override bool OnEnter()
    {
        if( this.m_cObj.m_cAni["move"] == null )
        {
            this.m_cObj.m_cAni["idle"].wrapMode = WrapMode.Loop;
            this.m_cObj.m_cAni.Play("idle");
        }
        else
        {
            // float rate = this.m_cObj.m_cAni["move"].length / this.m_fCostTime;
            // this.m_cObj.m_cAni["move"].speed = rate;
            this.m_cObj.m_cAni["move"].wrapMode = WrapMode.Loop;
            this.m_cObj.m_cAni.Play("move");
        }
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
