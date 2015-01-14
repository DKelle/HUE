using UnityEngine;
using System.Collections;
using System.IO;

public class TextureModel : MonoBehaviour {
	public string dir;
	public string imgprefix;

	public Texture2D[] textures;
	int textNumber;
	int imgs = 75;

	void Start(){
		DirectoryInfo di = new DirectoryInfo (dir);
		//imgs = di.GetFiles ().

		textures = new Texture2D[imgs];

		for (int i = 0; i < imgs; i ++) {
			string path = (i > 9) ? dir+""+imgprefix+""+i+".png" : dir+""+imgprefix+"0"+i+".png";
			textures[i] = GetTexture(path);
		}
		
		
		textNumber = 0;
	}

	// Update is called once per frame
	void Update () {
		textNumber = (textNumber + 1) % imgs;
		renderer.material.mainTexture = textures [textNumber];
	}

	public static Texture2D GetTexture(string path){
		Texture2D text = null;

		if(System.IO.File.Exists(path)){
			Debug.Log ("Path exists: " + path);
			byte[] fileData = System.IO.File.ReadAllBytes (path);
			text = new Texture2D (1, 1);
			text.LoadImage (fileData);
		}else{
			Debug.Log ("Does not exist : " + path);
		}
		return text;
		
	}
}
