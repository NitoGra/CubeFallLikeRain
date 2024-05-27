using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Expload))]
public class BombContactHandler : Handler
{
	private WaitForSecondsRealtime wait;

	private void OnEnable()
	{
		StartCoroutine(TransparencyChange());
	}

	private IEnumerator TransparencyChange()
	{
		int liveTime = DestroyOnRandomTime();
		Color color = MeshHandler.material.color;
		float delayWait = Time.deltaTime;
		wait = new(delayWait);

		float alpha = color.a;
		float step = delayWait / liveTime;

		while (alpha < liveTime)
		{
			alpha -= step;
			color = new Color(color.r, color.g, color.b, alpha);
			MeshHandler.material.color = color;
			yield return wait;
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