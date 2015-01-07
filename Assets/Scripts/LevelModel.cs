using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelModel : MonoBehaviour {

	public int level;
	
	public GameObject character;

	public GameObject[] trail = new GameObject[10];

	List<GameObject> Wall;
	List<GameObject> Lava;
	List<GameObject> Key;
	List<List<GameObject>> lists;

	// Use this for initialization
	void Start () {

		lists = new List<List<GameObject>> ();
		Wall = new List<GameObject>();
		Lava = new List<GameObject>();
		Key = new List<GameObject> ();
		
		lists.Add (Wall);
		lists.Add (Lava);
		lists.Add (Key);

		for (int i = 0; i < trail.Length; i ++) {
			//trail[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
		}
		loadLevel ();
		
		StartCoroutine(TimedUpdate());
	}

	public void NextLevel(){
		character.GetComponent<CharacterController>().Respawn ();

		level ++;
		loadLevel ();
	}

	void loadLevel(){
		GameObject emptyLevel = GameObject.Find("Level");
		
		string[] levelcomponents = new string[] {"Wall", "Lava", "Key"};
		for (int i = 0; i < lists.Count; i++){
			string tag = levelcomponents[i];

			string[] rects = System.IO.File.ReadAllLines(@"Levels/level"+level+"/" + tag + ".txt");
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
				g.renderer.material.color = Color.white;
				g.tag = tag;


				//Add all of the walls as children of emptyLevel
				g.transform.parent = emptyLevel.transform;

				//Change layer to 'Level', so the light hits these blocks
				g.layer = 8;

				if(levelcomponents[i].Equals("Lava")){
					g.renderer.material.color = Color.red;
				}

				lists[i].Add (g);

				Debug.Log(r);
			}
		}

		//After we have loaded all gameobjects to the level, make sure they all have the CollisionDetection script
		foreach (Transform child in emptyLevel.transform) {
			child.gameObject.AddComponent<CollisionDetection>();
		}
	}

	public void Die(){
		for(int i = 0; i < trail.Length; i ++){
			Destroy (trail[i]);
		}
		character.GetComponent<CharacterController>(). Respawn ();
		
	}

	void Update(){
		float newx = character.transform.position.x;
		float newy = character.transform.position.y;
		float newz = character.transform.position.z;

		bool changedpos = false;
		//If the player goes off screen, make sure we reset 
		if (character.transform.position.x < -Camera.main.orthographicSize*2 + 3) {
			changedpos = true;
			newx = Camera.main.orthographicSize*2 - 3;
		}else if(character.transform.position.x > Camera.main.orthographicSize*2 - 3){
			changedpos = true;
			newx = -Camera.main.orthographicSize*2 + 3;
		}

		if (character.transform.position.z < -Camera.main.orthographicSize*2 - 3) {
			changedpos = true;
			newz = Camera.main.orthographicSize*2 - 3;
		}else if(character.transform.position.z > Camera.main.orthographicSize*2 - 3){
			changedpos = true;
			newz = -Camera.main.orthographicSize*2 + 3;
		}

		if (character.transform.position.y < -Camera.main.orthographicSize) {
			changedpos = true;
			newy = Camera.main.orthographicSize;
		}else if(character.transform.position.y > Camera.main.orthographicSize){
			changedpos = true;
			newy = -Camera.main.orthographicSize;
		}


		Vector3 newpos = new Vector3 (newx, newy, newz);
		character.transform.position = newpos;



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
