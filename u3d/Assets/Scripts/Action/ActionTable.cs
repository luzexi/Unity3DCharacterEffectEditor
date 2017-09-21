
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


//	ActionTable.cs
//	Author: Lu Zexi
//	2015-06-03



[System.Serializable]
public class ActionTable : ScriptableObject
{
	[SerializeField]
	public bool m_IsLookTarget;

	[SerializeField]
	public List<ActionObject> m_ActionObjects = new List<ActionObject>();


	public void CopyFrom( ActionTable src )
    {
		m_ActionObjects = new List<ActionObject>();
		// orderingActionTable	= src.orderingActionTable;
		m_IsLookTarget = src.m_IsLookTarget;
		foreach( ActionObject ao in src.m_ActionObjects )
		{
			ActionObject dest = new ActionObject();
			dest.CopyFrom( ao );
			m_ActionObjects.Add ( dest );

		}
    }

    public void Init()
    {
    	m_ActionObjects = new List<ActionObject>();
    	m_IsLookTarget = false;
    }

#if UNITY_EDITOR

	private GameObject previewCharacterSource;
	private GfxObject player;
	public void Draw()
	{
		GUILayout.BeginVertical();
		{
			previewCharacterSource = (GameObject)EditorGUILayout.ObjectField( previewCharacterSource, typeof(GameObject), false);

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Create"))
			{
				if(this.player != null)
				{
					GameObject.Destroy(this.player.gameObject);
					this.player = null;
				}
				this.player = (GameObject.Instantiate(previewCharacterSource) as GameObject).AddComponent<GfxObject>();
				GameObject cam = GameObject.Find("Main Camera");
				if(cam == null)
				{
					cam = new GameObject("Main Camera");
				}
				GameCamera game_c = cam.GetComponent<GameCamera>();
				game_c.m_Target = this.player.transform;
				// GameObject ingameObj = GameObject.Instantiate(Resources.Load("Scene/InGame")) as GameObject;
				// InGameManager.I.GameCamera = game_c.GetComponent<Camera>();
			}
			if (GUILayout.Button("Play"))
			{
				if(this.player != null)
				{
					// ActionExcute.Create(this , this.player);
					this.player.transform.localPosition = Vector3.zero;
					this.player.transform.localRotation = Quaternion.identity;
					this.player.SkillState(this);
				}
			}
			this.m_IsLookTarget = GUILayout.Toggle(this.m_IsLookTarget ,"LookTarget");
			if (GUILayout.Button("+ ActionObject"))
			{
				ActionObject ao = new ActionObject();
				this.m_ActionObjects.Add(ao);
			}
			GUILayout.EndHorizontal();
		}
		{
			GUILayout.BeginVertical();
			if(m_ActionObjects != null)
			{
				for( int i = 0 ; i<this.m_ActionObjects.Count ; i++ )
				{
					ActionObject ao = this.m_ActionObjects[i];
					ao.Draw( this );
				}
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndVertical();
	}

    public void OnLoad( string path )
    {
        path = path.Substring(path.IndexOf("Assets"));
        ActionTable load_scene_data = (ActionTable)Instantiate(AssetDatabase.LoadAssetAtPath(path, typeof(ActionTable)));
        if (load_scene_data != null)
        {
            CopyFrom(load_scene_data);
        }
    }

    public void OnSave( string path )
    {
        string asset_path = path.Substring(path.IndexOf("Assets"));//path setup

        ActionTable ao = (ActionTable)AssetDatabase.LoadAssetAtPath(asset_path, typeof(ActionTable));
        if (ao == null)
        {
            ActionTable new_ao = ScriptableObject.CreateInstance<ActionTable>();
            if (new_ao != null)
            {
                AssetDatabase.CreateAsset(new_ao, asset_path);
                ao = new_ao;
            }
        }
        if (ao != null)
        {
            ao.CopyFrom( this );
            EditorUtility.SetDirty(ao);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

#endif
}
