using UnityEngine;
using System;

namespace Game.Entity.Enemy.StateMachine.Sensor
{
    public class Enemy_Basic_Sensor : MonoBehaviour
    {
        public Action OnDetectPlayer;
        public Action OnStartChasePlayer;
        public Action OnLostPlayer;
        public Action OnPlayerStayInAttackArea;

        [SerializeField] bool playerInAttackArea;

        public bool PlayerInAttackArea => playerInAttackArea;

        Enemy_Basic_StateMachine stateMachine;

        void Start()
        {
            stateMachine = GetComponent<Enemy_Basic_StateMachine>();
        }

        void Update()
        {
            var playerMove = stateMachine.Player;
            var infoAsset = stateMachine.InfoAsset;

            var _dist = (playerMove.transform.position - transform.position).magnitude;

            foreach (var mod in infoAsset.attackData.attackModules)
            {
                if (_dist <= infoAsset.detectRadius)
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
                    playerInAttackArea = true;
                }
                else
                    playerInAttackArea = false;
            }
        }
    }
}