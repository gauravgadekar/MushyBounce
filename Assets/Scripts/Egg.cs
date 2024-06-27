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
            isAlive = false;
            onFellInWater?.Invoke();
            
        }
    }


    private void Bounce(Vector2 normal)
    {

        rig.velocity = normal * bounceVelocity;
    }

    public void Reuse()
    {
       transform.position = Vector2.up * 5;
       transform.rotation = new Quaternion(0, 0, 0,0);
       rig.velocity = Vector2.zero;
       rig.angularVelocity = 0;
       rig.gravityScale = 0;

       isAlive = true;
       
       StartCoroutine("WaitAndFall");

    }
}
