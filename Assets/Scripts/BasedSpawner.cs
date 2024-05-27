using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class BasedSpawner<T> : MonoBehaviour
	where T : Collider
{
	[SerializeField] private T _zoneSpawn;
	[SerializeField] private Handler _contactHandler;

	[SerializeField] private int _poolCapacity;
	[SerializeField] private int _poolMaxSize;
	[SerializeField] private float _spawnDelay;
	[SerializeField] private TextMeshProUGUI _display;

	[SerializeField] private bool _isSpawnInRandomArea;
	[SerializeField] private bool _isWorkOnStart;

	private int _countSpawnObject = 0;
	private string _textOutput;
	private WaitForSecondsRealtime _wait;
	private ObjectPool<Handler> _pool;
	private Coroutine coroutineWork = null;

	private void Start()
	{
		_pool = new ObjectPool<Handler>(
			createFunc: () => CreateFunc(),
			actionOnGet: (obj) => ActionOnGet(obj),
			actionOnRelease: (obj) => ActionOnRealese(obj),
			actionOnDestroy: (obj) => _pool.Release(obj),
			collectionCheck: true,
			defaultCapacity: _poolCapacity,
			maxSize: _poolMaxSize);

		_textOutput = $"Всего {_contactHandler.name} создано: ";

		if (_isWorkOnStart)
			LaunchSpawn(_isWorkOnStart);

		_display.text = _textOutput + Convert.ToString(_countSpawnObject);
	}

	private Handler CreateFunc()
	{
		Handler handler = Instantiate(_contactHandler); ;
		handler.SetSpawnSettings(_pool);
		return handler;
	}

	private void ActionOnGet(Handler obj)
	{
		Vector3 spawnPosistion = transform.position;

		if (_isSpawnInRandomArea)
			spawnPosistion = MakeRandomPositionInArea();

		obj.transform.position = spawnPosistion;
		obj.BackToDefault();

		_countSpawnObject++;
		_display.text = _textOutput + Convert.ToString(_countSpawnObject);
	}

	private Vector3 MakeRandomPositionInArea()
	{
		Bounds colliderBounds = _zoneSpawn.bounds;
		Vector3 colliderCenter = colliderBounds.center;

		float randomX = UnityEngine.Random.Range(colliderCenter.x - colliderBounds.extents.x, colliderCenter.x + colliderBounds.extents.x);
		float randomY = UnityEngine.Random.Range(colliderCenter.y - colliderBounds.extents.y, colliderCenter.y + colliderBounds.extents.y);
		float randomZ = UnityEngine.Random.Range(colliderCenter.z - colliderBounds.extents.z, colliderCenter.z + colliderBounds.extents.z);

		return new(randomX, randomY, randomZ);
	}

	private IEnumerator SpawnObject()
	{
		_wait = new(_spawnDelay);

		while (enabled)
		{
			if (_pool.CountActive < _poolMaxSize)
				_pool.Get();

			yield return _wait;
		}
	}

	protected void LaunchSpawn(bool isWork)
	{
		if (isWork)
		{
			coroutineWork = StartCoroutine(SpawnObject());
		}
		else
		{
			StopCoroutine(coroutineWork);
			coroutineWork = null;
		}

	}

	protected virtual void ActionOnRealese(Handler obj)
	{
		obj.gameObject.SetActive(false);
	}
}