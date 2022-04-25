using System;
using DeliverableIA.Core.Player;
using DeliverableIA.Core.ScriptableObjects;
using UnityEngine;

namespace DeliverableIA.Core
{
	public class Entity : MonoBehaviour
	{
		[Header("Stats")]
		[SerializeField] private float maxHealth;
		private float _currentHealth;

		[Header("Weapon")]
		[SerializeField] protected Weapon weapon;
		[SerializeField] protected Transform pfBullet, bulletSpawn;
		protected int CurrentAmmo, TotalAmmo;


		private void Start()
		{
			CurrentAmmo = weapon.magazineSize;
			TotalAmmo = weapon.maxAmmo - weapon.magazineSize;
			_currentHealth = maxHealth;
		}

		protected virtual void Die()
		{
			Destroy(gameObject);
		}

		public void TakeDamage(float damage)
		{
			_currentHealth -= damage;
			if (_currentHealth <= 0)
			{
				Die();
			}
		}

		public void Shoot(Vector3 targetPosition)
		{
			CurrentAmmo--;
			var position = bulletSpawn.position;
			var aimDir = (targetPosition - position).normalized;
			var bullet = Instantiate(pfBullet, position, Quaternion.LookRotation(aimDir, Vector3.up));
			bullet.GetComponent<BulletProjectile>().Init(weapon.bulletSpeed, weapon.bulletLifeTime, weapon.damage);
		}
	}
}
