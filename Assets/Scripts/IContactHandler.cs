using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IContactHandler<K> where K : MonoBehaviour
{
	public void SetSpawnSettings(ObjectPool<K> Pool);
}
