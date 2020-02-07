using System;
using UnityEngine;
using static Substance;

public class Creature : MonoBehaviour
{
    private Environment environment;
    private Rigidbody2D rb;
    public ChemicalBag ChemicalBag { get; private set; }
    private EggPouch eggPouch;
    private Brain brainConnection;

    [SerializeField] public GameObject meat;

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
        ChemicalBag = body.GetComponent<ChemicalBag>();

        eggPouch = GetComponentInChildren<EggPouch>();

        this.brainConnection = GetComponentInChildren<Brain>();
        this.brainConnection.Connect(GetComponentsInChildren<ISensor>(), GetComponentsInChildren<IActuator>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 100 == 0)
        {
            brainConnection.Act();

            foreach (var recipe in alwaysActiveRecipes)
                ChemicalBag.Convert(recipe);

            rb.AddTorque((ChemicalBag.Convert(Recipe.TURN_RIGHT) - ChemicalBag.Convert(Recipe.TURN_LEFT)) * environment.torqueImpulse, ForceMode2D.Impulse);
            rb.AddForce(rb.transform.up * ChemicalBag.Convert(Recipe.JET_FORWARD) * environment.fuelImpulse, ForceMode2D.Impulse);

            if (ChemicalBag[WASTE] > environment.minimumGlobuleMass)
            {
                ChemicalBag.TransferMax(environment.chemicalBag, ChemicalBag, WASTE_MIX);
            }

            if (SkinThickness() < environment.minSkinThickness)
                Die();
        }
    }

    private float SkinThickness()
    {
        float skinArea = ChemicalBag.ApproximateMass / environment.massDensity;
        float bodyArea = (ChemicalBag.ApproximateMass - ChemicalBag[SKIN]) / environment.massDensity;
        return Mathf.Sqrt(skinArea) - Mathf.Sqrt(bodyArea);
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " died");
        float mass = this.ChemicalBag.ExactMass();
        GameObject newMeat = Instantiate(meat, transform.position, transform.rotation, transform.parent);
        newMeat.GetComponent<ChemicalBag>().initialMass = new SubstanceMass[] { new SubstanceMass() { substance = MEAT, mass = mass } };
        Destroy(gameObject);
    }
}
