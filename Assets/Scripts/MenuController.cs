using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	public GUISkin skin;
	public Texture2D bgOverlayUnstretched;
	public Texture2D bgStretched;

	void OnGUI(){

		GUI.skin = skin;

		GUI.DrawTexture (new Rect (0,0,Screen.width,Screen.height), bgStretched);


//		GUI.Box (new Rect (0,0,100,50), "Top-left");
//		GUI.Box (new Rect (Screen.width - 100,0,100,50), "Top-right");
//		GUI.Box (new Rect (0,Screen.height - 50,100,50), "Bottom-left");
//		GUI.Box (new Rect (Screen.width - 100,Screen.height - 50,100,50), "Bottom-right");

		GUI.DrawTexture (new Rect (Screen.width/2 - 512,0,1024,512), bgOverlayUnstretched);

		if(GUI.Button(new Rect(Screen.width/2 - 100,360,200,30), "Play")) {
			Application.LoadLevel("Track 01");
		}

//		if(GUI.Button(new Rect(Screen.width/2 - 100,390,200,30), "Level 2")) {
			//Application.LoadLevel(2);
		//}

		if(GUI.Button(new Rect(Screen.width/2 - 100,450,200,30), "Exit")) {
			Application.Quit();
		}
	}
	
}