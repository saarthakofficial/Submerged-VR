using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public string riddle;
    public GameObject spawnedObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItem(GameObject item, ItemType type){
        spawnedObject = Instantiate(item, transform.position, Quaternion.identity, this.transform);
        if (type == ItemType.Compass){
            GameManager.instance.spawnedCompassPieces.Add(spawnedObject);
        }
        else if (type == ItemType.Amulet){
            GameManager.instance.spawnedAmuletPieces.Add(spawnedObject);
        }
    }
}
