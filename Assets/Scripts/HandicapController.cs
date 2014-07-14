using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HandicapController : MonoBehaviour 
{
	public int treshhold;
	public int baseHandicap;

	CheckpointController cpController;

	void Start()
	{
		this.cpController = GameObject.FindGameObjectWithTag ("CheckpointController").GetComponent<CheckpointController> ();
	}

	void Update()
	{
		List<CheckpointPlayerController> sortedList = this.cpController.CheckpointPlayerControllers.OrderByDescending(x => x.checkPointPassed).ToList();

		int i = 0;
		foreach (CheckpointPlayerController cpPlayerController in sortedList) 
		{
			if(sortedList.Count <= i + 1) continue;

			int differents = cpPlayerController.checkPointPassed - sortedList[i+1].checkPointPassed;

			float handicap = 0;
			if(differents > this.treshhold)
			{
				handicap = Mathf.Clamp((this.baseHandicap * (differents - this.treshhold)) / 100f, 0, 1);
			}

			bool isHandicapped = handicap == 0 ? false : true;

			cpPlayerController.GetComponent<HUDPlayerInfo>().SetHandicap(isHandicapped);

			// Debug.Log(cpPlayerController.gameObject.name + ": " + handicap);

			cpPlayerController.GetComponent<VehicleSplineController>().setSpeedFactor(1.0f - handicap);

			i++;
		}
	}
}
