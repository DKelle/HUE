using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelModel : MonoBehaviour {

	public int level;
	
	public GameObject character;

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

	// Update is called once per frame
	IEnumerator TimedUpdate () {
		while (true) {
			
			//Wait x seconds before we preform another update
			yield return new WaitForSeconds (.05f);

		}

	}

	
}
