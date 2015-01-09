using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {

	private Ray currentdraglocation;
	private Ray olddraglocation;

	private bool dragging;

	public bool mouseover = false;

	// array for storing if the 3 mouse buttons are dragging
	private bool isDragActive;
	
	// for remembering if a button was down in previous frame
	private bool downInPreviousFrame;

	void Start(){
		isDragActive = false;
		downInPreviousFrame = false;

		currentdraglocation = new Ray ();
		olddraglocation = new Ray ();
	}



	// Update is called once per frame
	void Update(){
		
		bool anybuttondown = false;
		

		if (Input.GetMouseButton(0) ){
			anybuttondown = true;
			
			if (downInPreviousFrame && mouseover){
				if (isDragActive){
					OnDragging();
				}
				else{
					isDragActive = true;
					OnDraggingStart();
				}
			}else if(mouseover){
				downInPreviousFrame = true;
			}
		}else{
			if (isDragActive){
				OnDraggingStop();
				isDragActive = false;
			}
			
			downInPreviousFrame = false;
		}

		
		if (!anybuttondown) {
			mouseover = false;
		}
		
	}
	
	void OnMouseOver(){
		mouseover = true;
	}

	void OnDraggingStart(){
		//Debug.Log ("just started dragging");
		olddraglocation = Camera.main.ScreenPointToRay (Input.mousePosition); 
		
	}

	void OnDragging(){

		dragging = true;

		currentdraglocation = Camera.main.ScreenPointToRay (Input.mousePosition);
		currentdraglocation.origin = new Vector2 (Mathf.Round (currentdraglocation.origin.x), Mathf.Round (currentdraglocation.origin.y));

		int dx = (int)(Mathf.Round (olddraglocation.origin.x - currentdraglocation.origin.x));
		int dy = (int)(Mathf.Round (olddraglocation.origin.y - currentdraglocation.origin.y));

		Vector2 newpos = new Vector2 (transform.position.x - dx, transform.position.y - dy);
		
		transform.position = newpos;

		olddraglocation = currentdraglocation;

	}

	void OnDraggingStop(){
		mouseover = false;
	}

}
