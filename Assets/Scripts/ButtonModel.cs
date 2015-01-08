using UnityEngine;
using System.Collections;

public class ButtonModel : MonoBehaviour {

	public LevelBuilder lb;

	public void SelectWall(){
		lb.RemoveLastBlock ();
		lb.blockToPlace = "Wall";
		Debug.Log ("Wall");
	}

	public void SelectLava(){
		lb.RemoveLastBlock ();
		lb.blockToPlace = "Lava";
		Debug.Log ("Lava");
		
	}

	public void SelectKey(){
		lb.RemoveLastBlock ();
		lb.blockToPlace = "Key";
		Debug.Log ("Key");
		
	}

	public void Cancel(){
		lb.RemoveLastBlock ();
		Debug.Log ("Cancelling");
	}

	public void Save(){
		lb.RemoveLastBlock ();
		Debug.Log ("Saving");
		lb.SaveWorld ();
	}
}
