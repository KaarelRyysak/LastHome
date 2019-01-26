using UnityEngine;

public class Door : MonoBehaviour {
	public bool open = false; //Closed on startup
	public Sprite openSprite;
	public Sprite closedSprite;
	private SpriteRenderer spriteRenderer;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = closedSprite;
	}

	private void OnMouseDown() {
		//Toggle active on click
		open = !open;

		//Change sprite
		if (open) {
			spriteRenderer.sprite = openSprite;
		} else {
			spriteRenderer.sprite = closedSprite;
		}
	}
}
