using UnityEngine;

namespace Game.Character.Enemy
{
    [CreateAssetMenu(fileName = "Enemy info Asset", menuName = "Enemy Info/new Enemy info Asset", order = 2)]
    public class Enemy_InfoAsset : ScriptableObject
    {
        public float maxVel = 1;
        public float acceleration = 10;

        public float detectRadius = 10;
    }
}