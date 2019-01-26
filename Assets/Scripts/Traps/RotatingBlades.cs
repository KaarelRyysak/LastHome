using UnityEngine;

public class RotatingBlades : MonoBehaviour {
	public float rotationAmount = 150f;
	private Trap trap;
	
	void Start() {
		trap = gameObject.GetComponent<Trap>();
	}

	void Update() {
		if (trap.activated) {
			transform.Rotate(new Vector3(0, 0, -rotationAmount * 10 * Time.deltaTime));
		} else {
			transform.Rotate(new Vector3(0, 0, -rotationAmount * Time.deltaTime));
		}
	}
}
