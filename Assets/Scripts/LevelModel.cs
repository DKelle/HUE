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
			//trail[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
		}
		loadLevel (level);
		
		StartCoroutine(TimedUpdate());
	}

	void loadLevel(int level){
		GameObject emptyLevel = GameObject.Find("Level");

		string[] rects = System.IO.File.ReadAllLines(@"Levels/level"+level+"/walls.txt");
		char[] delimiterChars = { ',' };

		foreach(string r in rects){
		//string r = rects [0];
			string[] newr = r.Split(delimiterChars);
			GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);

			Vector3 pos = new Vector3();
			Vector3 scale = new Vector3();

			pos.x = float.Parse(newr[0]);
			pos.y = float.Parse(newr[1]);
			pos.z = float.Parse(newr[2]);
			scale.x = float.Parse(newr[3]);
			scale.y = float.Parse(newr[4]);
			scale.z = float.Parse(newr[5]);

			g.transform.position		= pos;
			g.transform.localScale		= scale;

			g.GetComponent<BoxCollider>().isTrigger = true;

			walls.Add (g);

			//Add all of the walls as children of emptyLevel
			g.transform.parent = emptyLevel.transform;

			//Change layer to 'Level', so the light hits these blocks
			g.layer = 8;

			Debug.Log(r);
		}

	}
	
	// Update is called once per frame
	IEnumerator TimedUpdate () {
		while (true) {

			//Move everything in the array back one element
			//Slot 0 should be the current transform of the character
			for (int i = 0; i < trail.Length - 1; i++) {
				if(trail[i+1] != null){
					if(trail[i] == null){
						trail[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					}

					Color oldcolor = trail [i + 1].renderer.material.color;
					//trail [i] = trail[i+1].GetComponent<SpriteRenderer> ().sprite;
					trail [i].transform.position = trail[i+1].transform.position;
					trail [i].transform.localScale = trail[i+1].transform.localScale;
					trail [i].renderer.material.color = new Color(oldcolor.r, oldcolor.g, oldcolor.b, oldcolor.a - .1f);
					trail [i].collider.enabled = false;
					//Debug.Log ("Color: " + trail[i].renderer.material.color);
					
				}

			}

			if(trail[9] == null){
				trail[9] = GameObject.CreatePrimitive(PrimitiveType.Cube);
			}

			trail [9].transform.position = character.transform.position;
			trail [9].transform.localScale = character.transform.localScale;
			trail [9].renderer.material.color = new Color(character.renderer.material.color.r, character.renderer.material.color.g, character.renderer.material.color.b, character.renderer.material.color.a - .1f);
		
			//Debug.Log ("old a new a: " + character.renderer.material.color.a +", "+ (character.renderer.material.color.a - .1f+""));

			//Remove the box collider, so you cannot jump off of yourself
			trail[9].collider.enabled = false;
			
			//Wait x seconds before we preform another update
			yield return new WaitForSeconds (.05f);

		}

	}
	
}
