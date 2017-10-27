using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fin_script : MonoBehaviour {

	public Image playercanvas;

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			Color alpha= playercanvas.color;
			alpha.a = 1;
			playercanvas.color = alpha;
		}
	}
}
