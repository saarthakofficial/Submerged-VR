using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectItem : MonoBehaviour
{
    public int piece;
    public ItemType itemType;
    [SerializeField] InputActionReference BButtonReference;
    [SerializeField] bool underwater = true;
    [SerializeField] ParticleSystem collectVFX;

    // Start is called before the first frame update
    void Start(){
        GameManager.instance.player.GetComponent<Swimmer>().grabbing = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate(){
        if (BButtonReference.action.IsPressed()){
            Debug.Log(itemType.ToString() + piece);
            Debug.Log(ItemType.Compass.ToString() + piece);
            if (itemType == ItemType.Map){
                if (ItemsManager.instance.itemsCollected.Contains("Map1")){
                    GameManager.instance.map = GameManager.instance.map2;
                }
                else{
                    GameManager.instance.map = GameManager.instance.map1;
                    GameManager.instance.SelectRandomSpawners(ItemType.Compass);
                }
            }

            
            if (itemType == ItemType.Compass){
                UIManager.instance.mapStrikes[piece-1].SetActive(true);
            }
            if (itemType == ItemType.Amulet && piece < 3){
                GameManager.instance.compassTarget = GameManager.instance.spawnedAmuletPieces[piece].transform;
            }
            
            Instantiate(collectVFX, transform.position, Quaternion.identity);
            GameManager.instance.player.GetComponent<Swimmer>().DelayedFalsifyGrabbing();
            ItemsManager.instance.itemsCollected.Add(itemType.ToString() + piece);
            Invoke("FalsifyGrabbing",1.5f);
            if (underwater){
                for (int i = 0; i<3; i++){
                    if (GameManager.instance.chosenItemSpawners[i].GetComponent<ItemSpawner>().spawnedObject == this.gameObject){
                        GameManager.instance.chosenItemSpawners[i].GetComponentInChildren<Light>().enabled = false;
                        break;
                    }
                }
            }
            Destroy(this.gameObject);  
        }
        
    }

 
}