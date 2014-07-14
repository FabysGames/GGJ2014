using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	public int mapsToWin;
	public bool started = false;

	public PlayerController player1;
	public PlayerController player2;
	public GameObject hudExtra;

	CheckpointController cpController;

	public bool gameOver = false;
	string winner = "";

	public Texture2D player1Won;
	public Texture2D player2Won;

	void OnGUI()
	{
		if (!this.started) 
		{
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height),"");

			if(GUI.Button(new Rect(Screen.width/2 - 100,250,200,30), "Start")) {

					this.started = true;
			}
		}

		if (winner.Length > 0) 
		{
			// Welcher Spieler hat gewonnen

			if(this.winner == "Player A")
				GUI.DrawTexture (new Rect (Screen.width/2 - 256,-20,512,512), player1Won);
			else 
				GUI.DrawTexture (new Rect (Screen.width/2 - 256,-20,512,512), player2Won);



			if(GUI.Button(new Rect(Screen.width/2 - 200,500,150,30), "Restart")) {
				Application.LoadLevel(Application.loadedLevel);
			}
			if(GUI.Button(new Rect(Screen.width/2 + 50,500,150,30), "Exit")) {
				Application.LoadLevel("MainMenu");
			}
			

			//GUI.Label(new Rect(100, 100, 300, 50), "Player "+this.winner+" won!!!");
		}
	}

	void Start () 
	{
		this.cpController = GameObject.FindGameObjectWithTag ("CheckpointController").GetComponent<CheckpointController> ();
	
		this.player1.GetComponent<HUDPlayerInfo> ().TotalLaps = this.mapsToWin;
		this.player2.GetComponent<HUDPlayerInfo> ().TotalLaps = this.mapsToWin;
	}
	
	void Update () 
	{
		if (this.started == false && Input.GetKeyDown (KeyCode.JoystickButton0))
						this.started = true;

		if (!this.GameIsActive()) return;

		foreach (CheckpointPlayerController cpPlayerController in this.cpController.CheckpointPlayerControllers) 
		{
			if(cpPlayerController.RoundsCompleted == this.mapsToWin)
			{
				this.WonGame(cpPlayerController.GetComponent<PlayerController>());
			}
		}
	}

	public void LostGame(PlayerController playerController)
	{
		if (playerController == this.player1)
			this.WonGame (this.player2);
		else
			this.WonGame (this.player1);
	}

	public void WonGame(PlayerController playerController)
	{
		audio.Play();

		this.gameOver = true;
		this.winner = playerController.gameObject.name;
		playerController.GetComponentInChildren<Collider> ().enabled = false;
		playerController.won = true;

		Camera camera1 = this.player2.GetComponentInChildren<Camera> ();
		Camera camera2 = this.player1.GetComponentInChildren<Camera> ();

		if (playerController == this.player1) 
		{
			camera1.rect = new Rect (0, 0, 1, 1);
			camera1.depth = 1;
		} else 
		{
			camera2.rect = new Rect (0, 0, 1, 1);
			camera2.depth = 1;
		}

		camera1.GetComponent<HUDPlayerCam>().enabled = false;
		camera2.GetComponent<HUDPlayerCam>().enabled = false;
		this.hudExtra.SetActive (false);
	}

	public bool GameIsActive()
	{
		return this.started && !this.gameOver;
	}
}
