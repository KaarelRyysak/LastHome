using System.Collections;
using UnityEngine;

public class SortLayer : MonoBehaviour {
	SpriteRenderer spriteRenderer;
	public bool updated = false;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sortingOrder = (int) (transform.position.y * -100);
		if (updated) {
			StartCoroutine(UpdateSortingOrder());
		}
	}

	IEnumerator UpdateSortingOrder() {
		while (true) {
			spriteRenderer.sortingOrder = (int) (transform.position.y * -100);
			yield return null;
		}
	}
}
