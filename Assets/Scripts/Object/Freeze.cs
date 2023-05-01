using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy e = collision.gameObject.GetComponent<Enemy>();
        if (e) {
            e.SetFreeze();
        }
    }
}
