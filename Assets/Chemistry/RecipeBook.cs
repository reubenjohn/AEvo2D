
using System;
using static ChemicalEquation;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Substance;

public enum Recipe
{
    GROW_SKIN, AGE_SKIN, PROCESS_TOXIN, MAKE_VENOM, PROCESS_VENOM,
    VEGETATION_DIGESTIVE_ENZYME, MEAT_DIGESTIVE_ENZYME, DIGESTIVE_ENZYME_INTERFERENCE,
    DIGEST_VEGETATION, DIGEST_MEAT,
    GROW_FUEL_GLAND, GROW_CILIA,
    MAKE_JET_FUEL, MAKE_LEFT_FUEL, MAKE_RIGHT_FUEL,
    JET_FORWARD, TURN_LEFT, TURN_RIGHT,

    GROW_EGG, MATURE_EGG, GROW_BABY, RESORB_PREGNANCY_HORMONE
}


public class RecipeBook : MonoBehaviour
{
    Dictionary<Recipe, Reaction> recipes = new Dictionary<Recipe, Reaction>
    {
        // Vitals
        [Recipe.GROW_SKIN] =
            SKIN_GROWTH_FACTOR.M(1) + FAT.M(1) >
            SKIN_GROWTH_FACTOR.M(1) + SKIN.M(.5) + WASTE.M(.5),
        [Recipe.AGE_SKIN] =
            SKIN_GROWTH_FACTOR.M(1) + SKIN.M(.01) >
            SKIN_GROWTH_FACTOR.M(1) + WASTE.M(.01),
        [Recipe.PROCESS_TOXIN] = SKIN.M(.5) + TOXIN.M(.5) > WASTE.M(1),
        [Recipe.MAKE_VENOM] =
            VENOM_GLANDS.M(1) + FAT.M(1) >
            VENOM_GLANDS.M(1) + VENOM.M(.8) + WASTE.M(.2),
        [Recipe.PROCESS_VENOM] = SKIN.M(.5) + VENOM.M(.5) > WASTE.M(1),

        // Digestion
        [Recipe.VEGETATION_DIGESTIVE_ENZYME] =
            GROWTH_HORMONE.M(1) + FAT.M(1) >
            GROWTH_HORMONE.M(1) + VEGETATION_DIGESTIVE_ENZYME.M(1),
        [Recipe.MEAT_DIGESTIVE_ENZYME] =
            GROWTH_HORMONE.M(1) + FAT.M(1) >
            GROWTH_HORMONE.M(1) + MEAT_DIGESTIVE_ENZYME.M(1),
        [Recipe.DIGESTIVE_ENZYME_INTERFERENCE] =
            OMNIVORE_INTERFERENCE.M(1) + VEGETATION_DIGESTIVE_ENZYME.M(.005) + MEAT_DIGESTIVE_ENZYME.M(.005) >
            OMNIVORE_INTERFERENCE.M(1) + WASTE.M(.01),
        [Recipe.DIGEST_VEGETATION] =
            VEGETATION_DIGESTIVE_ENZYME.M(1) + VEGETATION.M(1) >
            VEGETATION_DIGESTIVE_ENZYME.M(1) + FAT.M(.5) + WASTE.M(.5),
        [Recipe.DIGEST_MEAT] =
            MEAT_DIGESTIVE_ENZYME.M(1) + MEAT.M(1) >
            MEAT_DIGESTIVE_ENZYME.M(1) + FAT.M(.8) + WASTE.M(.2),

        // Locomotion
        [Recipe.GROW_FUEL_GLAND] =
            GROWTH_HORMONE.M(1) + FAT.M(1) >
            GROWTH_HORMONE.M(1) + FUEL_GLAND.M(1),
        [Recipe.GROW_CILIA] =
            GROWTH_HORMONE.M(1) + FAT.M(1) >
            GROWTH_HORMONE.M(1) + CILIA.M(1),
        [Recipe.MAKE_JET_FUEL] =
            FUEL_GLAND.M(1) + FAT.M(1) >
            FUEL_GLAND.M(1) + JET_FUEL.M(.6) + WASTE.M(.4),
        [Recipe.MAKE_LEFT_FUEL] =
            CILIA.M(1) + FAT.M(1) >
            CILIA.M(1) + LEFT_FUEL.M(.6) + WASTE.M(.4),
        [Recipe.MAKE_RIGHT_FUEL] =
            CILIA.M(1) + FAT.M(1) >
            CILIA.M(1) + RIGHT_FUEL.M(.6) + WASTE.M(.4),
        [Recipe.JET_FORWARD] = JET_FUEL.M(1) > WASTE.M(1),
        [Recipe.TURN_LEFT] = LEFT_FUEL.M(1) > WASTE.M(1),
        [Recipe.TURN_RIGHT] = RIGHT_FUEL.M(1) > WASTE.M(1),

        // Reproduction
        [Recipe.GROW_EGG] =
            EGG_GROWTH_FACTOR.M(1) + FAT.M(1.01) >
            EGG_GROWTH_FACTOR.M(1) + EGG.M(.9) + WASTE.M(.1) + PREGNANCY_HORMONE.M(.01),
        [Recipe.MATURE_EGG] =
            BABY_GROWTH_FACTOR.M(1) + EGG.M(1) >
            BABY_GROWTH_FACTOR.M(1) + BABY.M(1),
        [Recipe.GROW_BABY] =
            BABY_GROWTH_FACTOR.M(1) + FAT.M(1.01) >
            BABY_GROWTH_FACTOR.M(1) + BABY.M(.9) + WASTE.M(.1) + PREGNANCY_HORMONE.M(.01),
        [Recipe.RESORB_PREGNANCY_HORMONE] =
            PREGNANCY_HORMONE_RESORBER.M(1) + PREGNANCY_HORMONE.M(1) >
            PREGNANCY_HORMONE_RESORBER.M(1) + FAT.M(1)
    };

    public Dictionary<Recipe, Reaction>.KeyCollection Keys
    {
        get => this.recipes.Keys;
    }

    public Reaction this[Recipe key]
    {
        get => recipes.Get(key);
        set => recipes[key] = value;
    }
}
