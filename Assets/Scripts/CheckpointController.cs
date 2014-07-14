using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckpointController : MonoBehaviour 
{
	List<CheckpointPlayerController> checkpointPlayerControllers = new List<CheckpointPlayerController>();

	public List<CheckpointPlayerController> CheckpointPlayerControllers 
	{
		get { return this.checkpointPlayerControllers; }
	}
}
