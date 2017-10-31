using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour {

	public float _speed = 1;
	public bool _cancontrol = true;
	public Vector3 _moveVector;
	private bool _iscrouched;
	[System.NonSerialized]
	public float _verticalSpeed = 0;
	private float angle;
	public Vector3 _targetmove;
	private bool _canturn;
	private float targetAngle;
	public bool hasKey1 = false;
	public bool hasKey2 = false;
	public bool toOpenDoor1 = false;
	public bool toOpenDoor2 = false;
	public bool engineisOn = false;
	public bool isTalking;

	// Use this for initialization
	void Start ()
	{
		_targetmove = transform.position;
		_targetmove.y = 1.0f;
	}		

	// Update is called once per frame
	void Update () {
		
		move ();
		turn ();
	}

	void FixedUpdate(){
		
		RaycastHit hit;
		Vector3 detect = (transform.TransformDirection(Vector3.forward));

		if (Physics.Raycast (transform.position, detect, 2.9f)) {
			_cancontrol = false;
		} else {
			_cancontrol = true;
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			if (Physics.Raycast (transform.position, detect, out hit)) {

				if (hit.collider.gameObject.CompareTag ("llave1")) {
					AudioManager.instance.PlaySound2D ("flip_card");
					hit.collider.gameObject.SetActive (false);
					hasKey1 = true;						
				}
				if (hit.collider.gameObject.CompareTag ("llave2")) {
					AudioManager.instance.PlaySound2D ("flip_card");
					hit.collider.gameObject.SetActive (false);
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
				if (hit.collider.gameObject.CompareTag ("stair1")) {
					transform.position = new Vector3 (5, 1, -7);
				}
			}
		}
	}

	void move(){

		if (isTalking) {
			return;
		}

		if (_cancontrol) {
			
			if (Input.GetKeyDown (KeyCode.W)) {
				AudioManager.instance.PlaySound2D("footsteps_metal");
				_targetmove = transform.position + transform.forward * 2;
				_cancontrol = false;
			}
		}

		if (transform.position != _targetmove) {
			if (Vector3.Distance (transform.position, _targetmove) > 0.1f) {

				transform.position += (transform.forward * Time.deltaTime*5);

			} else {			
				transform.position = _targetmove;
				_cancontrol = true;
			}
		}


	}
		
	void turn(){

		if (isTalking) {
			return;
		}

		if (_canturn) {

			if (Input.GetButtonDown ("izquierdo")) {

				targetAngle += -90;
				_canturn = false;
			}

			if (Input.GetButtonDown ("derecho")) {

				targetAngle += 90;
				_canturn = false;
			}							
		}					 										

	   if (Mathf.Abs(targetAngle - angle)>5.0f) {
			
		if (targetAngle > angle) {

			angle += Time.deltaTime * 180;

		} else {

			angle -= Time.deltaTime * 180;
		}
	}
		if (Mathf.Abs (targetAngle - angle) < 5.0f) {
			angle = targetAngle;
			_canturn = true;	
		}

		transform.rotation = Quaternion.Euler (0, angle, 0);
	}
}
