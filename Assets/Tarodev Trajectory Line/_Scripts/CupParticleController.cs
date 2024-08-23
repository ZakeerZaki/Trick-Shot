using UnityEngine;

public class CupParticleController : MonoBehaviour
{
    public ParticleSystem particleSystem;

    void Start()
    {
        // Ensure the Particle System is assigned
        if (particleSystem == null)
        {
            particleSystem = GetComponentInChildren<ParticleSystem>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the tag "Ball"
        if (collision.gameObject.CompareTag("Ball"))
        {
            PlayParticles();
        }
    }

    public void PlayParticles()
    {
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }

    public void StopParticles()
    {
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }
    }
}
