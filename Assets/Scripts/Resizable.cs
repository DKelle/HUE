using UnityEngine;
using System.Collections;

public class Resizable : MonoBehaviour 
{
	Ray olddraglocation;
	Ray currentdraglocation;

	private Vector2 screenPoint;
	private Vector2 offset;

	public bool foundblock = false;


	// array for storing if the 3 mouse buttons are dragging
	private bool[] isDragActive;
	
	// for remembering if a button was down in previous frame
	private bool[] downInPreviousFrame;

	void Start(){
		isDragActive = new bool[] {false, false, false};
		downInPreviousFrame = new bool[] {false, false, false};
	}


	void OnDragging(int mousebutton)
	{
		//If left click, user is resizing the block
		if (mousebutton == 0) {

			//If we don't round here, the amount that the block scales can depend on the speed at which the mouse moves
			currentdraglocation = Camera.main.ScreenPointToRay (Input.mousePosition);
			currentdraglocation.origin = new Vector2 (Mathf.Round (currentdraglocation.origin.x), Mathf.Round (currentdraglocation.origin.y));

			int dx = (int)(Mathf.Round (olddraglocation.origin.x - currentdraglocation.origin.x));
			int dy = (int)(Mathf.Round (olddraglocation.origin.y - currentdraglocation.origin.y));

			Vector2 scale = new Vector2 (transform.localScale.x + dx, transform.localScale.y + dy);
			Vector2 newpos = new Vector2 (transform.position.x - (dx / 2.0f), transform.position.y - (dy / 2.0f));

			transform.localScale = scale;
			transform.position = newpos;

			olddraglocation = currentdraglocation;
		}

		//If right click, the user is trying to move the block
		else if(mousebutton == 1){
			//Debug.Log("moving blcok");
		}
	}

	void OnDraggingStart(int mousebutton){
		if (mousebutton == 0) {
			//Debug.Log ("just started dragging");
			olddraglocation = Camera.main.ScreenPointToRay (Input.mousePosition); 
		}

	}

	void Update(){
	
		if (foundblock) {
			for (int i=0; i < isDragActive.Length; i++){
				if (Input.GetMouseButton(i)){
					if (downInPreviousFrame[i]){
						if (isDragActive[i]){
							OnDragging(i);
						}
						else{
							isDragActive[i] = true;
							OnDraggingStart(i);
						}
					}
					downInPreviousFrame[i] = true;
				}else{
					if (isDragActive[i]){
						isDragActive[i] = false;
					}
					downInPreviousFrame[i] = false;
				}
			}
		}


		if (Input.GetMouseButtonUp (0)) {
			foundblock = false;
		}

	}

	void OnMouseDown(){
		Debug.Log ("mouse down");
		foundblock = true;
	}

	void OnMouseOver(){
		if(Input.GetMouseButtonDown(0)){
			//This signifies that we have clicked on the block, so we can start updating position and scale
			foundblock = true;
		}
	}

	
	
}