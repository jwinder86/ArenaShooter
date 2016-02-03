using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Effects/Particle Behaviour")]
public class ParticleBehaviour : PoolableBehaviour {

    public bool singleBurst;
    
    // components
    private ParticleSystem cachedParticles;
    private ParticleSystem particles {
        get {
            if (cachedParticles == null) {
                cachedParticles = GetComponent<ParticleSystem>();
                cachedParticles.playOnAwake = false;
            }
            return cachedParticles;
        }
    }

    public override void Activate() {
        gameObject.SetActive(true);
        StartCoroutine(ParticleLifeRoutine());
    }

    public override void Deactivate() {
        particles.Stop(false);
        particles.Clear(false);

        gameObject.SetActive(false);
    }

    private IEnumerator ParticleLifeRoutine() {
        ParticleSystem.EmissionModule emission = particles.emission;
        emission.enabled = true;
        particles.Play(false);

        // only wait single frame for burst particles
        if (singleBurst) {
            yield return null;
        } else {
            yield return new WaitForSeconds(particles.duration);
        }

        yield return StartCoroutine(CleanUpRoutine());
    }

    private IEnumerator CleanUpRoutine() {
        ParticleSystem.EmissionModule emission = particles.emission;
        emission.enabled = false;

        yield return new WaitForSeconds(particles.startLifetime);

        ReturnToPool();
    }
}

