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
	private CharacterController _controller;
	private float angle;
	public Vector3 _targetmove;
	private bool _canturn;
	private float targetAngle;


	// Use this for initialization
	void Start ()
	{

		_controller = GetComponent<CharacterController> ();

	}

	// Update is called once per frame
	void Update () {
		
		move ();
		turn ();

		_moveVector *= Time.deltaTime;
		_controller.Move (_moveVector);
		_moveVector.y = 0;
		transform.LookAt (transform.position + _moveVector);


	}

	void move(){


		
		if (Input.GetKeyDown(KeyCode.W)) {

			_targetmove= transform.position + transform.forward * 4;

		}

		if (Vector3.Distance(transform.position,_targetmove)<1.0f) {

			transform.position += transform.forward * Time.deltaTime  ;
			
		}

		transform.position  = _targetmove;


	


	}
		
	void turn(){

		if (angle == targetAngle) {

			_canturn = true;
			
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

			angle += Time.deltaTime * 160;

		} else {

			angle -= Time.deltaTime * 160;

		}

	}

		if (Mathf.Abs (targetAngle - angle) < 5.0f) {

			angle = targetAngle;
		}

		transform.rotation = Quaternion.Euler (0, angle, 0);
	}

}
