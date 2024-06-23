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

    [Header("Events")] public static Action onHit;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out PlayerController playerController))
        {
            Bounce(collision.GetContact(0).normal);
            onHit?.Invoke();
        }
    }


    private void Bounce(Vector2 normal)
    {

        rig.velocity = normal * bounceVelocity;
    }
}
