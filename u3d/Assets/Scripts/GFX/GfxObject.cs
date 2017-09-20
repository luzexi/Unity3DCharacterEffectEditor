
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  GfxObject.cs
//  Author: Lu Zexi
//  2013-11-21



/// <summary>
/// 图形渲染类
/// </summary>
public partial class GfxObject : MonoBehaviour
{
    public StateControl m_cStateControl = null; //状态控制类
    public Animation m_cAni = null; //animation
    public CharacterController m_cCharacterController = null;   //character controller

    public GfxObject m_SelectTarget = null; //select target

    public int Hp =200;   //hp
    public int MaxHp =200;    //max hp
    public int Mp = 200;   //mp
    public int MaxMp = 200;    //max mp

    public int Atk = 20;  //atk
    public int Def = 5;  //def
    public int Hit = 1;  //hit

    public int m_Team = 0;   //team id
    public float m_ViewRange = 5f;   //view range
    public float m_MoveSpeed = 0.01f;   //move speed

    public bool m_IsPlayer = false; //is player

    // public DropObject DropObject = null; //test Drop
    // public Seeker m_Seeker;
    
    public Rigidbody m_rigidbody;
    public CapsuleCollider m_capsuleC;

    public Vector3 MoveDirection{get;set;}
    public Vector3 PushVelocity {get;set;}




    public virtual void Awake()
    {
        this.m_cAni = this.GetComponent<Animation>();
        if(null == this.m_cAni){
            this.m_cAni = this.gameObject.AddComponent<Animation>();
        }
        this.m_cCharacterController = this.GetComponent<CharacterController>();
        if(null == this.m_cCharacterController){
            this.m_cCharacterController = this.gameObject.AddComponent<CharacterController>();
        }
        this.m_cStateControl = new StateControl(this);
        this.m_cStateControl.Idle();
        if(null == this.m_rigidbody){
            this.m_rigidbody = this.gameObject.AddComponent<Rigidbody>();
            this.m_rigidbody.useGravity = false;
            this.m_rigidbody.isKinematic = true;
        } 

    }

    //start
    public virtual void Start()
    {
        //
    }

    //on active
    public virtual void OnActive()
    {
        //
    }

    //destory
    public virtual void Destory()
    {
        GameObject.Destroy(this.gameObject);
    }

    //update
    public virtual void Update()
    {
       this.m_cStateControl.Update();
       UpdatePosition();
    }

     //更新位置
    public virtual void UpdatePosition(){
        float cacheSpeed = 0;
        if(this.m_cStateControl.GetCurrentState().GetStateType() == STATE_TYPE.STATE_MOVE){
            cacheSpeed = this.m_MoveSpeed;
        }
            
        Vector3 v = this.MoveDirection * cacheSpeed;
        Vector3 vel = v;
        Vector3 cachePuv = this.PushVelocity;
        cachePuv.y = 0;
        vel += cachePuv;
        this.PushVelocity = Vector3.zero;
        if(vel != Vector3.zero)
            this.m_cCharacterController.Move(vel);
    }

    //is die
    public bool IsDie()
    {
        if( this.m_cStateControl.GetCurrentState() == null ||
            this.m_cStateControl.GetCurrentState().GetStateType() == STATE_TYPE.STATE_DIE
            )
            return true;
        return false;
    }

    ////////////////////////////////////////// state ////////////////////////////////////

    //die state
    public  void DieState(){
        if( IsDie() ) return;
        this.m_cStateControl.Die();
    }

    //idle state
    public void IdleState()
    {
        if( IsDie() ) return ;
        this.m_cStateControl.Idle();
    }

    //a start move state
    public void ASMoveState(Vector3 to)
    {
        if( IsDie() ) return ;
        this.m_cStateControl.Move(to,true);
    }

    //move dir
    public void MoveState( Vector3 dir )
    {
        if( IsDie() ) return ;
        this.m_cStateControl.Move(dir,false);
    }

    //hurt state
    public void HurtState()
    {
        if( IsDie() ) return ;
        this.m_cStateControl.Hurt();
    }

    public void SkillState( string path, int skillLayer = 0)
    {
        SkillState(path , null, skillLayer);
    }

    //skill state
    public void SkillState( string path , HitData hitData, int skillLayer = 0)
    {
        ActionTable act = Resources.Load("Battle/Action/"+path) as ActionTable;
        SkillState(act , hitData, skillLayer);
    }

    //skill state
    public void SkillState( ActionTable act , HitData hitData = null ,  int skillLayer = 0)
    {
        if( IsDie() ) return ;
        if(hitData == null)
        {
            hitData = new HitData();
            hitData.minAttack = Atk;
            hitData.maxAttack = Atk;
        }
        this.m_cStateControl.Skill(act , hitData, skillLayer);
    }

    //skill message
    public void SkillMessage( string args )
    {
        if( IsDie() ) return ;
// #if UNITY_EDITOR
//         if( EditorApplication.currentScene == "ActionEditor" )
//         {
//             return;
//         }
// #endif
        string[] msg_args = args.Split(';');
        StateBase state = this.m_cStateControl.GetCurrentState();
        if(state != null && state.GetStateType() == STATE_TYPE.STATE_SKILL)
        {
            SkillState skillState = (SkillState)state;
            skillState.SetMessage(msg_args);
        }
    }
}
