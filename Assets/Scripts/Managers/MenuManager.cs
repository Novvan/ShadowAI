using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeliverableIA.Managers
{
	public class MenuManager : MonoBehaviour
	{
		#region Variables

		private GameManager _gm;

		#endregion

		#region Unity Methods

		private void Start()
		{
			_gm = GameManager.Instance;
		}

		#endregion

		#region Custom Methods

		public void StartGame(string playScene)
		{
			SceneManager.LoadScene(playScene);
			_gm.SetState(GameState.Play);
		}

		#endregion
	}
}
