using UnityEngine;

public class Trap : MonoBehaviour {
	public bool activated;
	private bool bloody;
	public Sprite activatedSprite;
	public Sprite disabledSprite;
	public Sprite bloodyActivatedSprite;
	public Sprite bloodyDisabledSprite;
	private SpriteRenderer spriteRenderer;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();

		//Inactive on startup
		activated = false;
		spriteRenderer.sprite = disabledSprite;
	}

	private void OnMouseDown() {
		//Toggle active on click
		activated = !activated;

		//Change sprite
		if (activated && !bloody) {
			spriteRenderer.sprite = activatedSprite;
		} else if (!activated && !bloody) {
			spriteRenderer.sprite = disabledSprite;
		} else if (activated && bloody) {

			spriteRenderer.sprite = bloodyActivatedSprite;
		} else {
			spriteRenderer.sprite = bloodyDisabledSprite;
		}
	}


	private void OnTriggerStay2D(Collider2D collision) {
		//If collided with human
		if (collision.gameObject.GetComponent<Human>() != null && activated) {
			collision.gameObject.GetComponent<Human>().Die();
			bloody = true;
			spriteRenderer.sprite = bloodyActivatedSprite;
		}
	}
}
