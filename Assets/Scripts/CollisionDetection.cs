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
		Debug.Log ("Other" + other.tag);
		Debug.Log ("Me" + collider.tag);
		if(collider.tag.Equals("Lava")){
			levelmodel.GetComponent<LevelModel>().Die();
		}else if(collider.tag.Equals("Key")){
			levelmodel.GetComponent<LevelModel>().NextLevel();
		}
	}
}
