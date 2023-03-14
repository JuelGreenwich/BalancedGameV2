using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    //Stores the information about the object that is being duplicated (can be accessed via the editor)
    [SerializeField]
    private GameObject originalObject;

    //Shortcut for placing an item
    [SerializeField]
    private KeyCode placeHotKey = KeyCode.P;

    //Stores the value of a game object being placed
    private GameObject placeObject;


    // Update is called once per frame
    void Update()
    {
        DetectInput();

        if (placeObject != null)
        {
            MoveToMousePosition();
            FinishPlacement();
        }
    }

    private void FinishPlacement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            placeObject = null;
        }
    }

    //Detects the collider the camera is pointing at and moves the game object to that position.
    private void MoveToMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit pointerInfo;
        if (Physics.Raycast(ray, out pointerInfo))
        {
            // Check if the raycast hit the `placeObject`
            if (pointerInfo.collider.gameObject != placeObject)
            {
                placeObject.transform.position = pointerInfo.point;
            }
        }
    }


    //Detects players input and creates another instance of the selected object if the correct input is given
    private void DetectInput()
    {
        if (Input.GetKeyDown(placeHotKey))
        {
            if (placeObject == null)
            {
                placeObject = Instantiate(originalObject);
            }
            else
            {
                Destroy(placeObject);
            }
        }
    }
}