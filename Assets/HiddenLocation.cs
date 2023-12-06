using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenLocation : MonoBehaviour
{
    public string riddle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItem(GameObject item){
        Instantiate(item, transform.position, Quaternion.identity, this.transform);
    }
}
