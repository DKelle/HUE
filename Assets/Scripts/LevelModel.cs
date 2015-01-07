using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelModel : MonoBehaviour {
//int
	public int level;

//string
	string[] levelcomponents;
	

//GameObject
	public GameObject character;
	private GameObject key;

	List<GameObject> Wall;
	List<GameObject> Lava;
	List<GameObject> Key;
	List<List<GameObject>> lists;

	// Use this for initialization
	void Start () {
		levelcomponents = new string[] {"Wall", "Lava", "Key"};
		
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
		character.GetComponent<CharacterModel>().Respawn ();

		level ++;
		loadLevel ();
	}

	void loadLevel(){
		GameObject emptyLevel = GameObject.Find("Level");
		
		for (int i = 0; i < lists.Count; i++){
			string tag = levelcomponents[i];

			string[] rects = System.IO.File.ReadAllLines(@"Levels/level"+level+"/" + tag + ".txt");
			char[] delimiterChars = { ',' };

			foreach(string r in rects){
			//string r = rects [0];
				string[] newr = r.Split(delimiterChars);
				GameObject g = (tag.Equals("Key")) ? Instantiate( Resources.Load("Heart3D", typeof(GameObject)) as GameObject) as GameObject : GameObject.CreatePrimitive(PrimitiveType.Cube);

				if(tag.Equals ("Key")){
					Debug.Log ("on key");
				}

				Vector3 pos 	= new Vector3();
				Vector3 scale 	= new Vector3();
				Quaternion rot 	= new Quaternion(0,0,0,0);

				pos.x = float.Parse(newr[0]);
				pos.y = float.Parse(newr[1]);
				pos.z = float.Parse(newr[2]);
				scale.x = float.Parse(newr[3]);
				scale.y = float.Parse(newr[4]);
				scale.z = float.Parse(newr[5]);

				g.transform.position		= pos;
				g.transform.localScale		= scale;
				g.transform.rotation		= rot;
				
				

				if(g.GetComponent<BoxCollider>() != null)
					g.GetComponent<BoxCollider>().isTrigger = true;
				g.renderer.material.color = Color.white;
				g.tag = tag;


				//Add all of the walls as children of emptyLevel
			
				g.transform.parent = emptyLevel.transform;

				//Change layer to 'Level', so the light hits these blocks
				g.layer = 8;

				if(tag.Equals("Lava")){
					g.renderer.material.color = Color.red;
				}else if(tag.Equals("Key")){
					key = g;
				}

				lists[i].Add (g);

				//Debug.Log(r);
			}
		}

		//After we have loaded all gameobjects to the level, make sure they all have the CollisionDetection script
		foreach (Transform child in emptyLevel.transform) {
			child.gameObject.AddComponent<CollisionDetection>();
		}
	}
	

	// Update is called once per frame
	IEnumerator TimedUpdate () {
		float dtheta = .1f;
		float theta;

		while (true) {
			key.transform.Rotate(Vector3.up * 5);
			
			//Wait x seconds before we preform another update
			yield return new WaitForSeconds (.025f);

		}

	}

	
}
