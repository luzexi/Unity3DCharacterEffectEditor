

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//	EffectController.cs
//	Author: Lu Zexi
//	2015-06-03




//effect controller
public class EffectController : MonoBehaviour
{
	[SerializeField]
	public float time = float.MaxValue;
	[SerializeField]
	public bool bullet = false;		//is bullet
	[SerializeField]
	public ICustomObject owner = null;	//owner
	[SerializeField]
	public ICustomObject target = null;	//target

	private List<ParticleSystem> particles = null;
	private List<Animation> animes = null;
	private Animator animatorCache = null;
	private float startTime = 0;	//start time
	private Vector3 startPosition = Vector3.zero;	//start position
	private float timeCounter = float.MaxValue;

	public bool IsAutoDestroy { get; set; }

	void Awake()
	{
		IsAutoDestroy = true;
		this.particles = new List<ParticleSystem>( GetComponentsInChildren< ParticleSystem >() );
		this.animes = new List<Animation>( GetComponentsInChildren< Animation >() );
		this.animatorCache = GetComponent< Animator >();
	}

	void Start()
	{
		this.startTime = Time.time;
		this.startPosition = this.transform.localPosition;
		if(time < 0 )
		{
			IsAutoDestroy = true;
			this.timeCounter = float.MaxValue;
		}
		else
		{
			IsAutoDestroy = false;
			this.timeCounter = time;
		}
	}

	void Update()
	{
		if(bullet)
		{
			if( target != null && target.GetGameObject() != null )
			{
				if( (this.transform.position - this.target.GetTransform().position).magnitude < 0.1f )
				{
					// target.OnHit( hitData , owner , target );
					GameObject.Destroy(this.gameObject);
					return;
				}
				this.transform.LookAt(this.target.GetTransform());
			}
			else
			{
				if( Time.time - this.startTime > 3f )
				{
					GameObject.Destroy(this.gameObject);
					return;
				}
			}
			this.transform.position += this.transform.forward*time;
			return;
		}

		timeCounter -= Time.deltaTime;
		if ( IsAutoDestroy )
		{
			bool is_alive = false;
			if ( particles.Count > 0 )
			{
				foreach( ParticleSystem p in particles )
				{
					if (p.IsAlive())
					{
						is_alive = true;
						break;
					}
				}
			}
			if ( animes.Count > 0 )
			{
				foreach( Animation p in animes )
				{
					if (p.isPlaying)
					{
						is_alive = true;
						break;
					}
				}
			}

			if ( animatorCache != null )
			{
				//todo playing judge
				is_alive = true;
			}

			if ( timeCounter <= 0f )
			{
				is_alive = false;
			}

			if (is_alive == false)
			{
				GameObject.Destroy(this.gameObject);
				return;
			}
		}
		else
		{
			if ( timeCounter <= 0f )
			{
				GameObject.Destroy(this.gameObject);
				return;
			}
		}
	}
}

