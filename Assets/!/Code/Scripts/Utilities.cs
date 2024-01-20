using AxisNS;
using UnityEngine;

namespace UtilityNS
{
    public class Utilities
    {
        public static Vector3 DefineDirection(Axis moveDirection)
        {
            switch (moveDirection)
            {
                case Axis.X:
                    return new Vector3(1f, 0f, 0f);
                case Axis.Y:
                    return new Vector3(0f, 1f, 0f);
                case Axis.Z:
                    return new Vector3(0f, 0f, 1f);
            }
            return new Vector3(0f, 0f, 0f);
        }
        public static T GetComponentFromGameObject<T>(GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component != null)
                return component;
            else
                return gameObject.GetComponentInChildren<T>();
        }
    }
}
