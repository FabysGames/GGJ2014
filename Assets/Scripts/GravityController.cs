using UnityEngine;
using System.Collections;

public class GravityController : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		VehicleController vehicle = other.GetComponent<VehicleController>();
		if (vehicle != null)
		{
			vehicle.setGravity(true);
		}
	}
}
