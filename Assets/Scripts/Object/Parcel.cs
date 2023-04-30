using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parcel : MonoBehaviour {
    public Player center;
    public Rigidbody2D rb;
    public GameObject pickupEffect;
    public bool isPickedUp = false;

    private float rotationModifier = 0f;

    void Start() {
        rb.mass = MetaData.PARCEL_MASS;
        center = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (isPickedUp) {
            FollowCursor();
        }

        if (!isPickedUp && rb.velocity.magnitude < 1) {
            FreezeParcel();
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (isPickedUp) {
                DropParcel();
            } else {
                if (IsWithinPickupRange()) {
                    TryPickupParcel();
                }
            }
        }
    }

    bool IsWithinPickupRange() {
        if (center != null) {
            return Vector3.Distance(center.transform.position, transform.position) < MetaData.PARCEL_PICK_RANGE;
        }
        return false;
    }

    public float GetDamage() {
        return MetaData.PARCEL_DAMAGE;
    }

    void DropParcel() {
        isPickedUp = false;
        pickupEffect.SetActive(false);
    }

    void FreezeParcel() {
        rb.bodyType = RigidbodyType2D.Static;
        gameObject.tag = "Obstacle";
    }

    void TryPickupParcel() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == gameObject) {
            PickupParcel();
        }
    }

    void PickupParcel() {
        isPickedUp = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        gameObject.tag = "Parcel";
        pickupEffect.SetActive(true);
    }

    void FollowCursor() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        rb.velocity = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position).normalized * MetaData.PARCEL_SPEED;

        if (center != null) {
            RotateParcel();
            LimitWithinRange();
        }
    }

    void RotateParcel() {
        Vector3 vectorToTarget = center.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * MetaData.PARCEL_SPEED);
    }

    void LimitWithinRange() {
        // Set drag limitation
        Vector3 centerPosition = center.transform.localPosition; //center of *black circle*
        Vector3 newLocation = transform.position; //*BlackCenter* + all that Math
        float distance = Vector3.Distance(newLocation, centerPosition); //distance from ~green object~ to *black circle*

        if (distance > MetaData.DRAG_RADIUS) //If the distance is less than the radius, it is already within the circle.
        {
            Vector3 fromOriginToObject = newLocation - centerPosition; //~GreenPosition~ - *BlackCenter*
            fromOriginToObject *= MetaData.DRAG_RADIUS / distance; //Multiply by radius //Divide by Distance
            newLocation = centerPosition + fromOriginToObject; //*BlackCenter* + all that Math
            transform.position = newLocation;
        }
    }
}
