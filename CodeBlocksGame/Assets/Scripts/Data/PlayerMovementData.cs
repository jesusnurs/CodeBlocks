using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Data/PlayerMovementData")]
    public class PlayerMovementData : ScriptableObject
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float dashForce;
        
        public float MovementSpeed  => movementSpeed;
        public float DashForce => dashForce;
    }
}
