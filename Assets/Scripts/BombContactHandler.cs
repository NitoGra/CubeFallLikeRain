using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Expload))]
public class BombContactHandler : Handler
{
	private float _delayWait;
	private WaitForSecondsRealtime _wait;

	private void OnEnable()
	{
		StartCoroutine(TransparencyChange());
	}

	private IEnumerator TransparencyChange()
	{
		_delayWait = Time.deltaTime;
		_wait = new(_delayWait);

		int liveTime = DestroyOnRandomTime();
		Color color = MeshHandler.material.color;
		float alpha = color.a;
		float step = _delayWait / liveTime;

		while (alpha < liveTime)
		{
			alpha -= step;
			color = new Color(color.r, color.g, color.b, alpha);
			MeshHandler.material.color = color;
			yield return _wait;
		}
	}

	protected override void Destroy()
	{
		GetComponent<Expload>().ExploadInRadius();
		base.Destroy();
	}

	public override void SetSpawnSettings(ObjectPool<Handler> pool)
	{
		SetDefaultFields(pool, Color.black, MinLiveTime, MaxLiveTime);
	}
}