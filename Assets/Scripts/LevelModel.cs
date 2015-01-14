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
	List<GameObject> Heart;
	List<GameObject> DraggableWall;
	List<List<GameObject>> lists;

	// Use this for initialization
	void Start () {
		levelcomponents = new string[] {"Wall", "Lava", "Heart"};
		
		lists = new List<List<GameObject>> ();
		Wall = new List<GameObject>();
		Lava = new List<GameObject>();
		Heart = new List<GameObject> ();

		lists.Add (Wall);
		lists.Add (Lava);
		lists.Add (Heart);


		loadLevel ();
		
		StartCoroutine(TimedUpdate());
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.R)){
			character.GetComponent<CharacterModel>().Die();
		}

		if(Input.GetKeyDown(KeyCode.I)){
			NextLevel();
		}
	}
		
	public void NextLevel(){
		character.GetComponent<CharacterModel> ().ClearTrail ();

		for (int i = 0; i < lists.Count; i++) {
			List<GameObject> component = lists [i];
			for (int j = 0; j < component.Count; j++) {
				Destroy (component[j]);
			}
			component.Clear ();
		}

		character.GetComponent<CharacterModel>().Respawn ();

		level ++;
		Debug.Log ("Entering level " + level);
		
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
				GameObject g = (tag.Equals("Heart")) ? Instantiate( Resources.Load("Heart3D", typeof(GameObject)) as GameObject) as GameObject : GameObject.CreatePrimitive(PrimitiveType.Cube);

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
				
				if(r.Contains("true"))
					g.AddComponent<Draggable>();

				if(g.GetComponent<BoxCollider>() != null)
					g.GetComponent<BoxCollider>().isTrigger = true;
				g.tag = tag;


				//Add all of the walls as children of emptyLevel
			
				g.transform.parent = emptyLevel.transform;

				//Change layer to 'Level', so the light hits these blocks
				g.layer = 8;


				if(tag.Equals("Wall")){
					//Don't render the game object. Instead, we will later draw a rect around the object
					//g.GetComponent<MeshRenderer>().enabled = false;
					//g.renderer.material.SetTexture("", GetTexture());
					g.AddComponent<BoundBoxes_BoundBox>();
					g.GetComponent<BoundBoxes_BoundBox>().lineColor = Color.white;
					g.renderer.material.color = Color.gray;
					g.renderer.material.mainTexture = TextureModel.GetTexture("Assets/Textures/construction_paper.png");
					
				}else if(tag.Equals("Lava")){
					//g.renderer.material.mainTexture = GetTexture("lava.png");
					g.AddComponent<TextureModel>();
					g.GetComponent<TextureModel>().dir = "Assets/Textures/lavatemp/";
					g.GetComponent<TextureModel>().imgprefix = "frame-0";
					g.renderer.material.color = Color.white;
					g.AddComponent<BoundBoxes_BoundBox>();
					g.GetComponent<BoundBoxes_BoundBox>().lineColor = Color.white;
				}else if(tag.Equals("Heart")){
					key = g;
					key.AddComponent<Rotator>();
					key.AddComponent<ColorChanger>();
					//InitHeart();
				}

				//Tile the texture, instead of stretching it
				Vector3 texturescale = new Vector3(g.transform.localScale.x/5, g.transform.localScale.y/5);
				g.renderer.material.mainTextureScale = texturescale;

				lists[i].Add (g);

				//Debug.Log(r);
			}
		}

		//After we have loaded all gameobjects to the level, make sure they all have the CollisionDetection script
		foreach (Transform child in emptyLevel.transform) {
			child.gameObject.AddComponent<CollisionDetection>();
		}
	}

	void InitHeart(){
		Light keylight = key.AddComponent<Light>() as Light;
		
		keylight.type = LightType.Directional;
		keylight.transform.position = new Vector3 (0, 0, -3.5f);
		keylight.range = 10;
		keylight.spotAngle = 100;
		keylight.intensity = .1f;

	}

	// Update is called once per frame
	IEnumerator TimedUpdate () {

		while (true) {


			if(key != null){
				key.transform.Rotate(Vector3.up * 5);
				key.renderer.material.color = character.renderer.material.color;
				//key.GetComponent<Light>().renderer.material.color = character.renderer.material.color;

			}

			//Wait x seconds before we preform another update
			yield return new WaitForSeconds (.025f);

		}

	}

	
}
