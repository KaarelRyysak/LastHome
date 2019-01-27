using UnityEngine;

public class Trap : BaseTrap {
	private bool bloody;
	public Sprite activatedSprite;
	public Sprite disabledSprite;
	public Sprite bloodyActivatedSprite;
	public Sprite bloodyDisabledSprite;
	private SpriteRenderer spriteRenderer;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();

		spriteRenderer.sprite = disabledSprite;
	}

	private void OnMouseDown() {
		//Toggle active on click
		Activated = !Activated;

		//Change sprite
		if (Activated && !bloody) {
			spriteRenderer.sprite = activatedSprite;
		} else if (!Activated && !bloody) {
			spriteRenderer.sprite = disabledSprite;
		} else if (Activated && bloody) {

			spriteRenderer.sprite = bloodyActivatedSprite;
		} else {
			spriteRenderer.sprite = bloodyDisabledSprite;
		}
	}


	private void OnTriggerStay2D(Collider2D collision) {
        Human human = collision.gameObject.GetComponent<Human>();
        //If collided with human
        if (human != null && Activated) {
			collision.gameObject.GetComponent<Human>().Die();
			bloody = true;
			spriteRenderer.sprite = bloodyActivatedSprite;
		}
	}
}
