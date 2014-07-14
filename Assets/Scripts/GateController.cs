using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour 
{
	public Transform resetPos;
	public PlayerController player1;
	public PlayerController player2;

	void OnTriggerEnter(Collider other)
	{
		var relativePosition = transform.InverseTransformPoint(other.transform.position);
		ColliderRoot colliderRoot = other.GetComponent<ColliderRoot> ();

		if (relativePosition.z > 0) 
		{
			if (colliderRoot != null) 
			{
				colliderRoot.root.transform.position = this.resetPos.position;
			} else 
			{
				other.transform.position = this.resetPos.position;
			}

			return;
		}

		CheckpointPlayerController cpPlayerController = null;

        if (colliderRoot != null) 
		{
			cpPlayerController = colliderRoot.root.GetComponent<CheckpointPlayerController> ();
		} else 
		{
			cpPlayerController = other.GetComponent<CheckpointPlayerController> ();
        }
        
      	
		cpPlayerController.RoundsCompleted++;
		cpPlayerController.GetComponent<HUDPlayerInfo> ().CompleteLaps++;

		if (this.player1.gameObject != cpPlayerController.gameObject) 
		{
			CheckpointPlayerController p1cp = this.player1.GetComponent<CheckpointPlayerController>();

			if(p1cp.RoundsCompleted < cpPlayerController.RoundsCompleted)
				this.player1.DamagePlayer ();
		} else 
		{
			CheckpointPlayerController p2cp = this.player2.GetComponent<CheckpointPlayerController>();

			if(p2cp.RoundsCompleted < cpPlayerController.RoundsCompleted)
				this.player2.DamagePlayer();
		}
	}
}
