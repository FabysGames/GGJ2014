using UnityEngine;
using System.Collections;

public class HUDPlayerCam : MonoBehaviour {

	public GUISkin skin;
	public HUDPlayerInfo hudPlayerInfo;
	public int playerPos = 0;

	public Texture2D playerLives3;
	public Texture2D playerLives2;
	public Texture2D playerLives1;

	void OnGUI() 
	{
		Texture2D playerLives = null;
		if (hudPlayerInfo.Lives >= 3)
			playerLives = playerLives3;
		else if(hudPlayerInfo.Lives == 2)
			playerLives = playerLives2;
		else if(hudPlayerInfo.Lives == 1)
			playerLives = playerLives1;

		
		GUI.DrawTexture (new Rect (camera.pixelRect.x + 15, camera.pixelRect.y,
		                           100,50), playerLives);
		GUI.skin.label.fontSize = 32;
		GUI.Label (new Rect (camera.pixelRect.x + camera.pixelWidth - 160, camera.pixelRect.y,
		                     150,50), "Lap: " + hudPlayerInfo.CompleteLaps + " / " + hudPlayerInfo.TotalLaps);

		GUI.color = Color.black;
	/*	GUI.Label (new Rect (camera.pixelRect.x + camera.pixelWidth - 160, camera.pixelRect.y + camera.pixelHeight - 50,
		                     150,50), "Pos: " + this.playerPos + " / 2");
*/
		GUI.color = Color.red;
		if(hudPlayerInfo.SpielerIsHandicapped)
			GUI.Label (new Rect (camera.pixelRect.x + 18, camera.pixelRect.y + camera.pixelHeight - 50,
			               		          		    		                   200,50), "HANDICAP");

	}

	void Update()
	{
		//Debug.Log (transform.rotation.y );
		if (transform.rotation.y > 0.5)
				this.playerPos = 1;
		else
				this.playerPos = 2;
	}
}
