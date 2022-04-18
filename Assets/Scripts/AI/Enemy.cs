using DeliverableIA.Core.ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace DeliverableIA.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float speed = 1f;
        [SerializeField] private int maxAmmo = 10;
        [SerializeField] private Weapon weapon;
        public Transform[] waypoints;
        private int _currentAmmo;
        private NavMeshAgent _navMeshAgent;

        public Weapon Weapon => weapon;

        public NavMeshAgent MeshAgent => _navMeshAgent;

        public int CurrentAmmo => _currentAmmo;

        #endregion

        #region Unity Methods

        private void Start()
        {
            _currentAmmo = maxAmmo;
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
        }

        #endregion
    }
}
