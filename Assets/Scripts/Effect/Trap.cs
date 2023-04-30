using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {
    public TrapType trapType;

    private void OnTriggerEnter2D(Collider2D collision) {
        switch (trapType) {
            case TrapType.FIRE:
                FireOnEnter(collision);
                break;
            case TrapType.SWAMP:
                SwampOnEnter(collision);
                break;
            case TrapType.HOLE:
                break;
            case TrapType.NONE:
            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        switch (trapType) {
            case TrapType.FIRE:
                FireOnExit(collision);
                break;
            case TrapType.SWAMP:
                SwampOnExit(collision);
                break;
            case TrapType.HOLE:
                break;
            case TrapType.NONE:
            default:
                break;
        }
    }

    private void FireOnEnter(Collider2D collision) {
        Player enteringPlayer = collision.gameObject.GetComponent<Player>();
        if (enteringPlayer) {
            enteringPlayer.isBurning = true;
        }

        Enemy enteringEnemy = collision.gameObject.GetComponent<Enemy>();
        if (enteringEnemy) {
            enteringEnemy.isBurning = true;
        }
    }

    private void FireOnExit(Collider2D collision) {
        Player exitingPlayer = collision.gameObject.GetComponent<Player>();
        if (exitingPlayer) {
            exitingPlayer.isBurning = false;
        }

        Enemy exitingEnemy = collision.gameObject.GetComponent<Enemy>();
        if (exitingEnemy) {
            exitingEnemy.isBurning = false;
        }
    }

    private void SwampOnEnter(Collider2D collision) {
        Player enteringPlayer = collision.gameObject.GetComponent<Player>();
        if (enteringPlayer) {
            enteringPlayer.isSwamped = true;
        }

        Enemy enteringEnemy = collision.gameObject.GetComponent<Enemy>();
        if (enteringEnemy) {
            enteringEnemy.isSwamped = true;
        }
    }

    private void SwampOnExit(Collider2D collision) {
        Player exitingPlayer = collision.gameObject.GetComponent<Player>();
        if (exitingPlayer) {
            exitingPlayer.isSwamped = false;
        }

        Enemy exitingEnemy = collision.gameObject.GetComponent<Enemy>();
        if (exitingEnemy) {
            exitingEnemy.isSwamped = false;
        }
    }
}
