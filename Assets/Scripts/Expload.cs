using System.Diagnostics;
using UnityEngine;

public class Expload : MonoBehaviour
{
	[SerializeField] private float _explodingRadius;
	[SerializeField] private float _explodingForse;

	private void ExploadArray(Collider[] hitColliders)
	{
		foreach (Collider hitCollider in hitColliders)
			hitCollider.attachedRigidbody?.AddExplosionForce(_explodingForse, transform.position, _explodingRadius);
	}

	public void ExploadInRadius()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explodingRadius);
		ExploadArray(hitColliders);
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, _explodingRadius);
	}
}