using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInporter : MonoBehaviour {

	public TextAsset textfile;
	public string[] textLines;

	// Use this for initialization
	void Start () {

		if (textfile != null) {
			textLines = (textfile.text.Split ("/n"[0]));
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
