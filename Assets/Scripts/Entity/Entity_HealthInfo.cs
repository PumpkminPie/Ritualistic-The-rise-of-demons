using UnityEngine;

namespace Game.Character.Health
{
    [CreateAssetMenu(fileName = "Health info Asset", menuName = "Health System/new Health info Asset", order = 1)]
    public class Entity_HealthInfo : ScriptableObject
    {
        public float health = 1;
    }
}