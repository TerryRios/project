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
	private bool hasKey = false;
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

		Vector3 detect = (transform.TransformDirection(Vector3.forward));

		if (Physics.Raycast (transform.position, detect, 2.9f)) {
			_cancontrol = false;
		} else {
			_cancontrol = true;
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "key") {
				hasKey = true;
				Destroy (other.gameObject);
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
