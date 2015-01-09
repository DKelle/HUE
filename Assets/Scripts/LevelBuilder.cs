using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelBuilder : MonoBehaviour {

//GameObject
	List<GameObject> Wall;
	List<GameObject> Lava;
	List<GameObject> Heart;
	List<GameObject> DraggableWall;
	List<List<GameObject>> lists;

	GameObject oldHeart;
//bool
	private bool createblockfromclick = false;

//string
	public string blockToPlace;


	// Use this for initialization
	void Start () {
		oldHeart = new GameObject ();
		oldHeart.SetActive (false);

		blockToPlace = "Wall";

		lists = new List<List<GameObject>> ();
		Wall = new List<GameObject>();
		Lava = new List<GameObject>();
		Heart = new List<GameObject> ();
		DraggableWall = new List<GameObject> ();
		

		lists.Add (Wall);
		lists.Add (Lava);
		lists.Add (Heart);
		lists.Add (DraggableWall);
		
	}
	
	// Update is called once per frame
	void Update () {

		//A left click will create a new block
		//Make sure the user is not already hovered over another block
		createblockfromclick = true;

		//Before we add a new block, make sure that the user isn't trying to resize an already placed block
		foreach (List<GameObject> list in lists) {
			foreach(GameObject g in list){
				if(g.GetComponent<Resizable>() != null && g.GetComponent<Resizable>().mouseover){
					createblockfromclick = false;
				}
			}
		}

		if (Input.GetMouseButtonDown (0) && createblockfromclick) {
			Debug.Log("Placing block");
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			//We have either just created a new block, or are resizing a block we have already place
			
			//Create the cube, and place it in the correct position
			GameObject g = (blockToPlace.Equals("Heart")) ? Instantiate( Resources.Load("Heart3D", typeof(GameObject)) as GameObject) as GameObject : GameObject.CreatePrimitive(PrimitiveType.Cube);
			
			//Destroy(g.collider);
			
			Vector3 cubePosition = new Vector3();
			cubePosition.x = Mathf.Round(ray.origin.x);
			cubePosition.y = Mathf.Round(ray.origin.y);
			
			g.transform.position = cubePosition;
			
			//Make sure the new block is resizable
			if(!blockToPlace.Equals("Heart")){
				g.AddComponent<Resizable>();

				//Make sure the player can rezise without having to release, and reclick
				g.GetComponent<Resizable>().mouseover = true;
			}else{
				g.AddComponent<ColorChanger>();
				g.AddComponent<Rotator>();
				
			}
			
			//Make Lava red
			if( blockToPlace.Equals("Lava"))
				g.renderer.material.color = Color.red;


			int i = 0;
			if(blockToPlace.Equals("Lava")){
				lists[1].Add(g);
			}else if(blockToPlace.Equals ("Wall")){
				lists[0].Add (g);
			}
			else if(blockToPlace.Equals("Heart")){
				//There can only be one Heart
				if(lists[2].Count > 0){
					oldHeart = GameObject.Instantiate(lists[2][0]) as GameObject;
					oldHeart.SetActive(false);
					Destroy(lists[2][0]);
					lists[2].Clear();
				}
				Quaternion rot = new Quaternion(0, 0, 0, 0);
				g.transform.localRotation = rot;
				lists[2].Add(g);
			}else{
				lists[3].Add(g);
			}
			//Debug.Log(ray.origin.x);

		}
	}

	public void RemoveLastBlock(){
		if(blockToPlace.Equals ("Wall")){
			Destroy(Wall[Wall.Count - 1]);
			Wall.RemoveAt(Wall.Count - 1);
		}else if(blockToPlace.Equals ("Lava")){
			Destroy(Lava[Lava.Count - 1]);
			Lava.RemoveAt(Lava.Count - 1);
		}else if(blockToPlace.Equals("Heart")){
			Destroy(Heart[0]);
			Heart.Clear();
			Heart.Add (GameObject.Instantiate(oldHeart) as GameObject);
			Heart[0].SetActive(true);
			Debug.Log ("Placing oldheart");
		}else{
			Destroy(DraggableWall[DraggableWall.Count - 1]);
			DraggableWall.RemoveAt(DraggableWall.Count - 1);
		}
	}

	public void SaveWorld(){
		DirectoryInfo di = new DirectoryInfo ("Levels");
		int level = di.GetDirectories ().Length + 1;

		//Create a directory for the new level
		Directory.CreateDirectory ("Levels/level" + level);

		string[] levelcomponents = new string[]{"Wall", "Lava", "Heart", "DraggableWall"};

		for (int i = 0; i < lists.Count; i ++) {
			string component = levelcomponents[i];
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"Levels/level" + level + "/" + component + ".txt")) {
				//We don't want to save the block that was placed when clicking the save button, so don't include the last block in the list
				for (int j = 0; j < lists[i].Count; j ++) {
					GameObject go = lists[i][j];
				
					//Remember position and scale
					string objectdata = go.transform.position.x + "," + go.transform.position.y + "," + go.transform.position.z + "," + go.transform.localScale.x + "," + go.transform.localScale.y + "," + go.transform.localScale.z;
					if(component.Equals("DraggableWall"))
						objectdata += ",true";
					file.WriteLine (objectdata);
				}
			}
		}
	}


}
