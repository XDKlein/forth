using UnityEngine;
using UnityEditor;

namespace forth
{
    public class ObjectData : MonoBehaviour
    {
        private GameEntity storedData;

        public GameEntity StoredData
        {
            get
            {
                return storedData;
            }

            set
            {
                storedData = value;
            }
        }
    }
}