using System.Collections;
using UnityEngine;

public class SortLayer : MonoBehaviour {
	SpriteRenderer spriteRenderer;
	public bool updated = false;
	public int offset;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sortingOrder = (int) (transform.position.y * -100) + offset;
		if (updated) {
			StartCoroutine(UpdateSortingOrder());
		}
	}

	IEnumerator UpdateSortingOrder() {
		while (true) {
			spriteRenderer.sortingOrder = (int) (transform.position.y * -100) + offset;
			yield return null;
		}
	}
}
