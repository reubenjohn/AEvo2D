using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OdorSource : MonoBehaviour
{
    private Environment environment;
    private ParticleSystem particles;
    private ChemicalBag chemicalBag;

    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    // Start is called before the first frame update
    void Start()
    {
        environment = GetComponentInParent<Environment>();
        particles = GetComponent<ParticleSystem>();
        chemicalBag = GetComponent<ChemicalBag>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 10 == 0)
        {
            var main = particles.main;
            main.startColor = Color.Lerp(Color.grey, Color.red, chemicalBag.ApproximateMass / 100);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particles.GetCollisionEvents(other, collisionEvents);
        for (int i = 0; i < numCollisionEvents; i++)
            collisionEvents[i].colliderComponent.gameObject.GetComponent<Nose>()
            .OnOdorDetected(this, GetOdors(chemicalBag), collisionEvents[i]);
    }

    private Mixture GetOdors(ChemicalBag chemicalBag)
    {
        return new Mixture(environment.odorousSubstances.ToDictionary(
            substance => substance,
            substance => chemicalBag[substance]
        ));
    }
}

public class Odor
{

}