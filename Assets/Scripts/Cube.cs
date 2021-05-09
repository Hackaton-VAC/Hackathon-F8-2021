using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    public Vector3 blurredPosition;
    public Vector3 blurredScale;
    public Vector3 currentPosition;
    public Vector3 currentScale;

    public float speed = 0f;
    public float rotationSpeed = 0f;
    public float resizeSpeed = 65f;

    public bool resizing = false;

    // Update is called once per frame
    public void Update()
    {
        if (currentPosition != transform.position)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, currentPosition, step);
            if (Vector3.Distance(transform.position, currentPosition) < 0.001f)
            {
               currentPosition = transform.position;
            }
        }
        if (currentScale != transform.localScale)
        {
            resizing = true;
            float step = resizeSpeed * Time.deltaTime;
            transform.localScale = Vector3.MoveTowards(transform.localScale, currentScale, step);
            if (Vector3.Distance(transform.position, currentScale) < 0.1f)
            {
                currentScale = transform.localScale;
            }
        }
        else
        {
            resizing = false;
        }
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
