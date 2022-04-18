using UnityEngine;

namespace DeliverableIA.Managers
{
    public class SoundManager : MonoBehaviour
    {
        #region Variables

        #endregion

        #region Unity Methods

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }
}