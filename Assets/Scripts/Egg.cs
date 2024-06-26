using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Egg : MonoBehaviour
{

    [Header("Physics Settings")]
    [SerializeField]
    private float bounceVelocity;
    private Rigidbody2D rig;
    private bool isAlive = true;
    private float gravityScale;

    [Header("Events")] public static Action onHit;
    public static Action onFellInWater;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        isAlive = true;

        gravityScale = rig.gravityScale;
        rig.gravityScale = 0;

        StartCoroutine("WaitAndFall");
    }

    IEnumerator WaitAndFall()
    {
        yield return new WaitForSeconds(2);
        rig.gravityScale = gravityScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive)
        {
            return;
        }
        if (collision.collider.TryGetComponent(out PlayerController playerController))
        {
            Bounce(collision.GetContact(0).normal);
            onHit?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isAlive)
        {
            return;
        }
        if (collider.CompareTag("water"))
        {
            onFellInWater?.Invoke();
            isAlive = false;
        }
    }


    private void Bounce(Vector2 normal)
    {

        rig.velocity = normal * bounceVelocity;
    }
}
