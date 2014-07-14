using UnityEngine;
using System.Collections;

public class HUDPlayerInfo : MonoBehaviour 
{
	public int CompleteLaps { get; set; }
	public int TotalLaps { get; set; }
	public bool SpielerIsHandicapped { get; set; }
	public int Lives { get; set; }

	public void SetHandicap(bool handicap)
	{
		this.SpielerIsHandicapped = handicap;
	}
}
