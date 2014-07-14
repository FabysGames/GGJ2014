using UnityEngine;
using System.Collections;


public class Checkpoint : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		CheckpointPlayerController cpController = null;

		ColliderRoot colliderRoot = other.GetComponent<ColliderRoot> ();

		if (colliderRoot != null) 
		{
			cpController = colliderRoot.root.GetComponent<CheckpointPlayerController> ();
		} else 
		{
			cpController = other.GetComponent<CheckpointPlayerController> ();
		}

		if(cpController != null)
		{
			cpController.checkPointPassed++;
		}
	}
}
