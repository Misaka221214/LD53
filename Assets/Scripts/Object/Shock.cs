using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : MonoBehaviour {
    private GameObject center;

    private void Start() {
        center = GameObject.Find("Player");
    }

    private void FixedUpdate() {
        if (center) {
            transform.position = center.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Bullet")) {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            if (e) {
                e.DisableFreeMove();
            }
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.gameObject.transform.position - transform.position).normalized * 25f);
        }
    }
}
