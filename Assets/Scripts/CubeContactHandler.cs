using UnityEngine;
using UnityEngine.Pool;

public class CubeContactHandler : MonoBehaviour
{
	[SerializeField] private LayerMask _floor;
	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private MeshRenderer _mesh;
	[SerializeField] private AudioSource _audio;

	private Quaternion _basedQuaternion = new Quaternion(0, 0, 0, 0);
	private Color _basedColor = Color.white;
	private ObjectPool<CubeContactHandler> _pool;
	private bool _isCanTouch = true;
	private int _minLifeTime = 2;
	private int _maxLifeTime = 5;

	private void OnCollisionEnter(Collision collision)
	{
		_audio.Play();

		if (_isCanTouch == false)
			return;

		if (collision.gameObject.layer == _floor)
			return;

		_isCanTouch = false;
		_mesh.material.color = UnityEngine.Random.ColorHSV();
		Invoke(nameof(Destroy), Random.Range(_minLifeTime, _maxLifeTime + 1));
	}

	private void Destroy()
	{
		_pool.Release(this);
	}

	public void SetSpawnSettings(ObjectPool<CubeContactHandler> pool)
	{
		_mesh.material.color = _basedColor;
		_isCanTouch = true;
		_pool = pool;
		transform.rotation = _basedQuaternion;
		_rigidbody.velocity = Vector3.zero;
	}
}