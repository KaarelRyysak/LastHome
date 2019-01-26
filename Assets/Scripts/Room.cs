using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
	public int y, x;

	public List<Room> neighbors = new List<Room>();
}
