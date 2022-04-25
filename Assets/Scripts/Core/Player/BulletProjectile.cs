using System;
using DeliverableIA.AI;
using DeliverableIA.Core.ScriptableObjects;
using UnityEngine;

namespace DeliverableIA.Core.Player
{
	public class BulletProjectile : MonoBehaviour
	{
		#region Variables

		private Rigidbody _rigidbody;
		private float _speed, _lifeTime, _damage;

		[Header("VFX")]
		[SerializeField] private Transform hitVFX;
		[SerializeField] private Transform missVFX;

		#endregion

		#region Unity Methods

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			_rigidbody.velocity = transform.forward * _speed;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Enemy"))
			{
				var vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
				Destroy(vfx.gameObject, _lifeTime);
				var enemy = other.GetComponent<Enemy>();
				if (enemy != null)
				{
					enemy.TakeDamage(_damage);
				}
			}
			else if (other.CompareTag("Player"))
			{
				var vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
				Destroy(vfx.gameObject, _lifeTime);
				var player = other.GetComponent<Player>();
				if (player != null)
				{
					player.TakeDamage(_damage);
				}
			}
			else
			{
				var vfx = Instantiate(missVFX, transform.position, Quaternion.identity);
				Destroy(vfx.gameObject, _lifeTime);
			}

			Destroy(gameObject);
		}

		#endregion

		#region Methods

		public void Init(float speed, float lifeTime, float damage)
		{
			_speed = speed;
			_lifeTime = lifeTime;
			_damage = damage;
		}

		#endregion
	}
}
