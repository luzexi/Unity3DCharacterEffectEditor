
// using UnityEngine;
// using System.Collections;
// using UnityEngine.UI;

// //	HitController.cs
// //	Author: Lu Zexi
// //	2015-06-03




// //hit controller
// public class HitController : MonoBehaviour
// {
// 	public float m_Time = 0;	//time
// 	// public HitData m_HitData = null;	//hit data
// 	public ICustomObject m_Owner = null;	//owner
// 	public ICustomObject m_Target = null;	//target

// 	private float m_StartTime = 0;	//start time

// 	//create
// 	public static HitController Create( ICustomObject gfx , ActionObject.Hit hit, EffectController effectController , ICustomObject target )
// 	{
// 		GameObject obj = new GameObject("HitController");
// 		HitController hitController = obj.AddComponent<HitController>();
// 		BoxCollider col = obj.AddComponent<BoxCollider>();
// 		hitController.m_Owner = gfx;
// 		hitController.m_Target = target;

// 		Transform trans = null;
// 		if( hit.bindEffect )
// 		{
// 			trans = effectController.gameObject.transform.Find(hit.parent);
// 		}
// 		else
// 		{
// 			trans = gfx.GetTransform().Find(hit.parent);
// 		}

// 		if( trans != null )
// 		{
// 			obj.transform.parent = trans;
// 		}

// 		obj.transform.localScale = hit.size;
// 		col.size = Vector3.one;
// 		col.center = Vector3.zero;
// 		col.isTrigger = true;

// 		obj.transform.localPosition = hit.offset;
// 		hitController.m_Time = hit.time;
// 		// hitController.m_HitData = hitdata;

// 		obj.transform.localRotation = Quaternion.Euler(hit.rotate);

// 		hitController.m_StartTime = Time.time;

// 		return hitController;
// 	}
	

// 	//update
// 	void Update()
// 	{
// 		float distime = Time.time - this.m_StartTime;
// 		if(distime  > this.m_Time)
// 		{
// 			GameObject.Destroy(this.gameObject);
// 		}
// 	}
// }

