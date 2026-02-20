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
    public struct AttackModules
    {
        [HideInInspector]
        public string inspectorName;

        public string attackName;

        [SerializeReference]
        public Entity_AttackModule module;

        [Header("Inputs")]
        public InputActionReference inputBind;

        [Header("Timings")]
        public float startDelay;
        public float duration;
        public float endDelay;

        [Header("Others")]
        public DireType dirType;
        public bool changeDireOnAir;
    }

    [CreateAssetMenu(menuName = "Entitys/Combat/AttackData")]
    public class Entity_AttackData : ScriptableObject
    {
        public List<AttackModules> attackModules;

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


