using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    public GameObject npcPrefab;
    public float _spawnTime = 10f;
    float _waitTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(npcPrefab, transform.position + new Vector3(0.0f, 0.0f, -10f), Quaternion.Euler(0f, 180f, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (_waitTime > _spawnTime)
        {
            Instantiate(npcPrefab, transform.position + new Vector3(0.0f, 0.0f, -10f), Quaternion.Euler(0f, 180f, 0f));
            _waitTime = 0.0f;
        }
        _waitTime += Time.deltaTime;
    }
}
