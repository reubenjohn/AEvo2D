using System;
using UnityEngine;
using static Substance;

public class Creature : MonoBehaviour
{
    private Environment environment;
    private Rigidbody2D rb;
    public ChemicalBag chemicalBag;
    private EggPouch eggPouch;
    private Brain brainConnection;

    [SerializeField] public GameObject meat;
    [SerializeField] public string inheritedName;

    public static readonly Mixture WASTE_MIX = new MixtureDictionary { [Substance.WASTE] = 1f }.ToMixture();

    private static readonly Recipe[] alwaysActiveRecipes = new Recipe[] {
        Recipe.AGE_SKIN,
        Recipe.PROCESS_TOXIN,
        Recipe.PROCESS_VENOM,
        Recipe.DIGESTIVE_ENZYME_INTERFERENCE,
        Recipe.RESORB_PREGNANCY_HORMONE,
    };

    // Start is called before the first frame update
    void Start()
    {
        environment = GetComponentInParent<Environment>();

        Transform body = transform.Find("Body");
        rb = body.GetComponent<Rigidbody2D>();
        chemicalBag = body.GetComponent<ChemicalBag>();

        eggPouch = GetComponentInChildren<EggPouch>();

        this.brainConnection = GetComponentInChildren<Brain>();
        this.brainConnection.Connect(GetComponentsInChildren<ISensor>(), GetComponentsInChildren<IActuator>());

        inheritedName = string.IsNullOrEmpty(inheritedName) ? gameObject.name : inheritedName;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 100 == 0)
        {
            brainConnection.Act();

            foreach (var recipe in alwaysActiveRecipes)
                chemicalBag.Convert(recipe);

            rb.AddTorque((chemicalBag.Convert(Recipe.TURN_RIGHT) - chemicalBag.Convert(Recipe.TURN_LEFT)) * environment.torqueImpulse, ForceMode2D.Impulse);
            rb.AddForce(rb.transform.up * chemicalBag.Convert(Recipe.JET_FORWARD) * environment.fuelImpulse, ForceMode2D.Impulse);

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
        Debug.Log(gameObject.name + " died");
        float mass = this.chemicalBag.ExactMass();
        GameObject newMeat = Instantiate(meat, transform.position, transform.rotation, transform.parent);
        newMeat.GetComponent<ChemicalBag>().initialMass = new SubstanceMass[] { new SubstanceMass() { substance = MEAT, mass = mass } };
        Destroy(gameObject);
    }
}
