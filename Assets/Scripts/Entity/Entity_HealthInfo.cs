using UnityEngine;

namespace Game.Entity.Health
{
    [CreateAssetMenu(menuName = "Entitys/Health System/Health info Asset", order = 1)]
    public class Entity_HealthInfo : ScriptableObject
    {
        public float health = 1;
    }
}