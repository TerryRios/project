using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class fin_script : MonoBehaviour {


	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			SceneManager.LoadScene ("Menu");
		}
	}
}
