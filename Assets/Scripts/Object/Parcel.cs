using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parcel : MonoBehaviour {
    private float damage = 2f;
    private float speed = 20f;
    private float pickRange = 5f;
    private float mass = 10f;
    private float radius = 0f;

    public Player center;
    public Rigidbody2D rb;
    public GameObject pickupEffect;

    private bool isPickedUp = false;
    private float rotationModifier = 0f;

    void Start() {
        rb.mass = mass;
        if (center != null) {
            radius = center.GetDragRange();
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (isPickedUp) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            rb.velocity = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position).normalized * speed;

            if (center != null) {
                Vector3 vectorToTarget = center.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);

                // Set drag limitation
                Vector3 centerPosition = center.transform.localPosition; //center of *black circle*
                Vector3 newLocation = transform.position; //*BlackCenter* + all that Math
                float distance = Vector3.Distance(newLocation, centerPosition); //distance from ~green object~ to *black circle*

                if (distance > radius) //If the distance is less than the radius, it is already within the circle.
                {
                    Vector3 fromOriginToObject = newLocation - centerPosition; //~GreenPosition~ - *BlackCenter*
                    fromOriginToObject *= radius / distance; //Multiply by radius //Divide by Distance
                    newLocation = centerPosition + fromOriginToObject; //*BlackCenter* + all that Math
                    transform.position = newLocation;
                }
            }
        }

        if (!isPickedUp && rb.velocity.magnitude < 1) {
            rb.bodyType = RigidbodyType2D.Static;
            gameObject.tag = "Obstacle";
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (IsWithinPickupRange()) {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject) {
                    isPickedUp = true;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    gameObject.tag = "Parcel";
                    pickupEffect.SetActive(true);
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) {
        }
    }

    bool IsWithinPickupRange() {
        if (center != null) {
            return Vector3.Distance(center.transform.position, transform.position) < pickRange;
        }
        return false;
    }

    public float GetDamage() {
        return damage;
    }
}
