using UnityEngine;
using System.Collections;

public class CharacterModel : MonoBehaviour {

//int
	private int offset = 2;

//Texture
	private Texture2D text;
		
//GameObject
	public GameObject[] trail = new GameObject[10];

	// Use this for initialization
	void Start () {
		StartCoroutine (TimedUpdate());

		text = TextureModel.GetTexture("Assets/Textures/construction_paper.png");
	}
	
	// Update is called once per frame
	void Update () {

		float newx = transform.position.x;
		float newy = transform.position.y;
		float newz = transform.position.z;
		
		bool changedpos = false;
		//If the player goes off screen, make sure we reset 
		if (transform.position.x < -Camera.main.orthographicSize*2 + offset) {
			changedpos = true;
			newx = Camera.main.orthographicSize*2 - offset;
		}else if(transform.position.x > Camera.main.orthographicSize*2 - offset){
			changedpos = true;
			newx = -Camera.main.orthographicSize*2 + offset;
		}
		
		if (transform.position.z < -Camera.main.orthographicSize*2 - offset) {
			changedpos = true;
			newz = Camera.main.orthographicSize*2 - offset;
		}else if(transform.position.z > Camera.main.orthographicSize*2 - offset){
			changedpos = true;
			newz = -Camera.main.orthographicSize*2 + offset;
		}
		
		if (transform.position.y < -Camera.main.orthographicSize) {
			changedpos = true;
			newy = Camera.main.orthographicSize;
		}else if(transform.position.y > Camera.main.orthographicSize){
			changedpos = true;
			newy = -Camera.main.orthographicSize;
		}
		
		
		Vector3 newpos = new Vector3 (newx, newy, newz);
		transform.position = newpos;
	}

	public void Die(){
		ClearTrail ();
		Respawn ();
	}

	public void ClearTrail(){
		for(int i = 0; i < trail.Length; i ++){
			Destroy (trail[i]);
		}
	}

	public void Respawn(){
		GetComponent<CharacterController> ().isJumping = false;

		float newx = transform.position.x;
		float newy = transform.position.y;
		float newz = transform.position.z;
		
		newx = -Camera.main.orthographicSize * 2 + 5;
		newy = Camera.main.orthographicSize;
		newz = -Camera.main.orthographicSize * 2 + 5;
		
		transform.position = new Vector3 (newx, newy, newz);
	}

	// Update is called once per frame
	IEnumerator TimedUpdate () {
		while (true) {
			
			//Move everything in the array back one element
			//Slot 0 should be the current transform of the character
			for (int i = 0; i < trail.Length - 1; i++) {
				if(trail[i+1] != null){
					if(trail[i] == null){
						trail[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
						trail[i].renderer.material.mainTexture = text;

						//make the trail rotate
						//trail[i].AddComponent<Rotator>();
						
						//trail[i].GetComponent<Rotator>().SetAxis(Vector3.forward);
						//trail[i].GetComponent<Rotator>().SetSpeed (1);
					}
					
					Color oldcolor = trail [i + 1].renderer.material.color;
					Vector3 oldscale = trail[i+1].transform.localScale;
					//trail [i] = trail[i+1].GetComponent<SpriteRenderer> ().sprite;

					float shrinkspeed = .075f;
					trail [i].transform.position = trail[i+1].transform.position;
					trail [i].transform.localScale = new Vector3(oldscale.x - shrinkspeed, oldscale.y - shrinkspeed, oldscale.z - shrinkspeed);
					trail [i].renderer.material.color = new Color(oldcolor.r, oldcolor.g, oldcolor.b, oldcolor.a - .1f);
					trail [i].collider.enabled = false;



					//Debug.Log ("Color: " + trail[i].renderer.material.color);
					
				}
				
			}
			
			if(trail[9] == null){
				trail[9] = GameObject.CreatePrimitive(PrimitiveType.Cube);
			}
			
			trail [9].transform.position = transform.position;
			trail [9].transform.localScale = transform.localScale;
			trail [9].renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, renderer.material.color.a - .1f);
			
			//Debug.Log ("old a new a: " + renderer.material.color.a +", "+ (renderer.material.color.a - .1f+""));
			
			//Remove the box collider, so you cannot jump off of yourself
			trail[9].collider.enabled = false;
			
			//Wait x seconds before we preform another update
			yield return new WaitForSeconds (.05f);
			
		}
		
	}
	
}
