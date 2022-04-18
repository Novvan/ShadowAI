using System;
using UnityEngine;

namespace DeliverableIA.AI
{
	public class Enemy : MonoBehaviour
	{
		#region Variables

		[SerializeField] private float speed = 1f;
		[SerializeField] private int maxAmmo = 10;
		private int _currentAmmo;

		#endregion

		#region Unity Methods

		private void Start()
		{
			_currentAmmo = maxAmmo;
		}

		#endregion

		#region Custom Methods

		public bool IsAmmoEmpty() => _currentAmmo > 0;
		public bool IsLineOfSight() => true;
		public bool IsInAttackRange() => false;

		#endregion

	}
}
