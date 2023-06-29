using System;
using System.Collections.Generic;
using System.Linq;

namespace RealNumbers
{
    public class Number
    {
        public int Integer { get; set; }
        public List<Irrational> Irrationals { get; set; }

        public Number(int integer, List<Irrational> irrationals)
        {
            Integer = integer;
            Irrationals = irrationals;
            Actions();
        }

        public Number(int integer)
                : this(integer, new List<Irrational>()) { }

        public Number(Irrational irrational)
                : this(0, new List<Irrational>().Append(irrational).ToList()) { }

        public Number(List<int> irrationals)
                : this(0, irrationals) { }

        public Number(int integer, int radicant)
                : this(integer, new Irrational(radicant)) { }

        public Number(List<Irrational> irrationals)
                : this(0, irrationals) { }

        public Number(int integer, Irrational irrational)
                : this(integer, new List<Irrational>().Append(irrational).ToList()) { }

        public Number(int integer, List<int> irrationals)
                : this(integer, irrationals.Select(irr => new Irrational(irr)).ToList()) { }

        private List<Irrational> AllIrational() => Irrationals.Append(new Irrational(Integer, 1)).ToList();

        private void Actions()
        {
            for (int i = 0; i < Irrationals.Count; i++)
            {
                Irrationals[i].UnderRadical();
                if (Irrationals[i].CheckIfSquare())
                {
                    if (Irrationals[i].Multiplier < 0)
                        Integer -= (int)Math.Sqrt(Irrationals[i].UnderRadical());
                    else
                        Integer += (int)Math.Sqrt(Irrationals[i].UnderRadical());
                    Irrationals.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < Irrationals.Count - 1; i++)
            {
                for (int j = i + 1; j < Irrationals.Count; j++)
                {
                    if (Irrationals[i].Radicant == Irrationals[j].Radicant)
                    {
                        Irrationals[i].Multiplier += Irrationals[j].Multiplier;
                        Irrationals.RemoveAt(j);
                        j--;
                    }
                }
            }
            for (int i = 0; i < Irrationals.Count; i++)
            {
                if (Irrationals[i] == 0)
                {
                    Irrationals.RemoveAt(i);
                    i--;
                }
            }
        }
        public override string ToString()
        {
            if (Irrationals.Count == 0 && Integer == 0)
            {
                return "0";
            }
            if (Irrationals.Count == 0)
            {
                return Integer.ToString();
            }
            string s = "";
            if (Integer == 0)
            {
                if (Irrationals[0] != 0)
                {
                    s += Irrationals[0].ToString();
                }
                for (int i = 1; i < Irrationals.Count; i++)
                {
                    if (Irrationals[i] > 0)
                    {
                        s += $"+{Irrationals[i].ToString()}";
                    }
                    else if (Irrationals[i] < 0)
                    {
                        s += Irrationals[i].ToString();
                    }
                }
                return s;
            }
            for (int i = 0; i < Irrationals.Count; i++)
            {
                if (Irrationals[i] > 0)
                {
                    s += $"+{Irrationals[i].ToString()}";
                }
                else if (Irrationals[i] < 0)
                {
                    s += Irrationals[i].ToString();
                }
            }
            return $"{Integer}{s}";
        }
        public int GCD()
        {
            if (Irrationals.Count == 0)
            {
                return Math.Abs(Integer);
            }
            int x = Irrational.GCD(Irrationals);
            int y = Math.Abs(Integer);
            if (x == y)
            {
                return x;
            }
            if (y == 0 || x == 0)
            {
                return x + y;
            }
            if (x == 1 && y == 1)
            {
                return 1;
            }
            int a = Math.Max(x, y);
            int b = Math.Min(x, y);
            int r = a % b;
            while (r != 0)
            {
                a = b;
                b = r;
                r = a % b;
            }
            return b;
        }
        public double ToDouble()
        {
            double d = Integer;
            for (int i = 0; i < Irrationals.Count; i++)
            {
                if (Irrationals[i].Multiplier < 0)
                {
                    d -= Math.Sqrt(Irrationals[i].UnderRadical());
                }
                else
                {
                    d += Math.Sqrt(Irrationals[i].UnderRadical());
                }
            }
            return d;
        }
        private static bool AreEqual(List<Irrational> n, List<Irrational> m)
        {
            for (int i = 0; i < n.Count; i++)
            {
                if (!m.Contains(n[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public static Number operator +(Number n) => n;
        public static Number operator -(Number n) => new Number(-n.Integer, n.Irrationals.Select(x => -x).ToList());

        public static Number operator +(Number n, Number m) => new Number(n.Integer + m.Integer, n.Irrationals.Concat(m.Irrationals).ToList());
        public static Number operator -(Number n, Number m) => n + (-m);

        public static Number operator +(Number n, int k) => n + new Number(k);
        public static Number operator -(Number n, int k) => n + (-k);

        public static Number operator +(int k, Number n) => n + k;
        public static Number operator -(int k, Number n) => k + (-n);

        public static Number operator +(Irrational x, Number n) => n + new Number(x);
        public static Number operator -(Irrational x, Number n) => x + (-n);

        public static Number operator +(Number n, Irrational x) => x + n;
        public static Number operator -(Number n, Irrational x) => n + (-x);

        public static Number operator *(Number n, int k) => new Number(n.Integer * k, n.Irrationals.Select(x => k * x).ToList());
        public static Number operator *(int k, Number n) => new Number(n.Integer * k, n.Irrationals.Select(x => k * x).ToList());

        public static Number operator *(Number n, Irrational x) => new Number(n.AllIrational().Select(t => t * x).ToList());
        public static Number operator *(Irrational x, Number n) => new Number(n.AllIrational().Select(t => t * x).ToList());

        public static Number operator *(Number n, Number m)
        {
            List<Irrational> iN = n.AllIrational();
            List<Irrational> iM = m.AllIrational();
            List<Irrational> irrationals = new List<Irrational>();
            for (int i = 0; i < iN.Count; i++)
            {
                for (int j = 0; j < iM.Count; j++)
                {
                    if (iN[i] * iM[j] != 0)
                    {
                        irrationals.Add(iN[i] * iM[j]);
                    }
                }
            }
            return new Number(irrationals);
        }

        public static Fraction operator /(Number n, Number m) => new Fraction(n, m);

        public static Fraction operator /(Number n, int k) => n / new Number(k);
        public static Fraction operator /(int k, Number n) => new Number(k) / n;

        public static Fraction operator /(Number n, Irrational i) => n / new Number(i);
        public static Fraction operator /(Irrational i, Number n) => new Number(i) / n;

        public static bool operator >(Number n, int k) => (n.Integer > k && n.Irrationals.Count == 0) || (n.Irrationals.Count != 0 && n.ToDouble() > k);
        public static bool operator <(Number n, int k) => !(n > k);

        public static bool operator >(int k, Number n) => n.ToDouble() < k;
        public static bool operator <(int k, Number n) => n.ToDouble() > k;

        public static bool operator >(Number n, Number m) => n.ToDouble() > m.ToDouble();
        public static bool operator <(Number n, Number m) => n.ToDouble() < m.ToDouble();

        public static bool operator ==(Number n, int k) => n.Integer == k && n.Irrationals.Count == 0;
        public static bool operator !=(Number n, int k) => n.Integer != k || n.Irrationals.Count != 0;

        public static bool operator ==(int k, Number n) => n.Integer == k && n.Irrationals.Count == 0;
        public static bool operator !=(int k, Number n) => n.Integer != k || n.Irrationals.Count != 0;

        public static bool operator ==(Number n, Number m) => n.Integer == m.Integer && AreEqual(n.Irrationals,m.Irrationals);
        public static bool operator !=(Number n, Number m) => n.Integer != m.Integer || !AreEqual(n.Irrationals, m.Irrationals);

        public static bool operator >=(Number n, int k) => n.ToDouble() >= k;
        public static bool operator <=(Number n, int k) => n.ToDouble() <= k;

        public static bool operator >=(int k, Number n) => n.ToDouble() <= k;
        public static bool operator <=(int k, Number n) => n.ToDouble() >= k;

        public static bool operator >=(Number n, Number m) => n.ToDouble() >= m.ToDouble();
        public static bool operator <=(Number n, Number m) => n.ToDouble() <= m.ToDouble();
    }
}