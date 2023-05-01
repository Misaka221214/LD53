using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reroll : MonoBehaviour {
    public GameObject[] parcels;

    private void OnTriggerStay2D(Collider2D collision) {
        Parcel parcel = collision.gameObject.GetComponent<Parcel>();
        if (parcel) {
            Transform t = parcel.transform;
            Destroy(parcel.gameObject);
            Instantiate(parcels[Random.Range(0, parcels.Length)], t.position, t.rotation);
        }
    }
}
