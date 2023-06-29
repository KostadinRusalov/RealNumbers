using System;
using System.Collections.Generic;

namespace RealNumbers
{
    public class Fraction
    {
        public Number Numerator { get; set; }
        public Number Denominator { get; set; }

        public Fraction(Number numerator, Number denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Denominator can't be negative!");
            }
            if (denominator < 0)
            {
                Numerator = -numerator;
                Denominator = -denominator;
            }
            else
            {
                Numerator = numerator;
                Denominator = denominator;
            }
            Reduce();
            Rationalise();
        }
        public Fraction(int num)
                : this(num, 1) { }

        public Fraction(Irrational i)
                : this(i, 1) { }

        public Fraction(Number n)
                : this(n, 1) { }

        public Fraction(int num, int den)
                : this(new Number(num), new Number(den)) { }

        public Fraction(Irrational num, Irrational den)
                : this(new Number(num), new Number(den)) { }

        public Fraction(int num, Irrational den)
                : this(new Number(num), new Number(den)) { }

        public Fraction(Irrational num, int den)
                : this(new Number(num), new Number(den)) { }

        public Fraction(int num, Number den)
                : this(new Number(num), den) { }

        public Fraction(Number num, int den)
                : this(num, new Number(den)) { }

        public Fraction(Irrational num, Number den)
                : this(new Number(num), den) { }

        public Fraction(Number num, Irrational den)
                : this(num, new Number(den)) { }


        private void Rationalise()
        {
            if (Denominator != 1)
            {
                if (Denominator.Integer == 0 && Denominator.Irrationals.Count == 1)
                {
                    Numerator *= Denominator.Irrationals[0];
                    Denominator *= Denominator.Irrationals[0];
                }
                else if (Denominator.Integer != 0 && Denominator.Irrationals.Count == 1)
                {
                    Number n = new Number(Denominator.Integer, -Denominator.Irrationals[0]);
                    Numerator *= n;
                    Denominator *= n;
                }
                else if (Denominator.Integer == 0 && Denominator.Irrationals.Count == 2)
                {
                    Number n = new Number(new List<Irrational> { Denominator.Irrationals[0], -Denominator.Irrationals[1] });
                    Numerator *= n;
                    Denominator *= n;
                }
            }
        }
        private double ToDouble() => Numerator.ToDouble() / Numerator.ToDouble();

        private void Reduce()
        {
            int d = GCD();
            if (d != 1)
            {
                Numerator.Integer /= d;
                Denominator.Integer /= d;
                for (int i = 0; i < Numerator.Irrationals.Count; i++)
                {
                    Numerator.Irrationals[i].Multiplier /= d;
                }
                for (int i = 0; i < Denominator.Irrationals.Count; i++)
                {
                    Denominator.Irrationals[i].Multiplier /= d;
                }
            }
        }
        private int GCD()
        {
            int dn = Numerator.GCD();
            int dd = Denominator.GCD();
            if (dn == 0 || dd == 0)
            {
                return dd + dn;
            }
            if (dn == dd)
            {
                return dd;
            }
            if (dd == 1 || dn == 1)
            {
                return 1;
            }
            int a = Math.Max(dn, dd);
            int b = Math.Min(dn, dd);
            int r = a % b;
            while (r != 0)
            {
                a = b;
                b = r;
                r = a % b;
            }
            return b;
        }
        public static List<Fraction> Unique(List<Fraction> fracs)
        {
            for (int i = 0; i < fracs.Count; i++)
            {
                for (int j = i + 1; j < fracs.Count; j++) 
                {
                    if (fracs[i] == fracs[j])
                    {
                        fracs.RemoveAt(j);
                        j--;
                    }
                }
            }
            return fracs;
        }
        public Fraction Sqrt()
        {
            if (Numerator.Irrationals.Count == 0 && Denominator.Irrationals.Count == 0 && Numerator.Integer >= 0 && Denominator.Integer >= 0)
            {
                Irrational n = new Irrational(Numerator.Integer);
                Irrational d = new Irrational(Denominator.Integer);
                return new Fraction(n, d);
            }
            return new Fraction(-1);
        }
        public override string ToString()
        {
            if (Numerator == 0)
            {
                return "0";
            }
            if (Numerator == Denominator)
            {
                return "1";
            }
            if (Denominator == 1)
            {
                return Numerator.ToString();
            }
            if (Denominator.Irrationals.Count == 0 || (Denominator.Integer == 0 && Denominator.Irrationals.Count == 1))
            {
                if (Numerator.Irrationals.Count == 0)
                {
                    return $"{Numerator}/{Denominator}";
                }
                if (Numerator.Integer == 0 && Numerator.Irrationals.Count == 1)
                {
                    return $"{Numerator}/{Denominator}";
                }
                return $"({Numerator})/{Denominator}";
            }
            if ((Numerator.Integer == 0 && Numerator.Irrationals.Count == 1) || (Numerator.Integer != 0 && Numerator.Irrationals.Count == 0))
            {
                return $"{Numerator}/({Denominator})";
            }
            return $"({Numerator})/({Denominator})";
        }

        public static Fraction operator +(Fraction f) => f;
        public static Fraction operator -(Fraction f) => new Fraction(-f.Numerator, f.Denominator);

        public static Fraction operator +(Fraction f, Fraction r) => new Fraction(f.Numerator * r.Denominator + r.Numerator * f.Denominator, f.Denominator * r.Denominator);
        public static Fraction operator -(Fraction f, Fraction r) => f + (-r);

        public static Fraction operator +(int k, Fraction f) => new Fraction(k) + f;
        public static Fraction operator -(int k, Fraction f) => k + (-f);

        public static Fraction operator +(Fraction f, int k) => k + f;
        public static Fraction operator -(Fraction f, int k) => f + (-k);

        public static Fraction operator +(Fraction f, Irrational i) => f + new Fraction(i);
        public static Fraction operator -(Fraction f, Irrational i) => f + (-i);

        public static Fraction operator +(Irrational i, Fraction f) => f + i;
        public static Fraction operator -(Irrational i, Fraction f) => i + (-f);

        public static Fraction operator +(Fraction f, Number n) => f + new Fraction(n);
        public static Fraction operator -(Fraction f, Number n) => f + (-n);

        public static Fraction operator +(Number n, Fraction f) => f + n;
        public static Fraction operator -(Number n, Fraction f) => n + (-f);

        public static Fraction operator *(Fraction f, Fraction r) => new Fraction(f.Numerator * r.Numerator, f.Denominator * r.Denominator);

        public static Fraction operator *(Fraction f, int k) => f * new Fraction(k);
        public static Fraction operator *(int k, Fraction f) => f * k;

        public static Fraction operator *(Fraction f, Irrational i) => f * new Fraction(i);
        public static Fraction operator *(Irrational i, Fraction f) => f * i;

        public static Fraction operator *(Fraction f, Number n) => f * new Fraction(n);
        public static Fraction operator *(Number n, Fraction f) => f * n;

        public static Fraction operator /(Fraction f, Fraction r) => new Fraction(f.Numerator * r.Denominator, r.Numerator * f.Denominator);

        public static Fraction operator /(Fraction f, int k) => f / new Fraction(k);
        public static Fraction operator /(int k, Fraction f) => new Fraction(k) / f;

        public static Fraction operator /(Fraction f, Irrational i) => f / new Fraction(i);
        public static Fraction operator /(Irrational i, Fraction f) => new Fraction(i) / f;

        public static Fraction operator /(Fraction f, Number n) => f / new Fraction(n);
        public static Fraction operator /(Number n, Fraction f) => new Fraction(n) / f;

        public static bool operator >(Fraction f, Fraction r) => f.ToDouble() > r.ToDouble();
        public static bool operator <(Fraction f, Fraction r) => f.ToDouble() < r.ToDouble();

        public static bool operator >(Fraction f, int k) => f > new Fraction(k);
        public static bool operator <(Fraction f, int k) => f < new Fraction(k);

        public static bool operator >(int k, Fraction f) => f < new Fraction(k);
        public static bool operator <(int k, Fraction f) => f > new Fraction(k);

        public static bool operator >(Irrational i, Fraction f) => f < new Fraction(i);
        public static bool operator <(Irrational i, Fraction f) => f > new Fraction(i);

        public static bool operator >(Fraction f, Irrational i) => f > new Fraction(i);
        public static bool operator <(Fraction f, Irrational i) => f < new Fraction(i);

        public static bool operator >(Fraction f, Number n) => f > new Fraction(n);
        public static bool operator <(Fraction f, Number n) => f < new Fraction(n);

        public static bool operator >(Number n, Fraction f) => f < new Fraction(n);
        public static bool operator <(Number n, Fraction f) => f > new Fraction(n);

        public static bool operator ==(Fraction f, Fraction r) => f.Numerator == r.Numerator && f.Denominator == r.Denominator;
        public static bool operator !=(Fraction f, Fraction r) => f.Numerator != r.Numerator || f.Denominator != r.Denominator;

        public static bool operator ==(Fraction f, int k) => f == new Fraction(k);
        public static bool operator !=(Fraction f, int k) => f != new Fraction(k);

        public static bool operator ==(int k, Fraction f) => f == k;
        public static bool operator !=(int k, Fraction f) => f != k;

        public static bool operator ==(Fraction f, Irrational i) => f == new Fraction(i);
        public static bool operator !=(Fraction f, Irrational i) => f != new Fraction(i);

        public static bool operator ==(Irrational i, Fraction f) => f == new Fraction(i);
        public static bool operator !=(Irrational i, Fraction f) => f != new Fraction(i);

        public static bool operator ==(Number n, Fraction f) => f == new Fraction(n);
        public static bool operator !=(Number n, Fraction f) => f != new Fraction(n);

        public static bool operator ==(Fraction f, Number n) => f == new Fraction(n);
        public static bool operator !=(Fraction f, Number n) => f != new Fraction(n);

        public static bool operator >=(Fraction f, Fraction r) => f.ToDouble() >= r.ToDouble();
        public static bool operator <=(Fraction f, Fraction r) => f.ToDouble() <= r.ToDouble();

        public static bool operator >=(Fraction f, int k) => f.ToDouble() >= k;
        public static bool operator <=(Fraction f, int k) => f.ToDouble() <= k;

        public static bool operator >=(int k, Fraction f) => f.ToDouble() <= k;
        public static bool operator <=(int k, Fraction f) => f.ToDouble() >= k;

        public static bool operator >=(Fraction f, Number n) => f.ToDouble() >= n.ToDouble();
        public static bool operator <=(Fraction f, Number n) => f.ToDouble() <= n.ToDouble();

        public static bool operator >=(Number n, Fraction f) => f.ToDouble() <= n.ToDouble();
        public static bool operator <=(Number n, Fraction f) => f.ToDouble() >= n.ToDouble();

        public static bool operator >=(Fraction f, Irrational i) => f.ToDouble() >= i.ToDouble();
        public static bool operator <=(Fraction f, Irrational i) => f.ToDouble() <= i.ToDouble();

        public static bool operator >=(Irrational i, Fraction f) => f.ToDouble() >= i.ToDouble();
        public static bool operator <=(Irrational i, Fraction f) => f.ToDouble() <= i.ToDouble();
    }
}