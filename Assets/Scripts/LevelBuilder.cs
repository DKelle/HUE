using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour {

	List<GameObject> cubes;

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
			if(cubes[cubes.Count - 1].GetComponent<BoxCollider>() == null){
				cubes[cubes.Count - 1].AddComponent<BoxCollider2D>();
			}
		}

		RaycastHit hit = new RaycastHit (); 

		//A left click will create a new block
		bool clickedcube = false;

		//Before we add a new block, make sure that the user isn't trying to resize an already placed block
		foreach(GameObject g in cubes){
			if(g.GetComponent<Resizable>().foundblock){
				clickedcube = true;
			}
		}

		if (Input.GetMouseButtonDown (0) && !clickedcube) {
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
			g.GetComponent<Resizable>().foundblock = true;

			cubes.Add(g);
			//Debug.Log(ray.origin.x);
		}
		
		if (Input.GetMouseButtonDown (1)) {
			Debug.Log("Pressed right click.");
		}
		
		if (Input.GetMouseButtonDown (2)) {
			Debug.Log("Pressed middle click.");		
		}
	}


}
