using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
     private const string _level = "Level ";
     private const string _currentLevel = "currentLevel";
     private Scene _lastLoadedScene;
     public static event UnityAction<bool> OnLevelLoaded;
     public GameObject LoadingBar;
     [HideInInspector]
     public int currentLevel;


     private void Awake()
     {

          AdmobManager.Instance.InitiliazedAds();

          LevelLoad();

     }

     private void LevelLoad()
     {

          if (!PlayerPrefs.HasKey(_currentLevel))
               PlayerPrefs.SetInt(_currentLevel, 1);
          else
               currentLevel = PlayerPrefs.GetInt(_currentLevel);


          if (currentLevel >= SceneManager.sceneCountInBuildSettings)
          {
               currentLevel = 1;
               PlayerPrefs.SetInt(_currentLevel, currentLevel);
          }

          currentLevel = PlayerPrefs.GetInt(_currentLevel);
          LoadingBar.SetActive(true);

          SceneLoader(currentLevel.ToString());
     }

     public void SetCurrentLevel()
     {
          currentLevel++;

          if (currentLevel >= SceneManager.sceneCountInBuildSettings)
               currentLevel = 1;

          PlayerPrefs.SetInt(_currentLevel, currentLevel);
          SceneLoader(currentLevel.ToString());
     }


     public void SceneLoader(string name) => ChangeScene(name);

     void ChangeScene(string sceneName)
     {
          LoadingBar.SetActive(true);
          StartCoroutine(SceneController(_level + sceneName));
     }

     IEnumerator SceneController(string sceneName)
     {
          OnLevelLoaded?.Invoke(false);

          if (_lastLoadedScene.IsValid())
          {
               SceneManager.UnloadSceneAsync(_lastLoadedScene);
               bool isUnloadScene = false;
               while (!isUnloadScene)
               {
                    isUnloadScene = !_lastLoadedScene.IsValid();
                    yield return new WaitForEndOfFrame();

               }
          }

          SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

          bool isSceneLoaded = false;

          while (!isSceneLoaded)
          {
               _lastLoadedScene = SceneManager.GetSceneByName(sceneName);
               isSceneLoaded = _lastLoadedScene != null && _lastLoadedScene.isLoaded;

               yield return new WaitForEndOfFrame();
          }

          OnLevelLoaded?.Invoke(true);
          LoadingBar.SetActive(false);


     }

     public void NextLevel()
     {
          SetCurrentLevel();
     }

     public void RestartLevel()
     {
          LevelLoad();
     }
}




#if UNITY_EDITOR
[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor {
     public override void OnInspectorGUI() {
          base.OnInspectorGUI();

          if (GUILayout.Button("Next Level"))
               LevelManager.Instance.SetCurrentLevel();
          
     }
}

#endif