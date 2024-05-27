using UnityEngine;
using UnityEngine.Pool;

public class CubeContactHandler : Handler
{
	[SerializeField] private AudioSource _audio;

	readonly private Quaternion _defaultQuaternion = new(0, 0, 0, 0);
	readonly private float _hueMin = 0f;
	readonly private float _hueMax = 0.5f;
	readonly private float _saturationMin = 0.7f;
	readonly private float _saturationMax = 1f;

	private bool _isCanTouch = true;

	private void OnCollisionEnter(Collision collision)
	{
		_audio.Play();

		if (_isCanTouch == false)
			return;

		if (collision.rigidbody != null)
			return;

		_isCanTouch = false;
		MeshHandler.material.color = UnityEngine.Random.ColorHSV(_hueMin, _hueMax, _saturationMin, _saturationMax);

		DestroyOnRandomTime();
	}

	protected override void Destroy()
	{
		base.Destroy();
		_isCanTouch = true;
	}

	public override void SetSpawnSettings(ObjectPool<Handler> pool)
	{
		_isCanTouch = true;
		transform.rotation = _defaultQuaternion;
		SetDefaultFields(pool, Color.white, MinLiveTime, MaxLiveTime);
	}
}