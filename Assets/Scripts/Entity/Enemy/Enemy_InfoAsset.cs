using UnityEngine;

namespace Game.Entity.Enemy
{
    [CreateAssetMenu(menuName = "Entitys/Enemy Info/Enemy info Asset", order = 2)]
    public class Enemy_InfoAsset : ScriptableObject
    {
        public float maxVel = 1;
        public float acceleration = 10;

        public float detectRadius = 10;
    }
}