using UnityEngine;
using System.Collections.Generic;

namespace forth
{
    /*public class ClockwiseComparer : IComparer<Border.BorderPoint>
    {
        private Vector2 m_Origin;
        public Vector2 origin { get { return m_Origin; } set { m_Origin = value; } }

        public ClockwiseComparer(Vector2 origin)
        {
            m_Origin = origin;
        }

        public int Compare(Border.BorderPoint first, Border.Border second)
        {
            return IsClockwise(first.coordinates, second.coordinates, m_Origin);
        }

        public static int IsClockwise(Vector2 first, Vector2 second, Vector2 origin)
        {
            if (first == second)
                return 0;

            Vector2 firstOffset = first - origin;
            Vector2 secondOffset = second - origin;

            float angle1 = Mathf.Atan2(firstOffset.x, firstOffset.y);
            float angle2 = Mathf.Atan2(secondOffset.x, secondOffset.y);

            if (angle1 < angle2)
                return -1;

            if (angle1 > angle2)
                return 1;

            return (firstOffset.sqrMagnitude < secondOffset.sqrMagnitude) ? -1 : 1;
        }
    }*/
}