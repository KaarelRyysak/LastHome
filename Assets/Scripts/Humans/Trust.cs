using UnityEngine;
using UnityEngine.UI;

public class Trust : MonoBehaviour {
	public static Trust instance;
	Image trustBar;

	float value = 500;
	public float Value {
		get {
			return value;
		} set {
			this.value = value;
			trustBar.fillAmount = value / 1000;
		}
	}

	void Awake() {
		instance = this;
		trustBar = GetComponentsInChildren<Image>()[1];
		Value = Value;
	}
}