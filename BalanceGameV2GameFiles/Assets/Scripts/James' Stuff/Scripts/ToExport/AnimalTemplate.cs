using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animal", menuName = "Animal")]
public class AnimalTemplate : ScriptableObject
{
    public new string name;
    public float health;

    public int attackPower;

    public GameObject prefab;
    public Sprite icon;

    public int cost;
    public int index;

    public float lifeSpan;
    public float hungerRate;
    public float maxHunger;
    public float hungerThreshold;

    public float roamRadius;
    public float matingRadius;
    public float matingCoolDown;
    
    public int maxChildren;
}
