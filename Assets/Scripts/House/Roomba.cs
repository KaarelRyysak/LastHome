using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomba : MonoBehaviour {
	public Vector2Int startRoom;
	Room currentRoom;
	House house;
	SpriteRenderer spriteRenderer;
	Camera cam;
	bool bodyCarried = false;
	bool hasBeenClicked = false;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start() {
		cam = Camera.main;
		house = House.instance;
		currentRoom = house.rooms[startRoom.y, startRoom.x];
		StartCoroutine(Wobble());
	}

	void Update() {
		if (Input.GetButtonDown("Fire1") && hasBeenClicked) {
			Debug.Log(cam.ScreenToWorldPoint(Input.mousePosition));
		}
	}

	void OnMouseDown() {
		hasBeenClicked = !hasBeenClicked;
	}

	IEnumerator Wobble() {
		while (true) {
			yield return new WaitForSeconds(1);
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}
	}
}
