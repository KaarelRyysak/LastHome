using UnityEngine;

public class PitTrap : BaseTrap {
	private GameObject border;
	private bool bloody;
	public Sprite activatedSprite;
	public Sprite disabledSprite;
	public Sprite bloodyActivatedSprite;
	public Sprite bloodyDisabledSprite;
	private SpriteRenderer spriteRenderer;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		border = gameObject.transform.GetChild(0).gameObject;

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
		if (human != null && Activated && !human.falling) {

			SortLayer sortLayer = human.gameObject.GetComponent<SortLayer>();
			sortLayer.offset = -6000;
			human.gameObject.transform.GetChild(0).GetComponent<SortParent>().offset = -6001;

			Vector3 humanpos = human.transform.position;
			Vector3 newPos = Vector3.Lerp(humanpos, gameObject.transform.position, 0.1f);
			human.StopAllCoroutines();

			Vector3 thisPos = gameObject.transform.position;

			if (Vector3.Magnitude(humanpos - newPos) < 0.1f) {
				Drop(human);

			} else {
				human.transform.position = newPos;
			}
		}
	}

	private void Drop(Human human) {

        human.StartCoroutine(human.Fall(border));

        human.Die();

        human.transform.Rotate(new Vector3(0, 0, -90));

        human.transform.parent = gameObject.transform;
		human.StartCoroutine(human.Fall(border));
		human.transform.parent = border.transform;
	}
}
