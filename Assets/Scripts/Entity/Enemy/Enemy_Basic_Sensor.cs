using UnityEngine;
using System;

namespace Game.Entity.Enemy.StateMachine.Sensor
{
    public class Enemy_Basic_Sensor : MonoBehaviour
    {
        public event Action OnDetectPlayer;
        public event Action OnStartChasePlayer;
        public event Action OnLostPlayer;
        public event Action OnPlayerStayInAttackArea;

        Enemy_Basic_StateMachine stateMachine;

        void Start()
        {
            stateMachine = GetComponent<Enemy_Basic_StateMachine>();
        }

        void Update()
        {
            var _dist = (stateMachine.playerMove.transform.position - transform.position).magnitude;

            foreach (var mod in stateMachine.infoAsset.attackData.attackModules)
            {
                if (_dist <= stateMachine.infoAsset.detectRadius)
                {
                    OnDetectPlayer?.Invoke();
                }
                else
                {
                    OnLostPlayer?.Invoke();
                }
                if (_dist <= mod.enemyPart.attackDistance)
                {
                    OnPlayerStayInAttackArea?.Invoke();
                }
            }
        }
    }
}