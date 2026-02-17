using Game.Character.Enemy.StateMachine;
using Game.Character.Enemy.StateMachine.States;
using UnityEngine;

namespace Game.Character.Enemy.StateMachine.States
{
    public class Enemy_Basic_Chase : Enemy_Basic_States
    {
        public Enemy_Basic_Chase(Enemy_Basic_StateMachine enemy) : base(enemy) { }

        public override void Enter(Enemy_Basic_StateMachine enemy)
        {

        }

        public override void Execute(Enemy_Basic_StateMachine enemy)
        {
            GoTo(enemy, enemy.playerMove.transform, enemy.infoAsset.maxVel);
        }

        public override void Exit(Enemy_Basic_StateMachine enemy)
        {

        }
    }
}
