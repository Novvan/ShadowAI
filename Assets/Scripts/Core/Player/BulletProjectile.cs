using System;
using UnityEngine;

namespace DeliverableIA.Core.Player
{
	public class BulletProjectile : MonoBehaviour
	{
		#region Variables

		private Rigidbody _rigidbody;
		[Header("Projectile")]
		[SerializeField] private float speed;

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
			_rigidbody.velocity = transform.forward * speed;
		}

		private void OnTriggerEnter(Collider other)
		{
			if(other.CompareTag("Enemy"))
			{
				Transform vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
				Destroy(vfx.gameObject, 1f);
			}
			else
			{
				Transform vfx = Instantiate(missVFX, transform.position, Quaternion.identity);
				Destroy(vfx.gameObject, 1f);
			}

			Destroy(gameObject);
		}

		#endregion
	}
}
