using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcelSpawn : MonoBehaviour {
    public GameObject[] parcels;

    void OnEnable() {
        int draw = Random.Range(0, parcels.Length);
        Instantiate(parcels[draw], transform.position, transform.rotation);
    }
}
