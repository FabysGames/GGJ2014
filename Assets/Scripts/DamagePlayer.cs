using UnityEngine;
using System.Collections;

public class DamagePlayer : MonoBehaviour 
{
	public GameObject explosion;
	public bool doDamage = false;
	public float slowToSpeed = 0.75f;

	void OnTriggerEnter(Collider other)
	{
		ColliderRoot colliderRoot = other.GetComponent<ColliderRoot> ();
		this.audio.Play ();

		PlayerController playerController = null;
		
		if (colliderRoot != null) 
		{
			playerController = colliderRoot.root.GetComponent<PlayerController> ();
		} else 
		{
			playerController = other.GetComponent<PlayerController> ();
		}

		playerController.GetComponent<VehicleSplineController> ().speed *= this.slowToSpeed;

		if (doDamage)
				playerController.DamagePlayer ();


		if (this.explosion != null) 
		{
			GameObject go = Instantiate (this.explosion, new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity) as GameObject;
			go.transform.parent = transform.parent;
		}

		StartCoroutine ("DisableObject");
	}

	IEnumerator DisableObject()
	{
		collider.enabled = false;
		Color color = renderer.material.color;
		renderer.material.color = new Color (color.r, color.g, color.b, 0.6f);

		yield return new WaitForSeconds(3.4f);

		renderer.material.color = new Color (color.r, color.g, color.b, 1);

		collider.enabled = true;
	}
}
