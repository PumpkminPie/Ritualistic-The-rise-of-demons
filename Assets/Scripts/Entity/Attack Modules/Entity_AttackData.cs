using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Entity.Attack
{
    public enum DireType
    { 
        Mouse,
        Entity,
        ToPlayer
    }

    [Serializable]
    public struct EnemyPart
    {
        public float attackDistance;
        public Vector2 attackSize;
    }

    [Serializable]
    public struct AttackModule
    {
        [HideInInspector]
        public string inspectorName;

        public string attackName;
        public float damage;

        [SerializeReference]
        public Entity_AttackModule module;

        [Header("Inputs (Ignore if is player attack!)")]
        public InputActionReference inputBind;

        [Header("Timings")]
        public float startDelay;
        public float duration;
        public float endDelay;
        public float waitTimeAfterAttack;

        [Header("Others")]
        [Tooltip("If 'Mouse' => go to mouse dire.\nIf 'Entity' => go to face dire.\nIf 'ToPlayer' => go to player pos.")]
        public DireType dirType;
        //public bool changeDireOnAir;

        [Space]
        [Header("Enemy part (ignore if is player attack!)")]
        public EnemyPart enemyPart;
    }

    [CreateAssetMenu(menuName = "Entitys/Combat/AttackData")]
    public class Entity_AttackData : ScriptableObject
    {
        public List<AttackModule> attackModules;

#if UNITY_EDITOR

        void OnValidate()
        {
            for (int i = 0; i < attackModules.Count; i++)
            {
                var _m = attackModules[i];
                _m.inspectorName = attackModules[i].attackName;

                attackModules[i] = _m;
            }
        }
#endif
    }
}


