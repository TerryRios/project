using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileMap))]
public class MapEditor : Editor {

	public override void OnInspectorGUI(){
		
		TileMap map = target as TileMap;

		if (DrawDefaultInspector()) {
			map.GenerateMap ();
		}
		if (GUILayout.Button("Generate Map")) {
			map.GenerateMap ();
		}
	}
}
