using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Expload))]
public class BombContactHandler : Handler
{
	private WaitForSecondsRealtime wait = new(0.1f);

	private void OnEnable()
	{
		StartCoroutine(TransparencyChange());
	}

	private IEnumerator TransparencyChange()
	{
		Color color = MeshHandler.material.color;
		while (MeshHandler.material.color.a > 0)
		{
			print(MeshHandler.material.color.a);
			float alpha = color.a - 0.1f;
			MeshHandler.material.color = new Color(Color.black.r, Color.black.g, Color.black.b, alpha);
			yield return wait;
		}

		DestroyOnRandomTime();
	}

	protected override void Destroy()
	{
		print("Бомба взорвана!");

		GetComponent<Expload>().ExploadInRadius();
		base.Destroy();
	}

	public override void SetSpawnSettings(ObjectPool<Handler> pool)
	{
		SetDefaultFields(pool, Color.black, MinLiveTime, MaxLiveTime);
	}
}