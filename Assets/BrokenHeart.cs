using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenHeart : MonoBehaviour
{
    private float lifeTime = 1000f;
    float speed = 0.25f;
    void Update()
    {
        speed *= 1 + Time.deltaTime;
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            Destroy(gameObject);
    }
}
