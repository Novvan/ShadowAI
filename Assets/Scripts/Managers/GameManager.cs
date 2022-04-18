using System;
using UnityEngine;
using DeliverableIA.Core.Utils;

namespace DeliverableIA.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Variables

        private static GameManager _instance;
        private GameState _state = GameState.Menu;
        
        public static GameManager Instance => _instance;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (Instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            switch (_state)
            {
                case GameState.Menu:
                    break;
                case GameState.Play:
                    Tools.ToggleCursor(false);
                    break;
                case GameState.Pause:
                    break;
                case GameState.Victory:
                    break;
                case GameState.Defeat:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Custom Methods

        public void SetState(GameState state)
        {
            _state = state;
        }

        #endregion
    }

    public enum GameState
    {
        Menu,
        Play,
        Pause,
        Victory,
        Defeat,
    }
}