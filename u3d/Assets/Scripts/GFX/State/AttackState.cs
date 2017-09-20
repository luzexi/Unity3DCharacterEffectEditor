
using UnityEngine;

//  AttackState.cs
//  Auth: Lu Zexi
//  2013-11-21



/// <summary>
/// 攻击状态
/// </summary>
public class AttackState : StateBase
{
    private const float MOVE_SPEED = 20;
    private const float MOVE_BACK_COST = 0.3f;
    private GfxObject m_cTargetObj;
    private float[] m_vecHitTime1;
    private float[] m_vecHitTime2;
    private float[] m_vecHitRate;
    private bool m_bIsMove;
    private Vector3 m_cPos;
    private int m_iTargetIndex;
    private int m_iSelfIndex;

    private Vector3 m_cStartPos;
    private float m_fStartTime;
    private float m_fMoveCostTime;
    private bool[] m_vecIsHit;
    private System.Action<int,int,float,bool> m_delHitCallback;
    private System.Action<int> m_delOverCallback;

    private enum State
    {
        Start,
        Move_Start,
        Move_Ing,
        Move_End,
        Attack_Start,
        Attack_Ing,
        Attack_End,
        MoveBack_Start,
        MoveBack_Ing,
        MoveBack_End,
        End,
    }

    private State m_eState;

    public AttackState(GfxObject obj)
        : base(obj)
    { 

    }

    /// <summary>
    /// 获取状态类型
    /// </summary>
    /// <returns></returns>
    public override STATE_TYPE GetStateType()
    {
        return STATE_TYPE.STATE_ATTACK;
    }

    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="target"></param>
    public void Set(GfxObject target ,Vector3 pos, int target_index , int self_index ,
        float[] hit_time1 , float[] hit_time2 , float[] hit_rate ,
        System.Action<int,int,float,bool> callback , System.Action<int> over_callback , bool ismove)
    {
        this.m_delHitCallback = callback;
        this.m_delOverCallback = over_callback;
        this.m_cPos = pos;
        this.m_iTargetIndex = target_index;
        this.m_iSelfIndex = self_index;
        this.m_cTargetObj = target;
        this.m_vecHitTime1 = hit_time1;
        this.m_vecHitTime2 = hit_time2;
        this.m_vecHitRate = hit_rate;
        this.m_bIsMove = ismove;
    }

    /// <summary>
    /// 进入状态
    /// </summary>
    /// <returns></returns>
    public override bool OnEnter()
    {
        this.m_cStartPos = this.m_cObj.transform.localPosition;
        this.m_fMoveCostTime = (this.m_cStartPos - this.m_cPos).magnitude / MOVE_SPEED;

        if(this.m_bIsMove)
            this.m_eState= State.Move_Start;
        else
            this.m_eState = State.Attack_Start;

        this.m_cObj.m_cAni["attack"].wrapMode = WrapMode.Once;
        this.m_vecIsHit = new bool[this.m_vecHitTime1.Length];
        for(int i = 0 ; i<this.m_vecIsHit.Length ;i++)
        {
            this.m_vecIsHit[i] = false;
        }
        return true;
    }

    /// <summary>
    /// 逻辑更新
    /// </summary>
    /// <returns></returns>
    public override bool Update()
    {
        switch( this.m_eState )
        {
            case State.Start:
                this.m_eState++;
                break;
            case State.Move_Start:
                if(this.m_cObj.m_cAni["move"] != null)
                {
                    this.m_cObj.m_cAni["move"].wrapMode = WrapMode.Loop;
                    this.m_cObj.m_cAni.Play("move");
                }
                this.m_fStartTime = Time.time;
                this.m_eState++;
                break;
            case State.Move_Ing:
                float move_rate = (Time.time - this.m_fStartTime)/this.m_fMoveCostTime;
                if(move_rate >= 1)
                {
                    this.m_eState++;
                    this.m_cObj.transform.localPosition = this.m_cPos;
                    break;
                }
                this.m_cObj.transform.localPosition = Vector3.Lerp(this.m_cStartPos , this.m_cPos , move_rate);
                break;
            case State.Move_End:
                this.m_eState++;
                break;
            case State.Attack_Start:
                this.m_cObj.m_cAni.Play("attack");
                this.m_eState++;
                this.m_fStartTime = Time.time;
                Debug.Log("attack");
                break;
            case State.Attack_Ing:
                float difTime = Time.time - this.m_fStartTime;
                for(int i = 0 ; i<this.m_vecHitTime1.Length ; i++)
                {
                    if( this.m_vecIsHit[i] ) continue;
                    if(difTime >= this.m_vecHitTime1[i])
                    {
                        //hit
                        this.m_cTargetObj.HurtState();
                        this.m_delHitCallback(this.m_iTargetIndex , this.m_iSelfIndex , this.m_vecHitRate[i] , false);
                        this.m_vecIsHit[i] = true;
                    }
                }
                if(this.m_vecIsHit[this.m_vecIsHit.Length -1])
                {
                    if (!this.m_cObj.m_cAni.IsPlaying("attack"))
                    {
                        this.m_eState++;
                        break;
                    }
                }
                break;
            case State.Attack_End:
                this.m_eState++;
                break;
            case State.MoveBack_Start:
                this.m_fStartTime = Time.time;
                this.m_cObj.m_cAni["idle"].wrapMode = WrapMode.Once;
                this.m_cObj.m_cAni.Play("idle");
                this.m_fMoveCostTime = 2;
                this.m_eState++;
                break;
            case State.MoveBack_Ing:
                difTime = Time.time - this.m_fStartTime;
                if (difTime >= MOVE_BACK_COST)
                {
                    this.m_cObj.transform.localPosition = this.m_cStartPos;
                    this.m_eState++;
                    break;
                }
                this.m_cObj.transform.localPosition = Vector3.Lerp(this.m_cPos, this.m_cStartPos, difTime / MOVE_BACK_COST);
                break;
            case State.MoveBack_End:
                this.m_cObj.m_cAni["idle"].wrapMode = WrapMode.Loop;
                this.m_cObj.m_cAni.Play("idle");
                this.m_eState++;
                break;
            case State.End:
                this.m_delOverCallback(this.m_iSelfIndex);
                this.m_eState++;
                break;
            default:
                break;
        }
        return true;
    }

}

