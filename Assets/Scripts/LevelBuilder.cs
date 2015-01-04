﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour {

	List<GameObject> cubes;

	private bool createblockfromclick = false;

	Rect saverect;

	// Use this for initialization
	void Start () {
		cubes = new List<GameObject>();
		Debug.Log (typeof(string).Assembly.ImageRuntimeVersion);
	}
	
	// Update is called once per frame
	void Update () {

		//Add a 2D bounding box to the last added cube
		//For some reason, doing this immediately after the BoxCollider has been removed doesn't work
		if (cubes.Count > 0 && cubes [cubes.Count - 1] != null) {
			if(cubes[cubes.Count - 1].GetComponent<BoxCollider>() == null && cubes[cubes.Count - 1].GetComponent<BoxCollider2D>() == null){
				cubes[cubes.Count - 1].AddComponent<BoxCollider2D>();
				cubes[cubes.Count - 1].GetComponent<BoxCollider2D>().isTrigger = true;
				
			}
		}

		RaycastHit hit = new RaycastHit (); 

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
			Destroy(g.collider);

			Vector2 cubePosition = new Vector2();
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


		using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"Levels/level1.txt"))
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
