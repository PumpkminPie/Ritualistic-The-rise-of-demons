using UnityEngine;
using System;
using Game.Manegement;

namespace Game.Entity.Enemy.StateMachine.Sensor
{
    public class Enemy_Basic_Sensor : MonoBehaviour
    {
        public Action OnDetectPlayer;
        public Action OnStartChasePlayer;
        public Action OnLostPlayer;
        public Action OnPlayerStayInAttackArea;

        [SerializeField] bool playerInAttackArea;
        [SerializeField] bool playerInDetectArea;
        [SerializeField] bool canChase;

        public bool PlayerInAttackArea => playerInAttackArea;
        public bool PlayerInDetectArea => playerInDetectArea;
        public bool CanChase => canChase;

        Enemy_Basic_StateMachine stateMachine;

        void Start()
        {
            stateMachine = GetComponent<Enemy_Basic_StateMachine>();
        }

        void Update()
        {
            var playerMove = GameController.Instance.playerTrans;
            var infoAsset = stateMachine.InfoAsset;

            var _dist = (playerMove.transform.position - transform.position).magnitude;

            foreach (var mod in infoAsset.attackData.attackModules)
            {
                if (_dist <= infoAsset.detectRadius)
                {
                    if (canChase)
                        OnDetectPlayer?.Invoke();

                    playerInDetectArea = true;
                }
                else
                {
                    OnLostPlayer?.Invoke();
                    playerInDetectArea = false;
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

        public void SetCanChase(bool value) => canChase = value;
    }
}