using UnityEngine;

public class BombSpawner : BasedSpawner<SphereCollider>
{
	[SerializeField] private CubeSpawner _cubeSpawner;

	private bool _canSpawn = true;

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
		print("Бомба начинает спавниться!");
		LaunchSpawn(_canSpawn);
		_canSpawn = !_canSpawn;
		print("Бомба заканчивает спавниться!");
		LaunchSpawn(_canSpawn);
		_canSpawn = !_canSpawn;
	}
}