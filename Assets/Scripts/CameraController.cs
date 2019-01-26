using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float upperBound = 10f;
    public float lowerBound = -10f;
    public float leftBound = -15f;
    public float rightBound = 15f;

    public float maxZoom = 5f;
    //public float rotSpeed = 10f;
    public float scrollSpeed = 3f;
    private float zoom = 0f;

    private float initialSize;

    private Camera camera;

// Start is called before the first frame update
    void Start()
    {
        camera = this.GetComponent<Camera>();
        initialSize = camera.orthographicSize;
        zoom = initialSize;
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

        //Move camera
        gameObject.transform.Translate(new Vector3(x, y, 0f));

        //Get scroll
        zoom -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        //Stop based on bounds
        zoom = Mathf.Clamp(zoom, initialSize-maxZoom, initialSize+maxZoom);
        
        //Apply zoom
        camera.orthographicSize = zoom;
    }
}
