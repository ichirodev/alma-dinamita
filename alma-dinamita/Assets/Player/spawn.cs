using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public class spawn : MonoBehaviour
{
    [SerializeField] public List<Vector3> spawnPoints = new List<Vector3>(5);
    [SerializeField] public GameObject playerPrefab;
    private void Start()
    {
        #if UNITY_EDITOR
        Instantiate(playerPrefab, spawnPoints[2], Quaternion.identity);
        #else
        Instantiate(playerPrefab, spawnPoints[Rand.Range(0, 5)], Quaternion.identity);
        #endif
    }
}