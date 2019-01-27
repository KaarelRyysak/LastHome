using UnityEngine;

public class BrokenHeart : MonoBehaviour {
	private float lifeTime = 1f;
	float speed = 0.25f;
	void Update() {
		speed *= 1 + Time.deltaTime;
		transform.Translate(Vector3.up * Time.deltaTime * speed);

		lifeTime -= Time.deltaTime;
		if (lifeTime <= 0)
			Destroy(gameObject);
	}
}
