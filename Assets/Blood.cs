using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    protected ParticleSystem particleSystems;
    void Start()
    {
        particleSystems = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particleSystems.isStopped)
        {
            Destroy(gameObject);
        }
    }
}
