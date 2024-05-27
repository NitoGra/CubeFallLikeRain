using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
public abstract class Handler : MonoBehaviour, IContactHandler<Handler>
{
	[SerializeField] protected int MinLiveTime;
	[SerializeField] protected int MaxLiveTime;

	private Color _defaultColor;
	private ObjectPool<Handler> _pool;
	private int _minLifeTime;
	private int _maxLifeTime;
	private Rigidbody _rigidbody;

	protected MeshRenderer MeshHandler;

	private void Awake()
	{
		MeshHandler = GetComponent<MeshRenderer>();
		_rigidbody = GetComponent<Rigidbody>();
	}

	private int MakeRandomLiveTime()
	{
		return UnityEngine.Random.Range(_minLifeTime, _maxLifeTime + 1);
	}

	protected virtual void Destroy()
	{
		print("Бомба взорвана!");
		_pool.Release(this);
	}

	public abstract void SetSpawnSettings(ObjectPool<Handler> pool);

	public void DestroyOnRandomTime()
	{
		Invoke(nameof(Destroy), MakeRandomLiveTime());
	}

	public void BackToDefault()
	{
		_rigidbody.velocity = Vector3.zero;
		MeshHandler.material.color = _defaultColor;
		gameObject.SetActive(true);
	}

	public void SetDefaultFields(ObjectPool<Handler> pool, Color defaultColor, int minLifeTime, int maxLifeTime)
	{
		_pool = pool;
		_defaultColor = defaultColor;
		_minLifeTime = minLifeTime;
		_maxLifeTime = maxLifeTime;
	}
}