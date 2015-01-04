using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelBuilder : MonoBehaviour {

	List<GameObject> cubes;

	private bool createblockfromclick = false;

	Rect saverect;

	// Use this for initialization
	void Start () {
		cubes = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

		//A left click will create a new block
		//Make sure the user is not already hovered over another block
		createblockfromclick = true;

		//Before we add a new block, make sure that the user isn't trying to resize an already placed block
		foreach(GameObject g in cubes){
			if(g.GetComponent<Resizable>().mouseover){
				createblockfromclick = false;
			}
		}

		if (Input.GetMouseButtonDown (0) && createblockfromclick) {
			Debug.Log("Placing block");

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

			//Make sure the player can rezise without having to release, and reclick
			g.GetComponent<Resizable>().mouseover = true;

			cubes.Add(g);
			//Debug.Log(ray.origin.x);
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

		using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"Levels/level" + level + "/walls.txt"))
		{
			//We don't want to save the block that was placed when clicking the save button, so don't include the last block in the list
			for (int i = 0; i < cubes.Count - 1; i ++)
			{
				GameObject go = cubes[i];

				//Remember position and scale
				string objectdata = go.transform.position.x + "," + go.transform.position.y + "," + go.transform.localScale.x + "," + go.transform.localScale.y;
				file.WriteLine(objectdata);
			}
		}
	}


}
