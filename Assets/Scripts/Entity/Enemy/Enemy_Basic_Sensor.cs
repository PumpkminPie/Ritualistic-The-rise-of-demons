using UnityEngine;
using UnityEngine.Events;

namespace Game.Character.Enemy.StateMachine.Sensor
{
    public class Enemy_Basic_Sensor : MonoBehaviour
    {
        public bool canAttack = true;
        public bool canChase = true;

        public UnityEvent OnDetectPlayer;
        public UnityEvent OnStartChasePlayer;
        public UnityEvent OnLostPlayer;
        //public UnityEvent OnLostPlayer;

        Enemy_Basic_StateMachine stateMachine;

        void Start()
        {
            stateMachine = GetComponent<Enemy_Basic_StateMachine>();
        }

        void Update()
        {
            var _dist = (stateMachine.playerMove.transform.position - transform.position).magnitude;

            if (_dist <= stateMachine.infoAsset.detectRadius)
            {
                OnDetectPlayer?.Invoke();
            }
            else
            {
                OnLostPlayer?.Invoke();
            }
        }
    }
}