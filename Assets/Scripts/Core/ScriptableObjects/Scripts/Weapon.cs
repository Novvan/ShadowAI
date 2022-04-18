using UnityEngine;

namespace DeliverableIA.Core.ScriptableObjects.Scripts
{
    
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Scriptable Objects/Weapon", order = 1)]
    public class Weapon : ScriptableObject
    {
        public float damage;
        public float range;
        public float fireRate;
        public float reloadTime;
        public float bulletSpeed;
        public float bulletLifeTime;
        public Transform weaponPrefab;
    }
}
