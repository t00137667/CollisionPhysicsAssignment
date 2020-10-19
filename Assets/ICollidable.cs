using UnityEngine;

namespace Assets
{
    public interface ICollidable
    {
        void CollidesWith(GameObject target);
    }
}
