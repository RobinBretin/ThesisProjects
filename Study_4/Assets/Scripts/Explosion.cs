using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _triggerForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private GameObject _particles;
    [SerializeField] private AudioSource audio;
    private Color color;

    void Start()
    {
        color = GetComponent<Renderer>().material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Participant")
        {
            var surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);

            foreach(var obj in surroundingObjects)
            {
                var rb = obj.GetComponent<Rigidbody>();
                if (rb == null) continue;

                rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }

            Instantiate(_particles, transform.position, Quaternion.identity);
            audio.Play();
            Destroy(gameObject);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Drone"))
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Drone"))
        {
            GetComponent<Renderer>().material.color = color;
        }
    }

}
