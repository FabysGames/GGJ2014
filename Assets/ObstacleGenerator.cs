using UnityEngine;
using System.Collections;

public class ObstacleGenerator : MonoBehaviour 
{
	public GameObject[] prefabs;

	public Transform follower;
	public Vector2 spawnRange = new Vector2(5.0f, 10.0f);
	public int maxObstacles = 48;

	float nextSpawn = 0;
	int obstacles = 0;

	void Awake()
	{
		this.follower = transform.FindChild("Follower");

		nextSpawn = Random.Range(spawnRange.x, spawnRange.y);
	}

	void LateUpdate()
	{
		nextSpawn -= Time.deltaTime;

		if (nextSpawn < 0.0f)
		{
			nextSpawn = Random.Range(spawnRange.x, spawnRange.y);

			GameObject instance = (GameObject)Instantiate(prefabs[Random.Range(0, this.prefabs.Length)]);
			instance.transform.parent = transform;
			instance.transform.rotation = follower.transform.rotation;
			instance.transform.position = follower.transform.position - follower.transform.up * 1.5f + follower.right * Random.Range(-3.0f, 3.0f);

			RaycastHit groundHit;
			Physics.Raycast(instance.transform.position, -instance.transform.up, out groundHit, float.MaxValue, 1 << LayerMask.NameToLayer("Ground"));
			//instance.transform.position = groundHit.point;

			++obstacles;
			if (obstacles > maxObstacles)
				GameObject.Destroy(this);
		}
	}
}
