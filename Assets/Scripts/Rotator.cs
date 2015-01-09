using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.localRotation = new Quaternion (0, 0, 0, 0);
		StartCoroutine(TimedUpdate());
	}
	
	// Update is called once per .25 seconds
	IEnumerator TimedUpdate () {
		while (true) {
			transform.Rotate(Vector3.up * 5);
			
			//Wait x seconds before we preform another update
			yield return new WaitForSeconds (.025f);
		}


	}
}
