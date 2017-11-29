using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour{

	public static Gamemanager instance;

	public GameObject Character;
	public Vector3 nextCharaPosition;
	public int wfacing;
	public Quaternion nextCharaRotation;
	public string sceneToLoad;

	// Use this for initialization
	void Awake () {
		
		if (instance == null) {
			instance = this;
		} else if(instance != this){
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		if (!GameObject.Find("Character")) {
			GameObject chara = Instantiate (Character, nextCharaPosition,nextCharaRotation) as GameObject;
			chara.name = "Character";
		}
	}

		//check if instance exist
	public void LoadNextScene(){		
		SceneManager.LoadScene (sceneToLoad);
	}
}
