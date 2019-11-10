using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{

    protected Rigidbody rb;
    [HideInInspector]
    public float force;
    [HideInInspector]
    public float damage;
    protected TrailRenderer trail;
    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (Collider item in GetComponents<Collider>())
        {
            item.enabled = false;
        }
        if (other.GetComponent<Rigidbody>())
        {
            other.GetComponent<Rigidbody>().AddExplosionForce(50f *force, transform.position, 20f);
        }
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        transform.parent = other.transform;
        trail.emitting = false;
        if (other.GetComponent<Enemy>())
        {
            GameObject blood = Instantiate(other.GetComponent<Enemy>().BloodPrefab);
            blood.transform.position = other.ClosestPoint(transform.position);
            blood.transform.rotation = transform.rotation;
            other.GetComponent<Enemy>().Life -= damage;
        } else
        {
            StartCoroutine(DestroyTime());
        }

    }

    protected IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
