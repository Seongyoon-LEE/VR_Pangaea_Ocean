using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRewind : MonoBehaviour
{
    ParticleSystem particleSystem;
    private void Start()
    {
        this.particleSystem = GetComponent<ParticleSystem>();
    }
    private void FixedUpdate()
    {
        particleSystem.Simulate(10, true, false);
    }
}
