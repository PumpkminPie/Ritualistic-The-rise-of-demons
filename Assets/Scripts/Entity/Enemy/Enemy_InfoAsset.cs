using Game.Entity.Attack;
using UnityEngine;

namespace Game.Entity.Enemy
{
    [CreateAssetMenu(menuName = "Entitys/Enemy Info/Enemy info Asset", order = 2)]
    public class Enemy_InfoAsset : ScriptableObject
    {
        [Header("Movement")]
        public float maxVel = 1;
        public float acceleration = 10;

        public float detectRadius = 10;

        public Entity_AttackData attackData;
    }
}