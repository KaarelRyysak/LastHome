using System.Collections;
using UnityEngine;

public class Roomba : MonoBehaviour {
	public Vector2Int startRoom;
	public float speed, patience;
	Room currentRoom;
	House house;
	SpriteRenderer spriteRenderer;
	public Sprite unselected, selected;
	Camera cam;
	public bool bodyCarried = false;
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
		if (Input.GetButtonDown("Fire1")) {
			if (hasBeenClicked) {
				hasBeenClicked = false;
				spriteRenderer.sprite = unselected;

				//Interrupt previous stuff
				StopAllCoroutines();
				StartCoroutine(Wobble());

				//Move to the place
				StartCoroutine(MoveTo(cam.ScreenToWorldPoint(Input.mousePosition)));
			} else {
				Collider2D[] results = Physics2D.OverlapPointAll(cam.ScreenToWorldPoint(Input.mousePosition));
				foreach (Collider2D result in results) {
					if (result.CompareTag("Roomba")) {
						hasBeenClicked = !hasBeenClicked;
						spriteRenderer.sprite = hasBeenClicked ? selected : unselected;
						break;
					}
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player") && other.GetComponent<Human>().dead && !bodyCarried) {
			bodyCarried = true;
			other.transform.parent = transform;
			other.transform.position = transform.position + new Vector3(0, 0.02f);
		}
	}

	IEnumerator MoveTo(Vector2 targetPos) {
		Vector2 roomCoords = targetPos / 10;
		int xCoord = Mathf.RoundToInt(roomCoords.x);
		int yCoord = Mathf.RoundToInt(roomCoords.y);
		Room targetRoom;
		try {
			targetRoom = house.rooms[yCoord, xCoord];
			Vector3 roomPos = targetRoom.transform.position;
			targetPos = new Vector2(Mathf.Clamp(targetPos.x, roomPos.x - 4, roomPos.x + 4), Mathf.Clamp(targetPos.y, roomPos.y - 4, roomPos.y + 4));
		} catch {
			yield break;
		}
		yield return PathToRoom(targetRoom, house.GetPath(currentRoom, targetRoom, true) != null);
		yield return LerpMove(transform.position, targetPos);
	}

	IEnumerator PathToRoom(Room target, bool respectDoors = false) {
		//Extend path to include standing before and after doors
		Vector3 location = transform.position;
		foreach (Room room in house.GetPath(currentRoom, target, respectDoors)) {
			yield return LerpMove(transform.position, Vector3.Lerp(currentRoom.transform.position, room.transform.position, 0.4f));

			//Only proceed if door is open
			Door door = house.roomsToDoor[new Pair<Room>(currentRoom, room)];
			float waitEnd = Time.time + patience;

			while (!door.open && Time.time < waitEnd) {
				yield return null;
			}
			if (!door.open) { //Out of patience
				yield break;
			}

			yield return LerpMove(transform.position, Vector3.Lerp(currentRoom.transform.position, room.transform.position, 0.6f));
			currentRoom = room;
			if (bodyCarried) {
				GetComponentInChildren<Human>().UpdateRoom(room);
			}
		}
	}

	IEnumerator LerpMove(Vector2 from, Vector2 to) {
		Vector2 direction = to - from;
		float duration = direction.magnitude / speed;
		float startTime = Time.time;
		float endTime = startTime + duration;

		while (Time.time < endTime) {
			yield return null;
			transform.position = Vector2.Lerp(from, to, (Time.time - startTime) / duration);
		}
	}

	IEnumerator Wobble() {
		while (true) {
			yield return new WaitForSeconds(0.5f);
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}
	}
}
