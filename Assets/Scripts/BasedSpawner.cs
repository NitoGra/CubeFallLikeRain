using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class BasedSpawner<T, K> : MonoBehaviour
	where T : Collider
	where K : MonoBehaviour, IContactHandler<K>
{ 
	[SerializeField] private T _zoneSpawn;
	[SerializeField] private K _contactHandler;

	[SerializeField] private int _poolCapacity;
	[SerializeField] private int _poolMaxSize;
	[SerializeField] private float _spawnDelay;
	[SerializeField] private TextMeshProUGUI _display;

	[SerializeField] private bool _isSpawnInRandomArea;

	private string _textOutput;
	private WaitForSecondsRealtime _wait;
	private ObjectPool<K> _pool;

	private void Start()
	{
		_pool = new ObjectPool<K>(
			createFunc: () => Instantiate(_contactHandler),
			actionOnGet: (obj) => ActionOnGet(obj),
			actionOnRelease: (obj) => obj.gameObject.SetActive(false),
			actionOnDestroy: (obj) => _pool.Release(obj),
			collectionCheck: true,
			defaultCapacity: _poolCapacity,
			maxSize: _poolMaxSize);

		_textOutput = $"Всего {_contactHandler.name} создано: ";
		StartCoroutine(SpawnObject());
	}

	private void ActionOnGet(K obj)
	{
		if(_isSpawnInRandomArea)
		{

		}
		Bounds colliderBounds = _zoneSpawn.bounds;
		Vector3 colliderCenter = colliderBounds.center;

		float randomX = UnityEngine.Random.Range(colliderCenter.x - colliderBounds.extents.x, colliderCenter.x + colliderBounds.extents.x);
		float randomY = UnityEngine.Random.Range(colliderCenter.y - colliderBounds.extents.y, colliderCenter.y + colliderBounds.extents.y);
		float randomZ = UnityEngine.Random.Range(colliderCenter.z - colliderBounds.extents.z, colliderCenter.z + colliderBounds.extents.z);

		Vector3 randomPosistion = new(randomX, randomY, randomZ);
		obj.transform.position = randomPosistion;


		obj.gameObject.SetActive(true);
		obj.SetSpawnSettings(_pool);
		_display.text = _textOutput + Convert.ToString(_pool.CountAll);
	}

	protected IEnumerator SpawnObject()
	{
		_wait = new(_spawnDelay);

		while (enabled)
		{
			if (_pool.CountActive < _poolMaxSize)
				_pool.Get();

			yield return _wait;
		}
	}
}



/*
public class CubeSpawner : MonoBehaviour
{
	
	[SerializeField] private int _poolCapacity;
	[SerializeField] private int _poolMaxSize;
	[SerializeField] private float _spawnDelay;
	private WaitForSecondsRealtime _wait;
	private ObjectPool<CubeContactHandler> _pool;


	private void Start()
	{
		_pool = new ObjectPool<CubeContactHandler>(
			createFunc: () => Instantiate(_contactHandler),
			actionOnGet: (obj) => ActionOnGet(obj),
			actionOnRelease: (obj) => obj.gameObject.SetActive(false),
			actionOnDestroy: (obj) => _pool.Release(obj),
			collectionCheck: true,
			defaultCapacity: _poolCapacity,
			maxSize: _poolMaxSize);

		StartCoroutine(SpawnObject());
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

	private void ActionOnGet(CubeContactHandler obj)
	{
		Bounds colliderBounds = _zoneSpawn.bounds;
		Vector3 colliderCenter = colliderBounds.center;

		float randomX = UnityEngine.Random.Range(colliderCenter.x - colliderBounds.extents.x, colliderCenter.x + colliderBounds.extents.x);
		float randomY = UnityEngine.Random.Range(colliderCenter.y - colliderBounds.extents.y, colliderCenter.y + colliderBounds.extents.y);
		float randomZ = UnityEngine.Random.Range(colliderCenter.z - colliderBounds.extents.z, colliderCenter.z + colliderBounds.extents.z);

		Vector3 randomPosistion = new(randomX, randomY, randomZ);

		obj.gameObject.SetActive(true);
		obj.SetSpawnSettings(_pool);
		obj.GetComponent<UISpawnNumbers>().SetText(_display);
		obj.transform.position = randomPosistion;
		_display.text = _textOutput + Convert.ToString(_pool.CountAll);
	}
}
*/