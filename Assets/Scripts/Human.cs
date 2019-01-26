using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {
	Room currentRoom;
	House house;

    public GameObject corpse;

	void Start() {
		house = House.instance;
		currentRoom = house.StartRoom;
	}



    
    public void Die()
    {
        GameObject.Instantiate(corpse, this.gameObject.transform.position, this.gameObject.transform.rotation);

        Destroy(this.gameObject);
    }
}
