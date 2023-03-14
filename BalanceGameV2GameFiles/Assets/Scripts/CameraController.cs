using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 5f;

    [SerializeField]
    private float cameraSensitivityHorizontal = 5f;
    [SerializeField]
    private float cameraSensitivityVertical = 5f;

    private float horizontal = 0f;
    private float vertical = 0f;

    private bool stopRotation = false;

    [SerializeField]
    private float maximalRotation = 85f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + (-transform.right * cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + (transform.right * cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + (transform.forward * cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + (-transform.forward * cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position = transform.position + (-transform.up * cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = transform.position + (transform.up * cameraSpeed * Time.deltaTime);
        }

        horizontal += cameraSensitivityHorizontal * Input.GetAxis("Mouse X");
        vertical -= cameraSensitivityVertical * Input.GetAxis("Mouse Y");

        CheckStopRotation();

        if (stopRotation == false)
        {
            if (vertical > maximalRotation)
            {
                vertical = maximalRotation;
            }
            else if (vertical < -maximalRotation)
            {
                vertical = -maximalRotation;
            }

            transform.eulerAngles = new Vector3(vertical, horizontal, 0f);
        }

    }

    private void CheckStopRotation()
    {
        if (Input.GetMouseButtonDown(1))
        {
            stopRotation = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            stopRotation = false;
        }
    }
}
