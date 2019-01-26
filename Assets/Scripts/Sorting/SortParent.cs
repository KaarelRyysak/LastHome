using System.Collections;
using UnityEngine;

public class SortParent : MonoBehaviour {
	SpriteRenderer spriteRenderer;
	SpriteRenderer parentRenderer;
	public bool updated = false;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		parentRenderer = GetComponentInParent<SpriteRenderer>();
		spriteRenderer.sortingOrder = parentRenderer.sortingOrder;
		if (updated) {
			StartCoroutine(UpdateSortingOrder());
		}
	}

	IEnumerator UpdateSortingOrder() {
		while (true) {
			spriteRenderer.sortingOrder = parentRenderer.sortingOrder;
			yield return null;
		}
	}
}