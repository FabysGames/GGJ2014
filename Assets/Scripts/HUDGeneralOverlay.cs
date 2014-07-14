using UnityEngine;
using System.Collections;

public class HUDGeneralOverlay : MonoBehaviour {

	public Texture2D separator;
	public Texture2D gamePausedTex;
	public GameController gameController;
	public KeyCode menuKey; 
	
	private float timeMenuKeyLastPressed = 0.0f;
	private bool escapeMenuActive = false;
	
	void OnGUI() 
	{	
		//GUI.depth = -1;
		GUI.DrawTexture (new Rect (Screen.width / 2 - 30, 0, 60, Screen.height), separator);


		if (escapeMenuActive) 
		{
			drawEscapeMenu ();
		}
	}

	void Update()
	{
		if (Input.GetKeyDown (this.menuKey) || Input.GetKeyDown (KeyCode.JoystickButton7)) 
		{
			escapeMenuActive = !escapeMenuActive;
			gameController.gameOver = escapeMenuActive;
		}
	}

	void drawEscapeMenu() 
	{
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height),"");
	
		GUI.DrawTexture (new Rect (Screen.width / 2 - 256, 0, 512, 512), gamePausedTex);

		if(GUI.Button(new Rect(Screen.width/2 - 100,450,200,30), "Exit Game")) {
			//Application.Quit();
			Application.LoadLevel("MainMenu");
		}
		if(GUI.Button(new Rect(Screen.width/2 - 100,480,200,30), "Resume")) {
			escapeMenuActive = false;
		}
	}
}
