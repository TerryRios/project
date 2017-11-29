using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movement : MonoBehaviour {

	public int MakeMove = 0;
	public bool IsMoving = false;
	public int Facing;
	public float WalkSpeed = 5.0f;
	public float TurnSpeed = 1.5f;
	public float GridSize = 1.5f;
	private bool _canmove = false;
	private Vector3 StartPosition;
	private Vector3 EndPosition;
	private Vector3 old;
	private float t;

	void Start(){
		if (Gamemanager.instance.nextCharaPosition == new Vector3(0,0,0)) {
			Gamemanager.instance.nextCharaPosition = new Vector3 (-17, 1, 13);
		}
		transform.rotation = Gamemanager.instance.nextCharaRotation;
		if (Gamemanager.instance.wfacing == 0 && transform.rotation == Quaternion.identity) {
			Facing = 2;
			Debug.Log ("gfj");
		} else {
			Facing = Gamemanager.instance.wfacing;
		}
		transform.position = Gamemanager.instance.nextCharaPosition;
	}

	public void Update()
	{
		if (MakeMove == 1 && !IsMoving)
		{
			StartCoroutine(GoForward());
		}

		if (MakeMove == -1 && !IsMoving)
		{
//			StartCoroutine(GoBack());
		}

		if (MakeMove == 2 && !IsMoving)
		{
			StartCoroutine(TurnLeft());
		}

		if (MakeMove == -2 && !IsMoving)
		{
			StartCoroutine(TurnRight());
		}

		if (MakeMove == 3 && !IsMoving)
		{
//			StartCoroutine(StepLeft());
		}

		if (MakeMove == -3 && !IsMoving)
		{
//			StartCoroutine(StepRight());
		}
		if (_canmove&&IsMoving == false) {
					
			if (Input.GetKeyDown ("w")) {
				MakeMove = 1;
			}
		}
		if (Input.GetKeyDown("s"))
		{
			MakeMove = -1;
		}
		if (IsMoving == false) {			
		
			if (Input.GetKeyDown ("a")) {
				MakeMove = 2;
			}
			if (Input.GetKeyDown ("d")) {
				MakeMove = -2;
			}
		}
		if (Input.GetKeyDown("q"))
		{
			MakeMove = 3;
		}
		if (Input.GetKeyDown("e"))
		{
			MakeMove = -3;
		}
	}

	void FixedUpdate(){

		RaycastHit hit;
		
		Vector3 detect = (transform.TransformDirection(Vector3.forward));

		if (Physics.Raycast (transform.position, detect, out hit, 1.0f)) {
			Debug.Log (hit.collider.gameObject.name == "SP_stairDown1");
			if (Input.GetKeyDown (KeyCode.W)) {
				sp_script col = hit.collider.gameObject.GetComponent<sp_script> ();
				Gamemanager.instance.wfacing = col._facing;
				Gamemanager.instance.nextCharaRotation = col.spawnPoint.transform.rotation;
				Gamemanager.instance.nextCharaPosition = col.spawnPoint.transform.position;
				Gamemanager.instance.sceneToLoad = col.sceneToLoad;
				Gamemanager.instance.LoadNextScene();
			} 
			Debug.Log (hit.collider.gameObject.name == "SP_stairUp1");
			if (Input.GetKeyDown (KeyCode.W)) {
				sp_script col = hit.collider.gameObject.GetComponent<sp_script> ();
				Gamemanager.instance.wfacing = col._facing;
				Gamemanager.instance.nextCharaRotation = col.spawnPoint.transform.rotation;
				Gamemanager.instance.nextCharaPosition = col.spawnPoint.transform.position;
				Gamemanager.instance.sceneToLoad = col.sceneToLoad;
				Gamemanager.instance.LoadNextScene();
			}
			Debug.Log (hit.collider.gameObject.name == "SP_stairDown2");
			if (Input.GetKeyDown (KeyCode.W)) {
				sp_script col = hit.collider.gameObject.GetComponent<sp_script> ();
				Gamemanager.instance.wfacing = col._facing;
				Gamemanager.instance.nextCharaRotation = col.spawnPoint.transform.rotation;
				Gamemanager.instance.nextCharaPosition = col.spawnPoint.transform.position;
				Gamemanager.instance.sceneToLoad = col.sceneToLoad;
				Gamemanager.instance.LoadNextScene();
			} 
			Debug.Log (hit.collider.gameObject.name == "SP_stairUp2");
			if (Input.GetKeyDown (KeyCode.W)) {
				sp_script col = hit.collider.gameObject.GetComponent<sp_script> ();
				Gamemanager.instance.wfacing = col._facing;
				Gamemanager.instance.nextCharaRotation = col.spawnPoint.transform.rotation;
				Gamemanager.instance.nextCharaPosition = col.spawnPoint.transform.position;
				Gamemanager.instance.sceneToLoad = col.sceneToLoad;
				Gamemanager.instance.LoadNextScene();
			} 
		}

	   if (Physics.Raycast (transform.position, detect, 1.9f)) {
			
			Debug.DrawLine (transform.position, detect, Color.red);
		   _canmove = false;
	   } else {
			Debug.DrawLine (transform.position, detect, Color.green);
		   _canmove = true;
	   }
	}

	IEnumerator GoForward()
	{
		old = EndPosition;
		MakeMove = 0;
		IsMoving = true;

		StartPosition = transform.position;

		if (Facing == 0) {
			EndPosition = transform.position - (new Vector3 (0.0f, 0.0f, GridSize));
		}

		if (Facing == 1) {
			EndPosition = transform.position - (new Vector3 (GridSize, 0.0f, 0.0f));
		}

		if (Facing == 2) {
			EndPosition = transform.position - (new Vector3 (0.0f, 0.0f, -GridSize));
		}

		if (Facing == 3) {
			EndPosition = transform.position - (new Vector3 (-GridSize, 0.0f, 0.0f));
		}

		t = 0.0f;

		while (t < 1.0) {
			t += Time.deltaTime * (WalkSpeed / GridSize);
			transform.position = Vector3.Lerp (StartPosition, EndPosition, t);
			yield return new WaitForSeconds (0);
		}

		IsMoving = false;

	}

/*
	IEnumerator GoBack()
	{
		old = EndPosition;
		MakeMove = 0;
		IsMoving = true;

		StartPosition = transform.position;

		if (Facing == 0)
			EndPosition = transform.position - (new Vector3(0.0f, 0.0f, -GridSize));

		if (Facing == 1)
			EndPosition = transform.position - (new Vector3(-GridSize, 0.0f, 0.0f));

		if (Facing == 2)
			EndPosition = transform.position - (new Vector3(0.0f, 0.0f, GridSize));

		if (Facing == 3)
			EndPosition = transform.position - (new Vector3(GridSize, 0.0f, 0.0f));

		t = 0.0f;

		while (t < 1.0)
		{
			t += Time.deltaTime * (WalkSpeed / GridSize);
			transform.position = Vector3.Lerp(StartPosition, EndPosition, t);
			yield return new WaitForSeconds(0);
		}

		IsMoving = false;
	}

	IEnumerator StepLeft()
	{
		old = EndPosition;
		MakeMove = 0;
		IsMoving = true;

		StartPosition = transform.position;

		if (Facing == 0)
		{
			EndPosition = transform.position - (new Vector3(-GridSize, 0.0f, 0.0f));
		}

		if (Facing == 1)
		{
			EndPosition = transform.position - (new Vector3(0.0f, 0.0f, GridSize));
		}

		if (Facing == 2)
		{
			EndPosition = transform.position - (new Vector3(GridSize, 0.0f, 0.0f));
		}

		if (Facing == 3)
		{
			EndPosition = transform.position - (new Vector3(0.0f, 0.0f, -GridSize));
		}

		t = 0.0f;

		while (t < 1.0)
		{
			t += Time.deltaTime * (WalkSpeed / GridSize);
			transform.position = Vector3.Lerp(StartPosition, EndPosition, t);
			yield return new WaitForSeconds(0);
		}

		IsMoving = false;
	}

	IEnumerator StepRight()
	{
		old = EndPosition;
		MakeMove = 0;
		IsMoving = true;

		StartPosition = transform.position;

		if (Facing == 0)
		{
			EndPosition = transform.position - (new Vector3(GridSize, 0.0f, 0.0f ));
		}

		if (Facing == 1)
		{
			EndPosition = transform.position - (new Vector3( 0.0f, 0.0f, -GridSize ));
		}

		if (Facing == 2)
		{
			EndPosition = transform.position - (new Vector3(-GridSize, 0.0f, 0.0f ));
		}

		if (Facing == 3)
		{
			EndPosition = transform.position - (new Vector3( 0.0f, 0.0f, GridSize));
		}

		t = 0.0f;

		while (t < 1.0)
		{
			t += Time.deltaTime * (WalkSpeed / GridSize);
			transform.position = Vector3.Lerp(StartPosition, EndPosition, t);
			yield return new WaitForSeconds(0);
		}

		IsMoving = false;
	}
*/
	IEnumerator TurnLeft()
	{
		MakeMove = 0;
		IsMoving = true;

		Facing -= 1;
		if (Facing < 0)
			Facing = 3;

		var OldRotation = transform.rotation;
		transform.Rotate(0, -90, 0);
		var NewRotation = transform.rotation;

		for (t = 0.0f; t <= 1.0f; t += (TurnSpeed * Time.deltaTime))
		{
			transform.rotation = Quaternion.Slerp(OldRotation, NewRotation, t);
			yield return new WaitForSeconds(0);
		}

		transform.rotation = NewRotation;

		IsMoving = false;
	}

	IEnumerator TurnRight()
	{
		MakeMove = 0;
		IsMoving = true;

		Facing += 1;
		if (Facing > 3)
			Facing = 0;

		var OldRotation = transform.rotation;
		transform.Rotate(0, 90, 0);
		var NewRotation = transform.rotation;

		for (t = 0.0f; t <= 1.0f; t += (TurnSpeed * Time.deltaTime))
		{
			transform.rotation = Quaternion.Slerp(OldRotation, NewRotation, t);
			yield return new WaitForSeconds(0);
		}

		transform.rotation = NewRotation;

		IsMoving = false;
	}

	public void OnCollisionEnter()
	{
		EndPosition = old;
	}
		
}
