using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager instance;
    public List<string> itemsCollected = new List<string>();
    public bool compassCollected = false;
    public GameObject craftTrigger;
    public bool amuletCollected = false;
    void Awake(){
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (itemsCollected.Contains(ItemType.Compass.ToString() + 1) && itemsCollected.Contains(ItemType.Compass.ToString() + 2) && itemsCollected.Contains(ItemType.Compass.ToString() + 3)){
            if (compassCollected == false){
                craftTrigger.SetActive(true);
                compassCollected = true;
            }
        }
        if (itemsCollected.Contains(ItemType.Amulet.ToString() + 1) && itemsCollected.Contains(ItemType.Amulet.ToString() + 2) && itemsCollected.Contains(ItemType.Amulet.ToString() + 3)){
            if (amuletCollected == false){
                craftTrigger.SetActive(true);
                amuletCollected = true;
            }
        }
    }
}
