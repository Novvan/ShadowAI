using System;
using DeliverableIA.Core.ScriptableObjects;
using DeliverableIA.Core.ScriptableObjects.Scripts;
using UnityEngine;

namespace DeliverableIA.AI
{
	public class Enemy : MonoBehaviour
	{
		#region Variables

		[SerializeField] private float speed = 1f;
		[SerializeField] private int maxAmmo = 10;
		private int _currentAmmo;
		private float _counter;
		[SerializeField] private Weapon _weapon;

		public Weapon Weapon => _weapon;

		#endregion

		#region Unity Methods

		private void Start()
		{
			_currentAmmo = maxAmmo;
		}

		private void Update()
		{
			_counter += Time.deltaTime;
		}

		#endregion

		#region Custom Methods

		public bool IsAmmoEmpty() => _currentAmmo > 0;
		public bool IsLineOfSight() => true;
		public bool IsInAttackRange() => false;
		public bool IdleTimer()
		{
			var cnt = _counter;
			_counter = 0;
			return cnt >= 5;
		}

		#endregion

	}
}
