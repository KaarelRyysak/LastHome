using System.Collections;
using UnityEngine;

public class SortParent : MonoBehaviour {
	SpriteRenderer spriteRenderer;
	SpriteRenderer parentRenderer;
	public bool updated = false;
	public int offset;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
		spriteRenderer.sortingOrder = parentRenderer.sortingOrder + offset;
		if (updated) {
			StartCoroutine(UpdateSortingOrder());
		}
	}

	IEnumerator UpdateSortingOrder() {
		while (true) {
			spriteRenderer.sortingOrder = parentRenderer.sortingOrder + offset;
			yield return null;
		}
	}
}