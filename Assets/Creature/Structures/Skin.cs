using UnityEngine;

public class Skin : MonoBehaviour
{
    public Transform body;

    private Environment environment;
    private ChemicalBag chemicalBag;

    // Start is called before the first frame update
    void Start()
    {
        environment = GetComponentInParent<Environment>();
        chemicalBag = GetComponentInParent<ChemicalBag>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 10 == 0)
        {
            float bodyMass = chemicalBag.ApproximateMass - chemicalBag[Substance.SKIN];
            body.localScale = environment.Scale(bodyMass);
        }
    }
}
