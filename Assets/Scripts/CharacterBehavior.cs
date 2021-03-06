﻿using UnityEngine;
using System.Collections;
//using System.Diagnostics;

public class CharacterBehavior : MonoBehaviour {


//int
	public readonly int UP = 0;
	public readonly int DOWN = 3;
	public readonly int LEFT = 1;
	public readonly int RIGHT = 2;
	public int jumpheight = 5;

//bool
	public bool[] dirs = new bool[3];
	public bool[] canmove = new bool[4];
	public bool isJumping = false;

//Misc
	public Transform[] startTransforms = new Transform[4];
	public Transform[]   endTransforms = new Transform[4];

	public SpriteRenderer sr;


	// Use this for initialization
	void Start () {
		//s.Start ();
		StartCoroutine(TimedUpdate());
		sr.color = new Color (.1f, .2f, .3f);
	
	}


	//FixedUpdated is called every physics update
	void FixedUpdate() {

		//Make sure you are on the ground before you jump
		if(Input.GetKey("up")){
			dirs[UP] = true;
		}else{
			dirs[UP] = false;
		}

		if(Input.GetKey("left")){
			dirs[LEFT] = true;
		}else{
			dirs[LEFT] = false;
		}

		if(Input.GetKey("right")){
			dirs[RIGHT] = true;
		}else{
			dirs[RIGHT] = false;
		}

	}

	//Runs every half second
	IEnumerator TimedUpdate(){
		while (true) {

			float distance = 1f;

			//Before we detect key input, check to see which directions we can move without collisions


			//UP
			canmove[UP] = !Physics2D.Linecast(startTransforms[UP].position, endTransforms[UP].position);

			//If we hit a block, stop jumping
			if(!canmove[UP]){
				isJumping = false;
				jumpheight = 5;
			}

			//DOWN
			canmove[DOWN] = !Physics2D.Linecast(startTransforms[DOWN].position, endTransforms[DOWN].position);

			
			Debug.DrawLine(startTransforms[RIGHT].position, endTransforms[RIGHT].position, Color.red);

			//Detect key inputs, and change the characters position
			Vector2 newpos = transform.position;
			if (dirs [UP] && canmove[UP] && !canmove[DOWN]) {
				//
				isJumping = true;
			} 

			if(canmove[DOWN] && !isJumping){
				newpos.y = transform.position.y - distance;
			}else {
				newpos.y = transform.position.y;
			}

			if(isJumping){
				if(jumpheight > 1){
					newpos.y = transform.position.y + distance;
					jumpheight --;
				}else{
					jumpheight = 5;
					isJumping = false;
				}
			}
			
			//The reason I am not putting all collision detection in the same block is because 
			//that would allow for the player to move diagonally through a wall
			transform.position = newpos;

			//RIGHT
			canmove[RIGHT] = !Physics2D.Linecast(startTransforms[RIGHT].position, endTransforms[RIGHT].position);
			
			//LEFT
			canmove[LEFT] = !Physics2D.Linecast(startTransforms[LEFT].position, endTransforms[LEFT].position);
			
			if (dirs [LEFT] && canmove[LEFT]) {
				newpos.x = transform.position.x - distance;
			
			} else if (dirs [RIGHT] && canmove[RIGHT]) {
				newpos.x = transform.position.x + distance;
			} else {
				newpos.x = transform.position.x;
			}

			transform.position = newpos;

			//Wait x seconds before we preform another update
			yield return new WaitForSeconds (.05f);
		}
	}


	// Update is called once per frame
	void Update () {

		HSLColor charcolor = new HSLColor (sr.color);
		//Debug.Log ("Before color change:" + charcolor.h);
		charcolor.h += .75f;
		charcolor.s += .001f;
		//Debug.Log ("After color change:" + charcolor.l);

		Color newc = charcolor.ToRGBA ();
		//Debug.Log ("newc: " + newc.r); 

		sr.color = charcolor.ToRGBA ();
		//Debug.Log ("R: " + sr.color.r);
		//Debug.Log ("B: " + sr.color.b);
		//Debug.Log ("G: " + sr.color.g);
		//do something
		//float elapsed = 0f + s.Elapsed.Milliseconds;

		//sr.color = new Color (Mathf.Sin(elapsed), Mathf.Sin(elapsed), Mathf.Sin(elapsed));
	}
}
