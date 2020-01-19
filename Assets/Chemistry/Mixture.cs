
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using static Creature;

public class Mixture
{
    protected Dictionary<Substance, float> contents;
    private readonly Lazy<float> totalMass;


    public Mixture()
    {
        contents = new Dictionary<Substance, float>();
        totalMass = new Lazy<float>(this.Mass);
    }

    public Mixture(IDictionary<Substance, float> source)
    {
        contents = new Dictionary<Substance, float>(source);
        totalMass = new Lazy<float>(this.Mass);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public float TotalMass
    {
        get { return totalMass.Value; }
    }

    public float Mass()
    {
        return contents.Values.Sum();
    }

    public Mixture Copy()
    {
        return new Mixture(contents);
    }

    public override string ToString()
    {
        return "{ " + string.Join(", ", contents.Select(pair => pair.Key + ": " + pair.Value)) + " }";
    }

    public Dictionary<Substance, float>.KeyCollection Keys
    {
        get => contents.Keys;
    }

    public Dictionary<Substance, float>.ValueCollection Values
    {
        get => contents.Values;
    }

    public float this[Substance key]
    {
        get => VirtualGet(key);
        protected set => contents[key] = value;
    }

    protected float VirtualGet(Substance key)
    {
        if (!contents.ContainsKey(key))
            contents[key] = 0f;
        return contents[key];
    }

    public static Mixture operator *(Mixture a, float scale)
    {
        return Reduce(a, substance => a[substance] * scale);
    }

    public static Mixture operator +(Mixture a, Mixture b)
    {
        return Reduce(a, b, substance => a[substance] + b[substance]);
    }

    public static Mixture operator -(Mixture a, Mixture b)
    {
        return Reduce(a, b, substance => a[substance] - b[substance]);
    }

    public static Mixture Reduce(Mixture a, Func<Substance, float> reduction)
    {
        return new Mixture(a.contents.Keys.ToDictionary(substance => substance, reduction));
    }

    public static Mixture Reduce(Mixture a, Mixture b, Func<Substance, float> reduction)
    {
        return new Mixture(a.contents.Keys.Union(b.contents.Keys).ToDictionary(substance => substance, reduction));
    }

    internal Mixture Select(params Substance[] substances)
    {
        return new Mixture(contents.Where(pair => substances.Contains(pair.Key)).ToDictionary(pair => pair.Key, pair => pair.Value));
    }

    internal MixtureDictionary ToMixtureDictionary()
    {
        MixtureDictionary mixDict = new MixtureDictionary();
        foreach (var entry in contents)
            mixDict[entry.Key] = entry.Value;
        return mixDict;
    }
}

public class MixtureDictionary : Dictionary<Substance, float>
{
    public Mixture ToMixture()
    {
        return new Mixture(this);
    }
    
    public static Reaction operator >(MixtureDictionary a, MixtureDictionary b) => new Reaction(a,b);
    public static Reaction operator <(MixtureDictionary a, MixtureDictionary b) => throw new System.NotImplementedException();
}

public class Flask : Mixture
{
    private static readonly IEnumerable<float> ENUMERABLE_ONE = new float[] { 1f };

    public Flask() : base()
    {
    }

    public Flask(IDictionary<Substance, float> initialMix) : base(initialMix)
    {
    }

    private bool Take(Mixture b)
    {
        if (!this.MassesGreaterThanEqualTo(b))
            return false;
        foreach (var substance in b.Keys)
            this[substance] -= b[substance];
        return true;
    }

    private void Put(Mixture b)
    {
        foreach (var substance in b.Keys)
            this[substance] = b[substance] + (this.contents.ContainsKey(substance) ? this[substance] : 0);
    }

    public bool MassesGreaterThanEqualTo(Mixture b)
    {
        return b.Keys.All(substance => this[substance] >= b[substance]);
    }


    public static float MaxYield(Mixture available, Mixture required)
    {
        return required.Keys
        .Where(substance => required[substance] != 0)
        .Select(substance => available[substance] / required[substance])
        .Min();
    }

    internal float Convert(Reaction reaction)
    {
        float yield = MaxYield(this, reaction.ingredients);
        Mixture transferMixture = reaction.change * yield;

        Take(reaction.ingredients * yield);
        Put(reaction.effects * yield);

        return yield;
    }

    public static bool Transfer(Flask destination, Flask source, Mixture transferMixture)
    {
        if (source.Take(transferMixture))
        {
            destination.Put(transferMixture);
            return true;
        }
        return false;
    }

    public new float this[Substance key]
    {
        get => VirtualGet(key);
        private set => contents[key] = value;
    }
}
