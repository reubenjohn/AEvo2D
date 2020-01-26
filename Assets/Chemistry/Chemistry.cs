using System.Collections.Generic;
using System.Linq;
using System;
using static Substance;
using UnityEngine;

public enum Substance
{
    GROWTH_HORMONE,
    FAT, WASTE,
    SKIN_GROWTH_FACTOR, SKIN, TOXIN, VENOM, VENOM_GLANDS,
    VEGETATION, MEAT, VEGETATION_DIGESTIVE_ENZYME, MEAT_DIGESTIVE_ENZYME, OMNIVORE_INTERFERENCE,
    FUEL_GLAND, CILIA, JET_FUEL, LEFT_FUEL, RIGHT_FUEL,
    EGG, EGG_GROWTH_FACTOR, BABY, BABY_GROWTH_FACTOR, PREGNANCY_HORMONE, PREGNANCY_HORMONE_RESORBER
}

[Serializable]
public class SubstanceMass
{
    public Substance substance;
    public float mass;
}

public static class SubstanceMassUtilities
{
    public static float TotalMass(this SubstanceMass[] substanceMasses) => substanceMasses.Sum(substanceMass => substanceMass.mass);
}


public class Reaction
{
    public readonly Mixture ingredients;
    public readonly Mixture effects;
    public readonly Mixture change;

    public Reaction(Mixture ingredients, Mixture effects)
    {
        this.ingredients = ingredients;
        this.effects = effects;
        this.change = effects - ingredients;
        if (Math.Abs(this.change.TotalMass) > 1e-6)
            throw new ArgumentException(string.Format("Mass must be conserved in a reaction [{4}][{3}]({0} != {1}):\n{2}",
                ingredients.TotalMass.ToString(), effects.TotalMass.ToString(), ToString(), change, change.TotalMass));
    }

    public Reaction(MixtureDictionary ingredients, MixtureDictionary effects) : this(ingredients.ToMixture(), effects.ToMixture()) { }

    public override string ToString()
    {
        return ingredients.ToString() + " -> " + effects.ToString();
    }
}
