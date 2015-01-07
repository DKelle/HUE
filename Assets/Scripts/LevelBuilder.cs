using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelBuilder : MonoBehaviour {

	List<GameObject> Wall;
	List<GameObject> Lava;
	List<GameObject> Key;
	List<List<GameObject>> lists;

	private bool createblockfromclick = false;

	Rect saverect;

	// Use this for initialization
	void Start () {
		lists = new List<List<GameObject>> ();
		Wall = new List<GameObject>();
		Lava = new List<GameObject>();
		Key = new List<GameObject> ();

		lists.Add (Wall);
		lists.Add (Lava);
		lists.Add (Key);
		
	}
	
	// Update is called once per frame
	void Update () {

		//A left click will create a new block
		//Make sure the user is not already hovered over another block
		createblockfromclick = true;

		//Before we add a new block, make sure that the user isn't trying to resize an already placed block
		foreach (List<GameObject> list in lists) {
			foreach(GameObject g in list){
				if(g.GetComponent<Resizable>().mouseover){
					createblockfromclick = false;
				}
			}
		}

		for (int i = 0; i < lists.Count; i ++) {
			if (Input.GetMouseButtonDown (i) && createblockfromclick) {
				//Debug.Log("Placing block");
				
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				//We have either just created a new block, or are resizing a block we have already place
				
				//Create the cube, and place it in the correct position
				GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
				//Destroy(g.collider);
				
				Vector3 cubePosition = new Vector3();
				cubePosition.x = Mathf.Round(ray.origin.x);
				cubePosition.y = Mathf.Round(ray.origin.y);
				
				g.transform.position = cubePosition;
				
				//Make sure the new block is resizable
				g.AddComponent<Resizable>();
				
				//Make Lava red
				if( i == 1)
					g.renderer.material.color = Color.red;

				//Make sure the player can rezise without having to release, and reclick
				g.GetComponent<Resizable>().mouseover = true;
				
				lists[i].Add(g);
				//Debug.Log(ray.origin.x);

			}
		}
	}


	void OnGUI() {
		//GUI.Box (new Rect (0,0,10,9), "Loader Menu");
		saverect = new Rect (0, 0, 100, 100);
		if (GUI.Button (saverect, "Save")) {
			createblockfromclick = false;
			Debug.Log ("Saving");
			SaveWorld();
		}
		
	}

	void SaveWorld(){
		DirectoryInfo di = new DirectoryInfo ("Levels");
		int level = di.GetDirectories ().Length + 1;

		//Create a directory for the new level
		Directory.CreateDirectory ("Levels/level" + level);

		string[] levelcomponents = new string[]{"Wall", "Lava", "Key"};

		for (int i = 0; i < lists.Count; i ++) {
			string component = levelcomponents[i];
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"Levels/level" + level + "/" + component + ".txt")) {
				//We don't want to save the block that was placed when clicking the save button, so don't include the last block in the list
				for (int j = 0; j < lists[i].Count; j ++) {
					GameObject go = lists[i][j];
				
					//Remember position and scale
					string objectdata = go.transform.position.x + "," + go.transform.position.y + "," + go.transform.position.z + "," + go.transform.localScale.x + "," + go.transform.localScale.y + "," + go.transform.localScale.z;
					file.WriteLine (objectdata);
				}
			}
		}
	}


}
