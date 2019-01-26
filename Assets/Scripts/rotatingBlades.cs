using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatingBlades : MonoBehaviour
{
    public float rotationAmount = 150f;
    private Trap trap;
    // Start is called before the first frame update
    void Start()
    {
        trap = gameObject.GetComponent<Trap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trap.activated)
        {
            transform.Rotate(new Vector3(0, 0, -rotationAmount * 10 * Time.deltaTime));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -rotationAmount * Time.deltaTime));
        }
    }
}
