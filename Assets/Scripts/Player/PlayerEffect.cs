using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem landParticle;
    [SerializeField] private ParticleSystem jumpParticle;

    public void JumpEffect()
    {
        jumpParticle.Play();
    }

    public void LandEffect()
    {
        landParticle.Play();
    }
}
