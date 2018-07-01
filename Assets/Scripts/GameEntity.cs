using UnityEngine;
using UnityEditor;

namespace forth
{
    public abstract class GameEntity
    {
        public bool Is(System.Type entity)
        {
            return this.GetType() == entity;
        }

        public StarSystem ToStarSystem()
        {
            return this.GetType() == typeof(StarSystem) ? (StarSystem)this : null;
        }
    }
}