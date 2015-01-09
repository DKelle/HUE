using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour {

	public GameObject character;
	public GameObject levelmodel;

	// Use this for initialization
	void Start () {
		character = GameObject.Find ("Character");
		levelmodel = GameObject.Find ("LevelManager");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnTriggerEnter(Collider other) {
	
		if(collider.tag.Equals("Lava")){
			character.GetComponent<CharacterModel>().Die();
		}else if(collider.tag.Equals("Heart")){
			//Debug.Log ("Collision with key detected");
			if(levelmodel.GetComponent<LevelModel>() != null)
				levelmodel.GetComponent<LevelModel>().NextLevel();
		}
	}
}
