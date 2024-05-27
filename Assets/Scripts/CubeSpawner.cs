using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
	[SerializeField] private CubeContactHandler _cubeContactHandler;
	[SerializeField] private BoxCollider _zoneSpawn;
	[SerializeField] private int _poolCapacity;
	[SerializeField] private int _poolMaxSize;
	[SerializeField] private float _spawnDelay;
	[SerializeField] private TextMeshProUGUI _text;

	private string _textOutput = "Всего кубов создано: ";
	private WaitForSecondsRealtime _wait;
	private ObjectPool<CubeContactHandler> _pool;

	private void Start()
	{
		_pool = new ObjectPool<CubeContactHandler>(
			createFunc: () => Instantiate(_cubeContactHandler),
			actionOnGet: (obj) => ActionOnGet(obj),
			actionOnRelease: (obj) => obj.gameObject.SetActive(false),
			actionOnDestroy: (obj) => _pool.Release(obj),
			collectionCheck: true,
			defaultCapacity: _poolCapacity,
			maxSize: _poolMaxSize);

		StartCoroutine(GetCube());
	}

	private IEnumerator GetCube()
	{
		_wait = new(_spawnDelay);

		while (enabled)
		{
			if (_pool.CountActive < _poolMaxSize)
				_pool.Get();

			yield return _wait;
		}
	}

	private void ActionOnGet(CubeContactHandler obj)
	{
		Bounds colliderBounds = _zoneSpawn.bounds;
		Vector3 colliderCenter = colliderBounds.center;

		float randomX = UnityEngine.Random.Range(colliderCenter.x - colliderBounds.extents.x, colliderCenter.x + colliderBounds.extents.x);
		float randomY = UnityEngine.Random.Range(colliderCenter.y - colliderBounds.extents.y, colliderCenter.y + colliderBounds.extents.y);
		float randomZ = UnityEngine.Random.Range(colliderCenter.z - colliderBounds.extents.z, colliderCenter.z + colliderBounds.extents.z);

		Vector3 randomPos = new(randomX, randomY, randomZ);

		obj.gameObject.SetActive(true);
		obj.SetSpawnSettings(_pool);
		obj.GetComponent<UISpawnNumbers>().SetText(_text);
		obj.transform.position = randomPos;
		_text.text =_textOutput + Convert.ToString(_pool.CountAll);
	}
}