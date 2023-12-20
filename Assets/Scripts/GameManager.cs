using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public GameObject map1;
    public GameObject map2;
    public GameObject map;
    public GameObject collectibleMap;

    public GameObject[] rayInteractors;

    public List<Transform> totalItemSpawners;
    public List<Transform> chosenItemSpawners;
    public GameObject[] compassPieces;
    public GameObject[] amuletPieces;
    public List<GameObject> spawnedCompassPieces;
    public List<GameObject> spawnedAmuletPieces;
    public GameObject compass;
    public Transform compassTarget;
    public int timer = 0;
    private bool isTimerRunning = false;
    void Awake(){
        instance = this;
    }
    
    void Start()
    {
        timer = 0;
        UIManager.instance.SetTimerText();
    }

    public void StartTimer(){
        StartCoroutine(StartTimerCoroutine());
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        for (int i=0; i<UIManager.instance.times.Length; i++){
            if (timer<UIManager.instance.times[i]){
                for (int j=UIManager.instance.times.Length-1; j>i; j--){
                    UIManager.instance.times[j] = UIManager.instance.times[j-1];
                }
                UIManager.instance.times[i] = timer;
                break;
            }
        }
        UIManager.instance.SetTimerText();
    }

    IEnumerator StartTimerCoroutine()
    {
        isTimerRunning = true;

        while (isTimerRunning)
        {
            yield return new WaitForSeconds(1f);
            timer++;
        }
    }

    public void SelectRandomSpawners(ItemType itemType){
        int totalSpawners = 3;
        while (totalSpawners > 0){
            int randomSpawner = Random.Range(0, totalItemSpawners.Count);
            chosenItemSpawners.Add(totalItemSpawners[randomSpawner]);
            if (itemType == ItemType.Compass){
                chosenItemSpawners[3-totalSpawners].GetComponent<ItemSpawner>().SpawnItem(compassPieces[3-totalSpawners], itemType);
            }
            else if (itemType == ItemType.Amulet){
                map = null;
                chosenItemSpawners[3-totalSpawners].GetComponent<ItemSpawner>().SpawnItem(amuletPieces[3-totalSpawners], itemType);
            }
            chosenItemSpawners[3-totalSpawners].GetComponentInChildren<Light>().enabled = true;
            totalItemSpawners.RemoveAt(randomSpawner);
            totalSpawners--;
        }
        if (itemType == ItemType.Compass){
            map2.gameObject.transform.Find("RiddleText").GetComponent<TMP_Text>().text = $"1. {chosenItemSpawners[0].GetComponent<ItemSpawner>().riddle}<br><br>2. {chosenItemSpawners[1].GetComponent<ItemSpawner>().riddle}<br><br>3. {chosenItemSpawners[2].GetComponent<ItemSpawner>().riddle}";
            collectibleMap.gameObject.transform.Find("RiddleText").GetComponent<TMP_Text>().text = $"1. {chosenItemSpawners[0].GetComponent<ItemSpawner>().riddle}<br><br>2. {chosenItemSpawners[1].GetComponent<ItemSpawner>().riddle}<br><br>3. {chosenItemSpawners[2].GetComponent<ItemSpawner>().riddle}";
        }
        else if (itemType == ItemType.Amulet){
            compassTarget = spawnedAmuletPieces[0].transform;
        }
    }

    public void GrabCompass(Transform leftHand){
        compass.GetComponent<XRGrabInteractable>().enabled = false;
        compass.GetComponent<SphereCollider>().isTrigger = true;
        compass.transform.parent = leftHand;
        compass.transform.localPosition = new Vector3(-0.0668f,-0.0311f,0.0034f);
        compass.transform.localRotation = new Quaternion(-0.5991f,0.6835f,0.3019f,0.2874f);
    }

}