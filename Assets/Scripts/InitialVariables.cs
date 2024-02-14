using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InitialVariables", menuName = "ScriptableObjects/InitialVariables")]
public class InitialVariables : ScriptableObject
{
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
    public int timer;
    private bool isTimerRunning;
}