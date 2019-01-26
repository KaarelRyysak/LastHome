using UnityEngine;

public class Door : MonoBehaviour {
    public bool open;
	public Sprite openSprite;
	public Sprite closedSprite;
	private SpriteRenderer spriteRenderer;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = openSprite;

        open = true;
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
