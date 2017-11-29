using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class luzparpadeo : MonoBehaviour {

	public GameObject Lights;
	public float timer;

	void Start()
	{
		StartCoroutine(FlickeringLight());
	}

	IEnumerator FlickeringLight()
	{
		Lights.SetActive(true);
		timer = Random.Range(0.1f, 1);
		yield return new WaitForSeconds(timer);
		Lights.SetActive(false);
		timer = Random.Range(0.1f, 1);
		yield return new WaitForSeconds(timer);
		StartCoroutine(FlickeringLight());
	}
}
