using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	public Camera playerCamera;
	public GameObject playerCameraBase;			// Bezugsobjekt (z.B. am Fahrzeug), von dem aus die Kamera hoch geht

	public bool upOffsetEnabled = false;
	public float upOffsetMinDist = 7.0f;		// Erst danach gehts hoch
	public float upOffsetFactor = 0.1f;			// Skalierung fuer Hoehe
	public float upOffsetMaxHeight = 5.0f;		// Maximales Y-Offset

	public bool fovChangingEnabled = false;
	public float fovMinDist = 1.0f;
	public float fovMaxDist = 5.0f;
	public float fovValMinDist = 120;
	public float fovValMaxDist = 60;


	public Color blockdWireframeColor = new Color (1f, 1f, 1f, 1f);

	// Map mit (farb)geaenderten Objekten und der Originalfarbe
	private static Dictionary<GameObject, Color> modifiedObjects = new Dictionary<GameObject, Color>();

	void LateUpdate()
	{
		// Auf Fahrzeug gucken
		playerCamera.transform.LookAt(transform.FindChild("Anchor").position, transform.up);

		// Kamera nach oben schieben
		OffsetCamera();

		// Kamera FOV aendern
		ChangeFOV ();

		// Objekte durchsichtig machen
		CheckCameraLOS ();
	}

	void ChangeFOV() 
	{
		if (fovChangingEnabled) 
		{
			float dist = Vector3.Distance (playerCamera.transform.position, transform.position);
			if(dist <= fovMinDist) 
				playerCamera.fieldOfView = fovValMinDist;
			else if(dist >= fovMaxDist)
				playerCamera.fieldOfView = fovValMaxDist;
			else 
			{
				float factor = (dist - fovMinDist) / (fovMaxDist - fovMinDist);
				playerCamera.fieldOfView = fovValMinDist + (fovValMaxDist - fovValMinDist) * factor;
				//Debug.Log("dist=" + dist + "; factor=" + factor);
			}
		}

	}


	void CheckCameraLOS ()
	{
		// Alle Objekte finden und faerben, welche die Sichtlinie verdecken
		List<GameObject> losBlockingObjects = GetLOSBlockingObjects (transform.gameObject, playerCamera.gameObject);
		foreach (GameObject blockingObject in losBlockingObjects) 
		{
			if (!modifiedObjects.ContainsKey (blockingObject)) {

				try{	
					if(blockingObject.tag != "Obstacle")
					{

						MeshFilter meshFilter = blockingObject.GetComponent<MeshFilter>();
						int[] indices = meshFilter.mesh.GetIndices(0);
						meshFilter.mesh.SetIndices(indices,MeshTopology.Lines,0);

						modifiedObjects.Add (blockingObject, blockingObject.renderer.material.color);
						blockingObject.renderer.material.color = blockdWireframeColor;
					}
				} catch(MissingComponentException) {
					// Falls Objekt keinen Renderer hat dann machen wir es auch nicht durchsichtig.
				} catch(MissingReferenceException) {
					// Falls Objekt schon zerstoert wurde
				} 
			
			}	
		}

		// Bereits gefaerbte Objekte aufraeumen
		List<GameObject> objToRemove = new List<GameObject> ();
		foreach (KeyValuePair<GameObject, Color> pair in modifiedObjects) {
			// Alle nicht aktuell die Sicht blockierenden Objekte zurueckfaerben und zum Loeschen markieren
			if (!losBlockingObjects.Contains(pair.Key)) {
				objToRemove.Add (pair.Key);

				try {
					MeshFilter meshFilter = pair.Key.GetComponent<MeshFilter>();
					int[] indices = meshFilter.mesh.GetIndices(0);
					meshFilter.mesh.SetIndices(indices,MeshTopology.Triangles,0);
					pair.Key.renderer.material.color = pair.Value;
				} catch(MissingComponentException) {
					// Falls Objekt keinen Renderer hat dann machen wir es auch nicht durchsichtig.
				} catch(MissingReferenceException) {
					// Falls Objekt schon zerstoert wurde
				} 
			}
		}
		foreach (GameObject gObj in objToRemove)
			modifiedObjects.Remove (gObj);
	}

	void OffsetCamera()
	{
		// Kamera dynamisch nach oben bewegen
		if (upOffsetEnabled && playerCameraBase != null) {
			float dist = Vector3.Distance (playerCamera.transform.position, transform.position);
			// Je weiter weg desto hoeher die Kamera
			float cameraUpOffset = 0.0f;
			if (dist > upOffsetMinDist) {
				cameraUpOffset = (dist - upOffsetMinDist) * upOffsetFactor;
				cameraUpOffset *= cameraUpOffset;
				cameraUpOffset = Mathf.Min (cameraUpOffset, upOffsetMaxHeight);
			}
			Vector3 upDirection = new Vector3 (0, 1, 0);
			playerCamera.transform.localPosition = playerCameraBase.transform.localPosition + (upDirection * cameraUpOffset);
		}
	}
	
	public List<GameObject> GetLOSBlockingObjects(GameObject agent, GameObject target)
	{
		List<GameObject> blockingObjects = new List<GameObject> ();

		// Raycasts zwischen den Objekten
		RaycastHit[] hits;
		hits = Physics.RaycastAll(agent.transform.position, 
		                          target.transform.position - agent.transform.position, 
		                          Vector3.Distance(agent.transform.position, target.transform.position));
		foreach (RaycastHit hit in hits) 
		{
			if (hit.transform.Equals(target.transform) || hit.transform.IsChildOf(target.transform) || target.transform.IsChildOf(hit.transform)
			    || hit.transform.IsChildOf(agent.transform) || agent.transform.IsChildOf(hit.transform))
				// Raycast trifft aufs Ziel: zaehlt nicht
				continue;
			else
				// Raycast trifft auf ein Object dazwischen
				blockingObjects.Add(hit.transform.gameObject);
		}
	
		return blockingObjects;
	}
}
