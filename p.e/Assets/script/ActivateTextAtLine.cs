using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

	public TextAsset theText;

	public int starLine;
	public int endLine;

	public TextBoxManager theTextBox;

	public bool requireButtonPress;
	private bool waitForPress; 

	public bool destroyWhenActivated;

	// Use this for initialization
	void Start () {
		theTextBox = FindObjectOfType<TextBoxManager> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (waitForPress && Input.GetKeyDown(KeyCode.X)) 
		{
			theTextBox.ReloadScript (theText);
			theTextBox.currentLine = starLine;
			theTextBox.endAtLine = endLine;
			theTextBox.EnableTextBox ();

			if (destroyWhenActivated) {
				Destroy (gameObject);
			}
		}
	}
	void OnTriggerEnter(Collider other){
		
		if (other.name == "Player") {
			if (requireButtonPress) {
				waitForPress = true;
				return;
			}
			theTextBox.ReloadScript (theText);
			theTextBox.currentLine = starLine;
			theTextBox.endAtLine = endLine;
			theTextBox.EnableTextBox ();

			if (destroyWhenActivated) {
				Destroy (gameObject);
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.name == "Player") {
			waitForPress = false;
		}
		
	}
}
