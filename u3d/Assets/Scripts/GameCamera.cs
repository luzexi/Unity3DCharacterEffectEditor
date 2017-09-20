using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameCamera : MonoBehaviour
{
	[SerializeField]
	public Transform m_Target;
	[SerializeField]
	public float offset_x = 7;
	[SerializeField]
	public float offset_y = 6;
	[SerializeField]
	public float offset_z = 6;

	void Update()
	{
		if(m_Target != null)
		{
			this.transform.localPosition = this.m_Target.localPosition + new Vector3(this.offset_x,this.offset_y,this.offset_z);
			this.transform.LookAt(this.m_Target);
		}
	}
}

