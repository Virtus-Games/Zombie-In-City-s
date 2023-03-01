using System;
using UnityEngine;
using UnityEngine.Playables;

namespace CORE
{

    public class TeaserController : MonoBehaviour
    {

        public PlayableDirector playableDirector;
        private void OnEnable()
        {
            LevelManager.OnLevelLoaded += OnLevelLoadeded;

        }

        private void OnLevelLoadeded(bool arg0)
        {
            if (arg0 && playableDirector != null)
            {
                GameManager.Instance.UpdateGameState(GAMESTATE.TEASER);
                playableDirector.Play();
                playableDirector.paused += OnPlayableDirectorPaused;
            }
        }

        private void OnPlayableDirectorPaused(PlayableDirector obj)
        {
            playableDirector.Stop();
            GameManager.Instance.UpdateGameState(GAMESTATE.PLAY);
        }
    }
}