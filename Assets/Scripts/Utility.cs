using UnityEngine;
using UnityEditor;

namespace forth
{
    public class Utility
    {
        public static bool LineIntesection(Vector2 aStart, Vector2 aEnd, Vector2 bStart, Vector2 bEnd)
        {
            Vector2 a = aEnd - aStart;
            Vector2 b = bStart - bEnd;
            Vector2 c = aStart - bStart;

            float alphaNumerator = b.y * c.x - b.x * c.y;
            float alphaDenominator = a.y * b.x - a.x * b.y;
            float betaNumerator = a.x * c.y - a.y * c.x;
            float betaDenominator = a.y * b.x - a.x * b.y;

            bool isIntersect = true;

            if (alphaDenominator == 0 || betaDenominator == 0)
            {
                isIntersect = false;
            }
            else
            {
                if (alphaDenominator > 0)
                {
                    if (alphaNumerator < 0 || alphaNumerator > alphaDenominator)
                    {
                        isIntersect = false;
                    }
                }
                else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator)
                {
                    isIntersect = false;
                }
                if (isIntersect && betaDenominator > 0)
                {
                    if (betaNumerator < 0 || betaNumerator > betaDenominator)
                    {
                        isIntersect = false;
                    }
                }
                else if (betaNumerator > 0 || betaNumerator < betaDenominator)
                {
                    isIntersect = false;
                }
            }

            return isIntersect;
        }

        public static float PointToLineDistance(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            Vector2 v = lineEnd - lineStart;
            Vector2 w = point - lineStart;
            float c1 = Vector2.Dot(w, v);
            if (c1 <= 0)
                return Vector2.Distance(point, lineStart);
            float c2 = Vector2.Dot(v, v);
            if (c2 <= c1)
                return Vector2.Distance(point, lineEnd);
            float b = c1 / c2;
            Vector2 Pb = lineStart + b * v;
            return Vector2.Distance(point, Pb);
        }
    }
}