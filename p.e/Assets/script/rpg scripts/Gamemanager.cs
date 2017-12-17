using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour{

	public static Gamemanager instance;
	//spawns
	public string nextSpawnPoint;
	public Image loadingScreen;

//	private PlayerClass character1;
//	private HeroStateMachine chara1;

	public GameObject Character;
	public Vector3 nextCharaPosition;
	public int wfacing;

	public Quaternion nextCharaRotation;
	public string sceneToLoad;
	public bool Atacado = false;
	public bool noEnemys;
	public bool detectE = false;
	public int C_battleBar = 0;
	public bool boxHit = false;

	// Use this for initialization
	void Awake () {

		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

		if (instance == null) {
			instance = this;
		} else if(instance != this){
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		if (!GameObject.Find("Character")) {
			Debug.Log ("inst chara");
			GameObject chara = Instantiate (Character, nextCharaPosition,nextCharaRotation) as GameObject;
			chara.name = "Character";	
		}
	}

	void Update(){
		if (boxHit == true) {			
			Debug.Log ("save");
			boxHit = false;
		}
	}
		//check if instance exist
	public void LoadNextScene(){
		loadingScreen.enabled = true;
		SceneManager.LoadSceneAsync(sceneToLoad);
	}
}
