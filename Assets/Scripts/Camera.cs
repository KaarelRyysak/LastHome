using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float upperBound = 50;
    public float lowerBound = -50;
    public float leftBound = -50;
    public float rightBound = 50;

// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Get input
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 currentPos = gameObject.transform.position;
        
        //Stop movement based on bounds
        if((currentPos.x < leftBound && x < 0) || (currentPos.x > rightBound && x > 0))
        {
            x = 0f;
        }
        if((currentPos.y < lowerBound && y < 0) || (currentPos.y > upperBound && y > 0))
        {
            y = 0f;
        }

        //Move
        gameObject.transform.Translate(new Vector3(x, y, 0f));
    }
}
