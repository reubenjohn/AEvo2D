using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : MonoBehaviour
{
    [SerializeField] public float maxBrownianForce = 1f;
    private Rigidbody2D rb;

    public static readonly Mixture MEAT_MIX = new MixtureDictionary { [Substance.MEAT] = 1f }.ToMixture();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float magnitude = Random.Range(-maxBrownianForce, maxBrownianForce);
        float angle = Random.Range(-180, 180);
        rb.AddForce(magnitude * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Meat meat))
        {
            ChemicalBag otherChemicalBag = other.gameObject.GetComponent<ChemicalBag>();
            ChemicalBag chemicalBag = GetComponent<ChemicalBag>();
            float mass = chemicalBag.ExactMass();
            float otherMass = otherChemicalBag.ExactMass();
            if (mass >= otherMass)
            {
                ChemicalBag.TransferMax(chemicalBag, otherChemicalBag, MEAT_MIX);
                Destroy(other.gameObject);
            }
        }
    }
}
