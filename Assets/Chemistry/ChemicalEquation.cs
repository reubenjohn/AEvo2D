using System.Collections;
using System.Collections.Generic;

public static class ChemicalEquation
{
    public class Term : SubstanceMass
    {
        public static Expression operator +(Term a, Term b) => new Expression() { [a.substance] = a.mass, [b.substance] = b.mass };
        public static Reaction operator >(Term a, Term b) => new Expression() { [a.substance] = a.mass } > new Expression() { [b.substance] = b.mass };
        public static Reaction operator <(Term a, Term b) => throw new System.NotImplementedException();
    }

    public class Expression : MixtureDictionary
    {
        public Expression() { }
        public Expression(Mixture mix)
        {
            foreach (var substance in mix.Keys)
            {
                Add(substance, mix[substance]);
            }
        }

        public static Expression operator +(Expression a, Term b) => new Expression(a.ToMixture() + new Expression() { [b.substance] = b.mass }.ToMixture());
        public static Reaction operator >(Expression a, Expression b) => new Reaction(a.ToMixture(), b.ToMixture());
        public static Reaction operator >(Expression a, Term b) => a > new Expression() { [b.substance] = b.mass };
        public static Reaction operator >(Term a, Expression b) => new Expression() { [a.substance] = a.mass } > b;
        public static Reaction operator <(Expression a, Expression b) => throw new System.NotImplementedException();
        public static Reaction operator <(Expression a, Term b) => throw new System.NotImplementedException();
        public static Reaction operator <(Term a, Expression b) => throw new System.NotImplementedException();
    }

    public static Term M(this Substance substance, float mass) => new Term() { substance = substance, mass = mass };
    public static Term M(this Substance substance, double mass) => M(substance, (float)mass);
}
