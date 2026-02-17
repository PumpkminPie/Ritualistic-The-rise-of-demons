using UnityEngine;

namespace Game
{ 
    public class Interfaces
    {
        //namespace 
        // Interface para sistema de life geral (planejar -> ser mais geral, "abrindo portas")
        public interface IHealth
        {
            void Attack();
        }

        // Interface para debugs visuais futuros (!debug!)
        public interface IDebug
        {
            string Info();
        }
    }
}
