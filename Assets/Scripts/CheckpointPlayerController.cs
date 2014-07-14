using UnityEngine;
using System.Collections;

public class CheckpointPlayerController : MonoBehaviour 
{
	public int checkPointPassed = 0;
	public int roundsCompleted = 0;

	public int CheckPointPassed { get { return this.checkPointPassed; } set { this.checkPointPassed = value; } }
	public int RoundsCompleted { get { return this.roundsCompleted; } set { this.roundsCompleted = value; } }
	
	void Start()
	{
		CheckpointController cpController = GameObject.FindGameObjectWithTag ("CheckpointController").GetComponent<CheckpointController> ();
	    cpController.CheckpointPlayerControllers.Add (this);
	}
}
