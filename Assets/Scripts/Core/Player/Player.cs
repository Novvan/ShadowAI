using System;
using UnityEngine;

namespace DeliverableIA.Core.Player
{
	public class Player : Entity
	{
		public static Action OnPlayerDeath;

		protected override void Die()
		{
			OnPlayerDeath?.Invoke();
			base.Die();
		}
	}
}
