using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TrackSplineGenerator : MonoBehaviour 
{
	public bool generate = false;
	public float weldMagnitude = 0.1f;

	public Transform[] trackParts;
	public GameObject splineRoot;

	void Awake()
	{
		trackParts = GetComponentsInChildren<Transform>().Where(t => t.name.StartsWith("Part_")).OrderBy(t => t.name).ToArray();

		if (this.generate)
			Generate();
	}
	
	void Generate()
	{
		List<Vector3> controlPoints = new List<Vector3>();
		List<Quaternion> controlPointsRotation = new List<Quaternion>();
		List<string> originalNames = new List<string>();

		foreach (Transform trackPart in trackParts)
		{
			foreach (Transform transform in trackPart.GetComponentsInChildren<Transform>().Where(t => t.name.StartsWith("spline_")).OrderBy(t => t.name))
			{
				controlPoints.Add(transform.position);
				controlPointsRotation.Add(transform.rotation);
				originalNames.Add(transform.name);
			}
		}

		List<int> pointsToDelete = new List<int>();
		for (int i = 0; i < controlPoints.Count - 1; ++i)
		{
			if (Vector3.Distance(controlPoints[i], controlPoints[i + 1]) < this.weldMagnitude)
				pointsToDelete.Add(i);
		}

		for (int i = pointsToDelete.Count - 1; i >= 0; --i)
		{
			controlPoints.RemoveAt(pointsToDelete[i]);
			controlPointsRotation.RemoveAt(pointsToDelete[i]);
			originalNames.RemoveAt(pointsToDelete[i]);
		}

		for (int i = 0; i < controlPoints.Count; ++i)
		{
			string name = "spline_" + i.ToString().PadLeft(4, '0');
			if(originalNames[i]=="spline_9999")
				name = originalNames[i];

			GameObject splinePoint = new GameObject(name);
			splinePoint.transform.parent = splineRoot.transform;
			splinePoint.transform.position = controlPoints[i];
			splinePoint.transform.rotation = controlPointsRotation[i];
		}
	}
}
