using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reflectcontroler : MonoBehaviour {

	public enum directions {x,y,z};
	public directions orientation;
	public GameObject mirror;
	public GameObject character;

	float offset;
    Vector3 probePos;


	// Update is called once per frame
	void Update () {
		if (orientation == directions.x) {
			offset = mirror.transform.position.x - character.transform.position.x;
			probePos.x = mirror.transform.position.x + offset + 3.0f;
			probePos.y = character.transform.position.y;
			probePos.z = character.transform.position.z;
		}
		if (orientation == directions.y) {
			offset = mirror.transform.position.x - character.transform.position.y;
			probePos.x = character.transform.position.x;
			probePos.y = mirror.transform.position.y + offset + 3.0f;
			probePos.z = character.transform.position.z;
		}
		if (orientation == directions.z) {
			offset = mirror.transform.position.x - character.transform.position.z;
			probePos.x = character.transform.position.x;
			probePos.y = character.transform.position.y;
			probePos.z = mirror.transform.position.z + offset + 3.0f;
		}

		transform.position = probePos;

	}
}
