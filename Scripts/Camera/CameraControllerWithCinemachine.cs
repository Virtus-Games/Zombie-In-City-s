using UnityEngine;
using Cinemachine;

public class CameraControllerWithCinemachine : Singleton<CameraControllerWithCinemachine>
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public void SetData()
    {
        GetComponent<CinemachineBrain>().enabled = true;
        GameObject Player = GameObject.FindWithTag("Player");
        cinemachineVirtualCamera.Follow = Player.transform;

    }


    private void OnEnable() => LevelManager.OnLevelLoaded += OnLevelLoaded;

    private void OnDisable() => LevelManager.OnLevelLoaded -= OnLevelLoaded;

    private void OnLevelLoaded(bool arg0)
    {
        if (arg0)
            CharackterStatus(true);
        else
            CharackterStatus(false);
    }

    public void CharackterStatus(bool isStatus)
    {
        if (isStatus)
        {
            SetData();
        }
        else
        {
            cinemachineVirtualCamera.Follow = null;
            cinemachineVirtualCamera.LookAt = null;
            GetComponent<CinemachineBrain>().enabled = false;
        }
    }
}