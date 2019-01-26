using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {
	Room currentRoom;
	House house;

	void Start() {
		house = House.instance;
		currentRoom = house.StartRoom;
	}


}
