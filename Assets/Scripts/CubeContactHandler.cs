using System;
using UnityEngine;
using UnityEngine.Pool;

public class CubeContactHandler : MonoBehaviour
{
	[SerializeField] private AudioSource _audio;
	[SerializeField] private UISpawnNumbers _text;

	readonly private Quaternion _defaultQuaternion = new(0, 0, 0, 0);
	readonly private Color _defaultColor = Color.white;
	readonly private float hueMin = 0f;
	readonly private float hueMax = 0.5f;
	readonly private float saturationMin= 0.7f;
	readonly private float saturationMax = 1f;
	
	private Rigidbody _rigidbody;
	private MeshRenderer _mesh;
	private ObjectPool<CubeContactHandler> _pool;

	private bool _isCanTouch = true;
	private int _minLifeTime = 2;
	private int _maxLifeTime = 5;

	private void Awake()
	{
		_mesh = GetComponent<MeshRenderer>();
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		_audio.Play();

		if (_isCanTouch == false)
			return;

		if (collision.rigidbody != null)
			return;

		_isCanTouch = false;
		_mesh.material.color = UnityEngine.Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax);

		int lifeTime = UnityEngine.Random.Range(_minLifeTime, _maxLifeTime + 1);
		_text.SeeNumber(Convert.ToString(lifeTime), transform.position);
		Invoke(nameof(Destroy), lifeTime);
	}

	private void Destroy()
	{
		_pool.Release(this);
	}

	public void SetSpawnSettings(ObjectPool<CubeContactHandler> pool)
	{
		_pool = pool;
		_isCanTouch = true;
		_rigidbody.velocity = Vector3.zero;
		_mesh.material.color = _defaultColor;
		transform.rotation = _defaultQuaternion;
	}
}