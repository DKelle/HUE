using UnityEngine;
using System.Collections;

public class ColorChanger : MonoBehaviour {

	//float
	public float r;
	public float g;
	public float b;
	
	public float dr;
	public float dg;
	public float db;

	// Use this for initialization
	void Start () {
		r = 3;
		g = 3;
		b = 3;

		dr = Random.Range (.01f, .05f);
		dg = Random.Range (.01f, .05f);
		db = Random.Range (.01f, .05f);
	
		renderer.material.color = new Color (r, g, b);
	}

	// Update is called once per frame
	void Update () {

		renderer.material.color = Step();

	}

	public Color Step(){
		IndividualStep (ref r, ref dr);
		IndividualStep (ref g, ref dg);
		IndividualStep (ref b, ref db);
		
		return new Color (r, g, b);
	}
	
	private void IndividualStep(ref float x, ref float dx){
		//The value has become too high, reverse the direction
		if (x + dx > 3) {
			dx *= -1;
		} else if (x + dx < 1) {
			dx *= -1;
		}
		
		x += dx;
	}
	

}
