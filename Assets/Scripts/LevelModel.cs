﻿using UnityEngine;
using System.Collections;

public class LevelModel : MonoBehaviour {

	public int level;
	
	public SpriteRenderer character;

	public GameObject[] trail = new GameObject[10];

	// Use this for initialization
	void Start () {
		level = 1;
		//loadLevel ();
		for (int i = 0; i < trail.Length; i ++) {
			trail[i] = new GameObject();
			trail[i].AddComponent<SpriteRenderer>();
		}
		StartCoroutine(TimedUpdate());
	}

	void loadLevel(){
		string[] rects = System.IO.File.ReadAllLines(@"Levels/level"+level+"/walls2.txt");
		char[] delimiterChars = { ',' };

		foreach(string r in rects){
		//string r = rects [0];
			string[] newr = r.Split(delimiterChars);
			GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);

			Vector2 pos = new Vector2();
			Vector2 scale = new Vector2();

			pos.x = System.Int32.Parse(newr[0]);
			pos.y = System.Int32.Parse(newr[1]);
			scale.x = System.Int32.Parse(newr[2]);
			scale.y = System.Int32.Parse(newr[3]);
			


			g.transform.position		= pos;
			//g.transform.localScale		= scale;
			
			Debug.Log(r);
		}
	}
	
	// Update is called once per frame
	IEnumerator TimedUpdate () {
		while (true) {

			//Move everything in the array back one element
			//Slot 0 should be the current transform of the character
			for (int i = 0; i < trail.Length - 1; i++) {
				if (trail [i + 1].GetComponent<SpriteRenderer> ().sprite != null) {
					SpriteRenderer r = trail [i + 1].GetComponent<SpriteRenderer> ();
					trail [i].GetComponent<SpriteRenderer> ().sprite = trail[i+1].GetComponent<SpriteRenderer> ().sprite;
					trail [i].GetComponent<SpriteRenderer> ().transform.position = trail[i+1].transform.position;
					trail [i].GetComponent<SpriteRenderer> ().transform.localScale = trail[i+1].transform.localScale;
					trail [i].GetComponent<SpriteRenderer> ().color = new Color(r.color.r, r.color.g, r.color.b, r.color.a - .1f);
					
				}
			}



			trail [9].GetComponent<SpriteRenderer> ().sprite = (Sprite)(Sprite.Instantiate (character.sprite));
			trail [9].GetComponent<SpriteRenderer> ().transform.position = character.transform.position;
			trail [9].GetComponent<SpriteRenderer> ().transform.localScale = character.transform.localScale;
			trail [9].GetComponent<SpriteRenderer> ().color = new Color(character.color.r, character.color.g, character.color.b, character.color.a - .1f);
			//Debug.Log ("X of oldest sprite: " + trail [0].GetComponent<SpriteRenderer> ().transform.position.x);
			
			
			//Wait x seconds before we preform another update
			yield return new WaitForSeconds (.05f);

		}

	}
	
}
