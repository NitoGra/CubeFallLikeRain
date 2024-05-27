using UnityEngine;

public class Expload : MonoBehaviour
{
	[SerializeField] private float _explodingRadiusMultiplyer;
	[SerializeField] private float _explodingForseMultiplyer;

	private float _explodingRadius;
	private float _explodingForse;
	private float _startExplodingRadius = 400;
	private float _startExplodingForse = 400;
	private float _proportionalityFactor = 1.5f;

	private void Start()
	{
		_explodingRadius = _proportionalityFactor / transform.localScale.x * _explodingRadiusMultiplyer + _startExplodingRadius;
		_explodingForse = _proportionalityFactor / transform.localScale.x * _explodingForseMultiplyer + _startExplodingForse;
	}

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
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(transform.position, _explodingRadius);
	}
}