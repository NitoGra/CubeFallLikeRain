using System.Collections;
using TMPro;
using UnityEngine;

public class UISpawnNumbers : MonoBehaviour
{
	readonly private Color _defaultColor = new(100, 33, 100, 255);
	readonly private Color _deleteColor = new(0, 0, 0, 0);
	
	private float _delay = 1f;
	private TextMeshProUGUI _text;

	public void SetText(TextMeshProUGUI text) => _text = text;

	public void SeeNumber(string number, Vector3 position)
	{
		_text.color = _defaultColor;
		_text.text = number;
		StartCoroutine(FlyNumbers(position));
	}

	private IEnumerator FlyNumbers(Vector3 position)
	{
		float timer = 0;
		_text.gameObject.transform.position = position;
		bool isWorking = true;

		while (isWorking)
		{
			_text.rectTransform.localPosition += position.normalized * 10;

			if (timer < _delay)
			{
				timer += Time.deltaTime;
				yield return Time.deltaTime;
				continue;
			}

			isWorking = false;
			Delete();
			StopAllCoroutines();
		}

	}

	private void Delete()
	{
		_text.color = _deleteColor;
	}
}