using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelModel : MonoBehaviour {

	public int level = 1;
	
	public GameObject character;

	public GameObject[] trail = new GameObject[10];
	public List<GameObject> walls;

	// Use this for initialization
	void Start () {
		walls = new List<GameObject>();
		for (int i = 0; i < trail.Length; i ++) {
			trail[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
		}
		loadLevel (3);
		
		StartCoroutine(TimedUpdate());
	}

	void loadLevel(int level){
		Debug.Log ("Trying to load level: " + level);
		string[] rects = System.IO.File.ReadAllLines(@"Levels/level"+level+"/walls.txt");
		char[] delimiterChars = { ',' };

		foreach(string r in rects){
		//string r = rects [0];
			string[] newr = r.Split(delimiterChars);
			GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);

			Vector2 pos = new Vector2();
			Vector2 scale = new Vector2();

			pos.x = float.Parse(newr[0]);
			pos.y = float.Parse(newr[1]);
			scale.x = float.Parse(newr[2]);
			scale.y = float.Parse(newr[3]);

			g.transform.position		= pos;
			g.transform.localScale		= scale;

			walls.Add (g);
	
			Debug.Log(r);
		}
	}
	
	// Update is called once per frame
	IEnumerator TimedUpdate () {
		while (true) {

			//Move everything in the array back one element
			//Slot 0 should be the current transform of the character
			for (int i = 0; i < trail.Length - 1; i++) {
				//if (trail [i + 1].GetComponent<SpriteRenderer> ().sprite != null) {
					Color oldcolor = trail [i + 1].renderer.material.color;
					//trail [i] = trail[i+1].GetComponent<SpriteRenderer> ().sprite;
					trail [i].transform.position = trail[i+1].transform.position;
					trail [i].transform.localScale = trail[i+1].transform.localScale;
					trail [i].renderer.material.color = new Color(oldcolor.r, oldcolor.g, oldcolor.b, oldcolor.a - .1f);
					trail[i].collider.enabled = false;
					
				//}
			}

			//trail [9].GetComponent<SpriteRenderer> ().sprite = (Sprite)(Sprite.Instantiate (character.sprite));
			trail [9].transform.position = character.transform.position;
			trail [9].transform.localScale = character.transform.localScale;
			trail [9].renderer.material.color = new Color(character.renderer.material.color.r, character.renderer.material.color.g, character.renderer.material.color.b, character.renderer.material.color.a - .1f);
			//Debug.Log ("X of oldest sprite: " + trail [0].GetComponent<SpriteRenderer> ().transform.position.x);
			//Remove the box collider, so you cannot jump off of yourself

			trail[9].collider.enabled = false;
			
			//Wait x seconds before we preform another update
			yield return new WaitForSeconds (.05f);

		}

	}
	
}
