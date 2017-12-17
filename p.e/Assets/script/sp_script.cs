using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sp_script : MonoBehaviour {

	public string sceneToLoad;
	public string spawnPointName;
	public int _facing;

	void OnTriggerEnter(){
		Gamemanager.instance.boxHit = true;
	}

}
