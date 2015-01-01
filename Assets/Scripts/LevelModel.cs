using UnityEngine;
using System.Collections;

public class LevelModel : MonoBehaviour {

	public SpriteRenderer character;

	public SpriteRenderer[] trail = new SpriteRenderer[10];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//Move everything in the array back one element
		//Slot 0 should be the current transform of the character
		for (int i = 0; i < trail.Length - 1; i++) {
			trail[i] = trail[i+1];
		}

		trail [9] = (SpriteRenderer)(SpriteRenderer.Instantiate(character));

		for (int i = 0; i < trail.Length; i ++) {
			if(trail[i] != null){
				Debug.Log(i + ", " + trail[i].transform.position);
				//s.transform.position = trail[i].position;
			}
	
		}

	}
}
