using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalDisplay : MonoBehaviour
{
    public AnimalTemplate animalTemplate;

    public TextMeshProUGUI nameText;
    public Image icon;
    public GameObject mesh;
    public int cost;
    public int index;

    void Start()
    {
        nameText.text = animalTemplate.name;
        icon.sprite = animalTemplate.icon;
        mesh = animalTemplate.prefab;
        cost = animalTemplate.cost;
        index = animalTemplate.index;
    }
}
