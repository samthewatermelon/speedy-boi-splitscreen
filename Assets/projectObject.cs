using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectObject : MonoBehaviour
{
    public Vector3 moveDirection;    
    public float moveSpeed;

    public bool speedIncrease;
    public float speedIncreaseRate;

    public bool rotateObject;
    public Vector3 rotation;
    public float rotationSpeed;

    private void Start()
    {
        if (moveSpeed == 0)
            moveSpeed = 0.1f; /// give a tiny bit of movement in case we forget to set speed
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += moveDirection * moveSpeed;

        //var rot = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, transform.eulerAngles.x + 1, 0.01f), 0f, 0f);

        if (rotateObject)
            //transform.eulerAngles += rot;// * rotation * rotationSpeed;
        transform.eulerAngles += rotation * rotationSpeed;

        if (speedIncrease)
            moveSpeed += moveSpeed * speedIncreaseRate;
    }
}
