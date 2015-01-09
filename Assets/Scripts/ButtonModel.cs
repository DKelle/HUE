using UnityEngine;
using System.Collections;

public class ButtonModel : MonoBehaviour {

	public LevelBuilder lb;

	public void SelectWall(){
		lb.RemoveLastBlock ();
		lb.blockToPlace = "Wall";
		Debug.Log ("Wall");
	}

	public void SelectDraggableWall(){
		lb.RemoveLastBlock ();
		lb.blockToPlace = "DraggableWall";
		Debug.Log ("Draggable Wall");
	}
	
	public void SelectLava(){
		lb.RemoveLastBlock ();
		lb.blockToPlace = "Lava";
		Debug.Log ("Lava");
		
	}

	public void SelectHeart(){
		lb.RemoveLastBlock ();
		lb.blockToPlace = "Heart";
		Debug.Log ("Heart");
		
	}

	public void Undo(){
		lb.RemoveLastBlock ();
		lb.RemoveLastBlock ();
		Debug.Log ("Undo");
	}

	public void Save(){
		lb.RemoveLastBlock ();
		Debug.Log ("Saving");
		lb.SaveWorld ();
	}
}
