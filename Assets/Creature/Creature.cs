using UnityEngine;
using static Substance;

public class Creature : MonoBehaviour
{
    private Environment environment;
    private Rigidbody2D rb;
    private ChemicalBag chemicalBag;
    private Brain brainConnection;

    public static readonly Mixture WASTE_MIX = new MixtureDictionary { [Substance.WASTE] = 1f }.ToMixture();

    // Start is called before the first frame update
    void Start()
    {
        environment = GetComponentInParent<Environment>();
        rb = GetComponent<Rigidbody2D>();
        chemicalBag = GetComponent<ChemicalBag>();

        this.brainConnection = GetComponent<Brain>();
        this.brainConnection.Connect(GetComponentsInChildren<ISensor>(), GetComponentsInChildren<IActuator>(), environment.GetVoluntaryRecipes().Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 10 == 0)
        {
            brainConnection.Act();

            chemicalBag.Convert(Recipe.AGE_SKIN);

            rb.AddTorque((chemicalBag.Convert(Recipe.TURN_RIGHT) - chemicalBag.Convert(Recipe.TURN_LEFT)) * environment.torqueImpulse, ForceMode2D.Impulse);
            rb.AddForce(transform.up * chemicalBag.Convert(Recipe.JET_FORWARD) * environment.fuelImpulse, ForceMode2D.Impulse);

            if (chemicalBag[WASTE] > environment.minimumGlobuleMass)
            {
                ChemicalBag.TransferMax(environment.chemicalBag, chemicalBag, WASTE_MIX);
            }

            if (SkinThickness() < environment.minSkinThickness)
                Die();
        }
    }

    private float SkinThickness()
    {
        float skinArea = chemicalBag.ApproximateMass / environment.massDensity;
        float bodyArea = (chemicalBag.ApproximateMass - chemicalBag[SKIN]) / environment.massDensity;
        return Mathf.Sqrt(skinArea) - Mathf.Sqrt(bodyArea);
    }

    private void Die()
    {
        ChemicalBag.Transfer(environment.chemicalBag, this.chemicalBag, this.chemicalBag.ToMixture());
        Destroy(gameObject);
    }
}
