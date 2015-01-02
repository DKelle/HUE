using UnityEngine;
using System.Collections;

public class LevelModel : MonoBehaviour {
	
	public SpriteRenderer character;

	public GameObject[] trail = new GameObject[10];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < trail.Length; i ++) {
			trail[i] = new GameObject();
			trail[i].AddComponent<SpriteRenderer>();
		}
		StartCoroutine(TimedUpdate());
	}
	
	// Update is called once per frame
	IEnumerator TimedUpdate () {
		while (true) {

			//Move everything in the array back one element
			//Slot 0 should be the current transform of the character
			for (int i = 0; i < trail.Length - 1; i++) {
					if (trail [i + 1].GetComponent<SpriteRenderer> ().sprite != null) {
							trail [i].GetComponent<SpriteRenderer> ().sprite = (Sprite)(Sprite.Instantiate (trail [i + 1].GetComponent<SpriteRenderer> ().sprite));
							trail [i].GetComponent<SpriteRenderer> ().transform.position = trail [i + 1].transform.position;
					} else
							Debug.Log ("Trail is null");
					Debug.Log ("X of " + i + " sprite: " + trail [i].GetComponent<SpriteRenderer> ().transform.position.x);
			}



			trail [9].GetComponent<SpriteRenderer> ().sprite = (Sprite)(Sprite.Instantiate (character.sprite));
			trail [9].GetComponent<SpriteRenderer> ().transform.position = character.transform.position;
			Debug.Log ("X of oldest sprite: " + trail [0].GetComponent<SpriteRenderer> ().transform.position.x);
			
			
			//Wait x seconds before we preform another update
			yield return new WaitForSeconds (.05f);

		}

	}
}
