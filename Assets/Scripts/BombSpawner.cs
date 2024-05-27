using UnityEngine;

public class BombSpawner : BasedSpawner<SphereCollider>
{
	[SerializeField] private CubeSpawner _cubeSpawner;

	private void OnEnable()
	{
		_cubeSpawner.BombSpawn += Bomb;
	}

	private void OnDisable()
	{
		_cubeSpawner.BombSpawn -= Bomb;
	}

	private void Bomb(Vector3 position)
	{
		gameObject.transform.position = position;
		LaunchSpawn(true);
		LaunchSpawn(false);
	}
}