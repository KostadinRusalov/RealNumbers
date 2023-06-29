using System;
using System.Collections.Generic;
using System.Linq;

namespace RealNumbers
{
    public class Irrational
    {
        public int Multiplier { get; set; }
        public int Radicant { get; set; }

        public Irrational(int multiplier, int radicant)
        {
            if (radicant < 0)
            {
                throw new ArgumentException("The value under √ can't be negative!");
            }
            if (radicant == 0)
            {
                Multiplier = 0;
                Radicant = 0;
            }
            else
            {
                Multiplier = multiplier;
                Radicant = radicant;
                InFrontOfRadical();
            }
        }
        public Irrational(int radicant)
                : this(1, radicant) { }

        public double ToDouble() => Math.Sqrt(UnderRadical());

        private Dictionary<int, int> PrimeFactorization(int x)
        {
            List<int> allPrimeDivisors = new List<int>();
            int del = x;
            for (int i = 2; i <= del; i++)
            {
                if (del % i == 0)
                {
                    allPrimeDivisors.Add(i);
                    del /= i;
                    i--;
                }
            }
            List<int> primeDivisors = allPrimeDivisors.Distinct().ToList();
            Dictionary<int, int> primeFactorization = new Dictionary<int, int>();
            foreach (int prime in primeDivisors)
            {
                primeFactorization.Add(prime, 0);
            }
            for (int i = 0; i < primeDivisors.Count; i++)
            {
                for (int j = 0; j < allPrimeDivisors.Count; j++)
                {
                    if (primeDivisors[i] == allPrimeDivisors[j])
                    {
                        primeFactorization[primeDivisors[i]]++;
                    }
                }
            }
            return primeFactorization;
        } 
        private bool CheckIfSquare(int x)
        {
            double sqrt = Math.Sqrt(x);
            for (int i = (int)Math.Round(sqrt); i > 0; i--)
            {
                if (sqrt.CompareTo(i) == 0)
                {
                    return true;
                }
            }
            return false;
        }
        private void InFrontOfRadical()
        {
            if (Radicant != 0)
            {
                if (CheckIfSquare(Radicant))
                {
                    Multiplier *= (int)Math.Sqrt(Radicant);
                    Radicant = 1;
                }
                if (Radicant != 1)
                {
                    Dictionary<int, int> primeFactorization = PrimeFactorization(Radicant);
                    foreach (int prime in primeFactorization.Keys)
                    {
                        Multiplier *= (int)Math.Pow(prime, (primeFactorization[prime] - (primeFactorization[prime] % 2)) / 2);
                        Radicant /= (int)Math.Pow(prime, primeFactorization[prime] - (primeFactorization[prime] % 2));
                    }
                }
            }
        }

        public int UnderRadical() => (int)Math.Pow(Multiplier, 2) * Radicant;
        public static int GCD(List<Irrational> irrationals)
        {
            List<Irrational> gcd = new List<Irrational>();
            foreach (Irrational item in irrationals)
            {
                gcd.Add(item);
            }
            if (gcd.Count == 1)
            {
                return Math.Abs(gcd[0].Multiplier);
            }
            int d = new int();
            for (int i = 0; i < gcd.Count - 1; i++)
            {
                d = GCD(gcd[i], gcd[i + 1]);
                gcd[i + 1] = new Irrational(d, 1);
                gcd.RemoveAt(i);
                i--;
            }
            return d;
        }
        public static int GCD(Irrational a, Irrational b)
        {
            if (a.Multiplier == b.Multiplier)
            {
                return Math.Abs(a.Multiplier);
            }
            if (a.Multiplier == 0 || b.Multiplier == 0)
            {
                return Math.Abs(a.Multiplier) + Math.Abs(b.Multiplier);
            }
            if (a.Multiplier == 1 && b.Multiplier == 1)
            {
                return 1;
            }
            int x = Math.Max(Math.Abs(a.Multiplier), Math.Abs(b.Multiplier));
            int y = Math.Min(Math.Abs(a.Multiplier), Math.Abs(b.Multiplier));
            int r = x % y;
            while (r != 0)
            {
                x = y;
                y = r;
                r = x % y;
            }
            return y;
        }
        public override string ToString()
        {
            if (Radicant == 0 || Multiplier == 0)
            {
                return "0";
            }
            if (Radicant == 1)
            {
                return $"{Multiplier}";
            }
            if (Multiplier == 1)
            {
                return $"√{Radicant}";
            }
            if (Multiplier == -1)
            {
                return $"-√{Radicant}";
            }
            return $"{Multiplier}√{Radicant}";
        }
        public bool CheckIfSquare()
        {
            int x = UnderRadical();
            double sqrt = Math.Sqrt(x);
            for (int i = (int)Math.Round(sqrt); i > 0; i--)
            {
                if (sqrt.CompareTo(i) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static Irrational operator +(Irrational x) => x;
        public static Irrational operator -(Irrational x) => new Irrational(-x.Multiplier, x.Radicant);

        public static Number operator +(Irrational x, int b) => new Number(b, x);
        public static Number operator -(Irrational x, int b) => x + (-b);

        public static Number operator +(int b, Irrational x) => new Number(b, x);
        public static Number operator -(int b, Irrational x) => b + (-x);

        public static Number operator +(Irrational x, Irrational y) => new Number(new List<Irrational> { x, y });
        public static Number operator -(Irrational x, Irrational y) => x + (-y);

        public static Irrational operator *(int x, Irrational y) => new Irrational(x * y.Multiplier, y.Radicant);
        public static Irrational operator *(Irrational x, int y) => y * x;

        public static Irrational operator *(Irrational x, Irrational y) => new Irrational(x.Multiplier * y.Multiplier, x.Radicant * y.Radicant);

        public static Fraction operator /(Irrational i, Irrational j) => new Fraction(i) / new Fraction(j);

        public static Fraction operator /(Irrational i, int k) => i / new Fraction(k);
        public static Fraction operator /(int k, Irrational i) => new Fraction(k) / i;

        public static Fraction operator /(Irrational i, Number n) => i / new Fraction(n);
        public static Fraction operator /(Number n, Irrational i) => new Fraction(n) / i;

        public static bool operator <(Irrational x, int b) => x.Multiplier * Math.Sqrt(x.Radicant) < b;
        public static bool operator >(Irrational x, int b) => x.Multiplier * Math.Sqrt(x.Radicant) > b;

        public static bool operator <(int b, Irrational x) => b < x.Multiplier * Math.Sqrt(x.Radicant);
        public static bool operator >(int b, Irrational x) => b > x.Multiplier * Math.Sqrt(x.Radicant);

        public static bool operator <(Irrational x, Irrational y) => x.UnderRadical() < y.UnderRadical();
        public static bool operator >(Irrational x, Irrational y) => x.UnderRadical() > y.UnderRadical();

        public static bool operator >(Number n, Irrational i) => n.ToDouble() > Math.Sqrt(i.UnderRadical());
        public static bool operator <(Number n, Irrational i) => n.ToDouble() < Math.Sqrt(i.UnderRadical());

        public static bool operator >(Irrational i, Number n) => n.ToDouble() < Math.Sqrt(i.UnderRadical());
        public static bool operator <(Irrational i, Number n) => n.ToDouble() > Math.Sqrt(i.UnderRadical());

        public static bool operator ==(Irrational x, int b) => x == new Irrational(b, 1);
        public static bool operator !=(Irrational x, int b) => x != new Irrational(b, 1);

        public static bool operator ==(int b, Irrational x) => x == b;
        public static bool operator !=(int b, Irrational x) => x != b;

        public static bool operator ==(Irrational x, Irrational y) => x.Multiplier == y.Multiplier && x.Radicant == y.Radicant;
        public static bool operator !=(Irrational x, Irrational y) => x.Multiplier != y.Multiplier || x.Radicant != y.Radicant;

        public static bool operator ==(Number n, Irrational i)
              => (n.Integer == i.Multiplier && i.Radicant == 1 && n.Irrationals.Count == 0)
                  || (n.Integer == 0 && n.Irrationals.Count == 1 && n.Irrationals[0] == i);
        public static bool operator !=(Number n, Irrational i)
              => n.Integer != i.Multiplier || i.Radicant != 1 || n.Irrationals.Count != 0
                  && (n.Integer != 0 || n.Irrationals.Count != 1 || n.Irrationals[0] != i);

        public static bool operator >=(Irrational x, int b) => x.UnderRadical() >= b;
        public static bool operator <=(Irrational x, int b) => x.UnderRadical() <= b;

        public static bool operator >=(int b, Irrational x) => x.UnderRadical() >= b;
        public static bool operator <=(int b, Irrational x) => x.UnderRadical() <= b;

        public static bool operator >=(Irrational x, Irrational y) => x.UnderRadical() >= y.UnderRadical();
        public static bool operator <=(Irrational x, Irrational y) => x.UnderRadical() <= y.UnderRadical();

        public static bool operator >=(Number n, Irrational i) => n.ToDouble() >= Math.Sqrt(i.UnderRadical());
        public static bool operator <=(Number n, Irrational i) => n.ToDouble() <= Math.Sqrt(i.UnderRadical());

        public static bool operator >=(Irrational i, Number n) => n.ToDouble() <= Math.Sqrt(i.UnderRadical());
        public static bool operator <=(Irrational i, Number n) => n.ToDouble() >= Math.Sqrt(i.UnderRadical());
    }
}