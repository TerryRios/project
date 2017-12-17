using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movement : MonoBehaviour {

	//
	public GameObject patronesposiblesG;

	public bool CanControl;

	public bool NadaEnfrente;
	public bool atacado = false;
	public bool batallacomenzo;
	public int C_battleBar = 0;

	public enum GameStates
	{
		no_Peleando,
		Zona_Segura,
		Peleando,
		idle
	}
	public GameStates gamestate;

	//
	public bool hasKey1 = false;
	public bool hasKey2 = false;
	public bool toOpenDoor1 = false;
	public bool toOpenDoor2 = false;
	public bool engineisOn = false;
	public bool isTalking;

	public int MakeMove = 0;
	public bool IsMoving = false;
	public int Facing;
	public float WalkSpeed = 5.0f;
	public float TurnSpeed = 1.5f;
	public float GridSize = 1.5f;
	private bool _canturn = true;
	private bool _canmove;
	private Vector3 StartPosition;
	private Vector3 EndPosition;
	private Vector3 old;
	private float t;

	public Transform enemySpawns;

	void Awake(){
		DontDestroyOnLoad (gameObject);
	}

	void Start(){
		if (Gamemanager.instance.nextCharaPosition == new Vector3(0,0,0)) {
			Gamemanager.instance.nextCharaPosition = new Vector3 (-17, 1, 13);
			transform.position = Gamemanager.instance.nextCharaPosition;
		}else
		if (Gamemanager.instance.nextSpawnPoint != "") {
			GameObject spawnPoint = GameObject.Find (Gamemanager.instance.nextSpawnPoint);
			transform.rotation = spawnPoint.transform.rotation;
			transform.position = spawnPoint.transform.position;

			Gamemanager.instance.nextSpawnPoint = "";
		}

		if (Gamemanager.instance.wfacing == 0 && transform.rotation == Quaternion.identity) {
			Facing = 2;
			Debug.Log ("gfj");
		} else {
			Facing = Gamemanager.instance.wfacing;
		}
	}

	public void Update()
	{
		if (atacado == true&&batallacomenzo == true) {
			patronesposiblesG.transform.position = enemySpawns.position;
			patronesposiblesG.transform.rotation = enemySpawns.rotation;
			patronesposiblesG.SetActive(true);
			Gamemanager.instance.detectE = true;
			//			
			Gamemanager.instance.C_battleBar = 1;
			C_battleBar = 1;

			batallacomenzo = false;
			Gamemanager.instance.Atacado = false;
			atacado = false;
		}	
		if (Gamemanager.instance.noEnemys) 
		{
			patronesposiblesG.SetActive (false);
			CanControl = true;
			Gamemanager.instance.C_battleBar = 0;
			C_battleBar = 0;	
		} 

		switch (gamestate) {
		case(GameStates.no_Peleando):
			if (atacado) {
				gamestate = GameStates.Peleando;
			}
			break;
		case(GameStates.Zona_Segura):
			break;
		case(GameStates.Peleando):
			gamestate = GameStates.idle;
			break;
		case(GameStates.idle):
			break;
		}

		RaycastHit hit;
			
		Vector3 detect = (transform.TransformDirection(Vector3.forward));

		if (Physics.Raycast (transform.position, detect, 2.2f)) {

			Debug.DrawLine (transform.position, detect, Color.red);
			_canmove = false;
			NadaEnfrente = false;
		} else {
			Debug.DrawLine (transform.position, detect, Color.green);
			_canmove = true;
			NadaEnfrente = true;
		}
		if (Physics.Raycast (transform.position, detect, out hit, 1.0f)) {

			if (Input.GetKeyDown (KeyCode.F)) {

				if (hit.collider.gameObject.CompareTag ("llave1")) {
					AudioManager.instance.PlaySound2D ("flip_card");
					Destroy(hit.collider.gameObject);
					hasKey1 = true;						
				}
				if (hit.collider.gameObject.CompareTag ("llave2")) {
					AudioManager.instance.PlaySound2D ("flip_card");
					Destroy(hit.collider.gameObject);
					hasKey2 = true;
				}
				if (hit.collider.gameObject.CompareTag ("cardreader1")&&hasKey1 == true) {
					AudioManager.instance.PlaySound2D ("card_beep");
					toOpenDoor1 = true;
					hit.collider.gameObject.GetComponent<Renderer> ().material.color = Color.green;
				}
				if (hit.collider.gameObject.CompareTag ("cardreader2")&&hasKey2 == true) {
					AudioManager.instance.PlaySound2D ("card_beep");
					toOpenDoor2 = true;
					hit.collider.gameObject.GetComponent<Renderer> ().material.color = Color.green;
				}
				if (hit.collider.gameObject.CompareTag ("generator")) {
					AudioManager.instance.PlaySound ("button_sound", hit.collider.gameObject.transform.position);
					engineisOn = true;
					hit.collider.gameObject.GetComponent<Renderer> ().material.color = Color.green;
				}
			}

			if (Input.GetKeyDown (KeyCode.W)) {
				sp_script col = hit.collider.gameObject.GetComponent<sp_script> ();
				Gamemanager.instance.wfacing = col._facing;
				Gamemanager.instance.nextSpawnPoint = col.spawnPointName;
				Gamemanager.instance.sceneToLoad = col.sceneToLoad;
				Gamemanager.instance.LoadNextScene ();
			}
		}

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
		if (_canmove == true&&IsMoving == false) {
			if (CanControl == true) {
				if (Input.GetKeyDown ("w")) {
					AudioManager.instance.PlaySound2D("footsteps_metal");
					MakeMove = 1;	
				}
			}
		}
		if (Input.GetKeyDown("s"))
		{
			MakeMove = -1;
		}
		if (IsMoving == false&&_canturn) {	
			if (CanControl == true) {
				if (Input.GetKeyDown ("a")) {
					MakeMove = 2;
				}
				if (Input.GetKeyDown ("d")) {
					MakeMove = -2;
				}
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
		
	IEnumerator GoForward()
	{
		old = EndPosition;
		MakeMove = 0;

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
			IsMoving = true;
			transform.position = Vector3.Lerp (StartPosition, EndPosition, t);
			yield return new WaitForSeconds (0);
		}

		IsMoving = false;
	}
		
	IEnumerator TurnLeft()
	{
		MakeMove = 0;

		Facing -= 1;
		if (Facing < 0)
			Facing = 3;

		var OldRotation = transform.rotation;
		transform.Rotate(0, -90, 0);
		var NewRotation = transform.rotation;
		IsMoving = true;
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

		Facing += 1;
		if (Facing > 3)
			Facing = 0;

		var OldRotation = transform.rotation;
		transform.Rotate(0, 90, 0);
		var NewRotation = transform.rotation;
		IsMoving = true;
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

	void OnTriggerStay(Collider other){
		if (other.CompareTag("Encuentro1")) {
			if (NadaEnfrente && IsMoving == false&&atacado == false) {
				CanControl = false;
				Debug.Log ("encuentro random");

			
				Debug.Log ("entro");
				patronesPosibles patron = other.gameObject.GetComponent<patronesPosibles> ();
				patronesposiblesG = patron.patronesposibles;
				other.GetComponent<BoxCollider> ().enabled = false;
				atacado = true;
				Gamemanager.instance.Atacado = true;

				batallacomenzo = true;
			}
		}		
	}
}
