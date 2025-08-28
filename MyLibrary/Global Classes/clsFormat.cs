using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DVLD.Classes
{
    public class clsFormat
    {
        public static string DateToShort(DateTime Dt1)
        {
            
            return Dt1.ToString("dd/MMM/yyyy");
        }
        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            // تحميل مشغل +
            public static Point operator +(Point a, Point b)
            {
                return new Point { X = a.X + b.X, Y = a.Y + b.Y };
            }

            // تحميل مشغل -
            public static Point operator -(Point a, Point b)
            {
                return new Point { X = a.X - b.X, Y = a.Y - b.Y };
            }

            // تحميل مشغل *
            public static Point operator *(Point a, int scalar)
            {
                return new Point { X = a.X * scalar, Y = a.Y * scalar };
            }

            // تحميل مشغل /
            public static Point operator /(Point a, int scalar)
            {
                if (scalar == 0) throw new DivideByZeroException();
                return new Point { X = a.X / scalar, Y = a.Y / scalar };
            }

            // تحميل مشغل ==
            public static bool operator ==(Point a, Point b)
            {
                return a.X == b.X && a.Y == b.Y;
            }

            // تحميل مشغل !=
            public static bool operator !=(Point a, Point b)
            {
                return !(a == b);
            }

            // تحميل مشغل >
            public static bool operator >(Point a, Point b)
            {
                return a.X > b.X || (a.X == b.X && a.Y > b.Y);
            }

            // تحميل مشغل <
            public static bool operator <(Point a, Point b)
            {
                return a.X < b.X || (a.X == b.X && a.Y < b.Y);
            }

            // تحميل مشغل >=
            public static bool operator >=(Point a, Point b)
            {
                return a > b || a == b;
            }

            // تحميل مشغل <=
            public static bool operator <=(Point a, Point b)
            {
                return a < b || a == b;
            }

            // تحميل مشغل []
            public int this[int index]
            {
                get
                {
                    return index == 0 ? X : Y;
                }
                set
                {
                    if (index == 0) X = value;
                    else if (index == 1) Y = value;
                    else throw new IndexOutOfRangeException("Index must be 0 or 1.");
                }
            }

            // تحميل مشغل ToString لعرض النقطة بشكل مناسب
            public override string ToString()
            {
                return $"({X}, {Y})";
            }

            // تحميل مشغل GetHashCode
            public override int GetHashCode()
            {
                return X ^ Y; // XOR لتوليد قيمة فريدة
            }

            // تحميل مشغل Equals لمقارنة الكائنات
            public override bool Equals(object obj)
            {
                if (obj is Point other)
                    return this == other;
                return false;
            }
        }

        public static string NumberOfCard(string NumCard)
        {
            string NumberCard=null;

            for(int i = 0; i < NumCard.Length; i++)
            {
                NumberCard += NumCard[i];
                if(i+1 % 3 == 0)
                {
                    NumCard += " ";
                }
            }

            return NumberCard;
        }
    }
}
