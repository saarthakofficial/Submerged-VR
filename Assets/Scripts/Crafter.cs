using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crafter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject compass;
    public GameObject amulet;
    [SerializeField] TMP_Text popUpText;
    public InputActionReference leftTriggerInput;
    public InputActionReference rightTriggerInput;
    public GameObject poofVFX;
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Interactor"){
            popUpText.gameObject.SetActive(true);
            if (leftTriggerInput.action.IsPressed() || rightTriggerInput.action.IsPressed()){
                if (ItemsManager.instance.compassCollected && !ItemsManager.instance.amuletCollected){
                    Instantiate(poofVFX, transform.position, Quaternion.identity);
                    GameManager.instance.SelectRandomSpawners(ItemType.Amulet);
                    GameManager.instance.compass = Instantiate(compass, transform.position, Quaternion.identity);
                    gameObject.SetActive(false);
                }
                else if (ItemsManager.instance.amuletCollected){
                    Instantiate(poofVFX, transform.position, Quaternion.identity);
                    Instantiate(amulet, transform.position, Quaternion.identity);
                    gameObject.SetActive(false);
                }
                else{
                    popUpText.text = "Not Enough Materials!";
                }
            }
            else{
                popUpText.text = "Press Trigger to Craft";
            }
            
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Interactor"){
            popUpText.gameObject.SetActive(false);
        }
    }


    void Update() {
        if (popUpText.gameObject.activeInHierarchy){
            popUpText.transform.LookAt(-Camera.main.transform);
        }
    }
}
