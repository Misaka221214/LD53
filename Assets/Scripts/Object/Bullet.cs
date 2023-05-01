using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Vector2 velocity = new Vector2();
    private Rigidbody2D rb;
    private float destroyCounter = MetaData.BULLET_DESTROY_TIME;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (destroyCounter < 0f) {
            Destroy(gameObject);
        }
        destroyCounter -= Time.deltaTime;
        //rb.velocity = velocity;
    }

    public void SetVelocity(Vector2 v) {
        velocity = v;
    }
}
