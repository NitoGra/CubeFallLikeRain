using System;
using UnityEngine;

public class CubeSpawner : BasedSpawner<BoxCollider>
{
	public event Action<Vector3> BombSpawn;

	protected override void ActionOnRealese(Handler obj)
	{
		BombSpawn?.Invoke(obj.transform.position);
		obj.gameObject.SetActive(false);
	}
}