using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrop : MonoBehaviour, IDropHandler
{
    [SerializeField] new Collider collider;
    EconomyManager economyManager;
    Camera cam;
    WorldTracker worldTracker;
    AnimalDisplay animalDisplay;
    void Awake()
    {
        economyManager = FindObjectOfType<EconomyManager>();
        cam = FindObjectOfType<Camera>();
        worldTracker = FindObjectOfType<WorldTracker>();
        animalDisplay = GetComponentInChildren<AnimalDisplay>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;
        Vector3 spawnLocation = (Vector3)eventData.position + new Vector3(0,0, 10);
        Vector3 spawnPos = cam.ScreenToWorldPoint(new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z));
        AnimalDisplay itemToSpawn = eventData.pointerDrag.GetComponent<AnimalDisplay>();

        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            if(economyManager.currency.Credits >= animalDisplay.cost)
            {
                economyManager.Purchase(itemToSpawn.index);
                Instantiate(itemToSpawn.mesh, spawnPos, Quaternion.identity);
                worldTracker.AddToList(itemToSpawn);
            }
            else
            {
                return;
            }
        }
    }
}
