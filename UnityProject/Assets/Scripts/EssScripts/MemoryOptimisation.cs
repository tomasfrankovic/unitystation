using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class MemoryOptimisation : MonoBehaviour
{
	public SpriteAtlas atlas;

	[ContextMenu("AddSpriteToAtlas")]
	public void AddSpriteToAtlas()
	{
		string[] guids1 = AssetDatabase.FindAssets("t:Texture2D", new string[] {"Assets/Textures/Clothing"});
		Object[] objs = new Object[100];

		//foreach (string guid1 in guids1)
		for (int i = 0; i < objs.Length; i++)
		{
			//Debug.Log(AssetDatabase.GUIDToAssetPath(guid1));
			Object obj = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guids1[i]));
			//Debug.Log(guids1[i]);
			Texture2D go = (Texture2D)obj;
			//Debug.Log(go);
			objs[i] = go;
			//Debug.Log(objs[i]);
		}
		atlas.Add(objs);
		Debug.Log("Done");


		/*Object obj = AssetDatabase.LoadMainAssetAtPath("Assets/Textures/background/space/space_1.png");

		if (obj == null && !(obj is GameObject))
			return;

		Texture2D go = (Texture2D)obj;
		Debug.Log(go);

		atlas.Add(new Object[]{go});*/
	}
}
