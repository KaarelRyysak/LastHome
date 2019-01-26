using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Panning")]
    public float upperBound = 10f;
    public float lowerBound = -10f;
    public float leftBound = -15f;
    public float rightBound = 15f;
    public float panSpeed = 1f;

    


    [Header("This is a percentage of the whole screen")]
    public float panEdgeSize = 0.15f;

    [Header("Zooming")]
    public float maxZoom = 5f;
    //public float rotSpeed = 10f;
    public float scrollSpeed = 3f;
    private float zoom = 0f;

    private float initialSize;

    private Camera mainCamera;


// Start is called before the first frame update
    void Start()
    {
        mainCamera = this.GetComponent<Camera>();
        initialSize = mainCamera.orthographicSize;
        zoom = initialSize;
    }

    // Update is called once per frame
    void Update()
    {
        //Get movement change input
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //Screen panning using mouse
        if (Input.mousePosition.x >= Screen.width - Screen.width * panEdgeSize)
        {
            x += Time.deltaTime * panSpeed;
        }

        if (Input.mousePosition.x <= 0 + Screen.width * panEdgeSize)
        {
            x -= Time.deltaTime * panSpeed;
        }

        if (Input.mousePosition.y >= Screen.height - Screen.height * panEdgeSize)
        {
            y += Time.deltaTime * panSpeed;
        }

        if (Input.mousePosition.y <= 0 + Screen.height * panEdgeSize)
        {
            y -= Time.deltaTime * panSpeed;
        }


        //Stop movement based on bounds
        Vector3 currentPos = gameObject.transform.position;
        
        if((currentPos.x < leftBound && x < 0) || (currentPos.x > rightBound && x > 0))
        {
            x = 0f;
        }
        if((currentPos.y < lowerBound && y < 0) || (currentPos.y > upperBound && y > 0))
        {
            y = 0f;
        }

        //Move camera
        gameObject.transform.Translate(new Vector3(x, y, 0f) * panSpeed * Time.deltaTime);



        //Get scroll
        zoom -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        //Stop based on bounds
        zoom = Mathf.Clamp(zoom, initialSize-maxZoom, initialSize+maxZoom);
        
        //Apply zoom
        mainCamera.orthographicSize = zoom;
        
        
    }
}
