using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectItem : MonoBehaviour
{
    public GameObject item;
    public string nameOfTheItem;
    [SerializeField] InputActionReference BButtonReference;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate(){
        if (BButtonReference.action.IsPressed() && !ItemsManager.instance.itemsCollected.Contains(nameOfTheItem)){
            ItemsManager.instance.itemsCollected.Add(nameOfTheItem);
            Destroy(item);
        }
    }
}
