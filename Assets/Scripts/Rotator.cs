using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	Vector3 axis = Vector3.up;
	float speed = 5;
	
	// Use this for initialization
	void Start () {

		transform.localRotation = new Quaternion (0, 0, 0, 0);
		StartCoroutine(TimedUpdate());
	}

	public void SetSpeed(float speed){
		Debug.Log ("setting spoeed");
		this.speed = speed;
	}

	public void SetAxis(Vector3 axis){
		this.axis = axis;
	}
	
	// Update is called once per .25 seconds
	IEnumerator TimedUpdate () {
		while (true) {
			transform.Rotate(axis * speed);

			//Wait x seconds before we preform another update
			yield return new WaitForSeconds (.025f);
		}


	}
}
