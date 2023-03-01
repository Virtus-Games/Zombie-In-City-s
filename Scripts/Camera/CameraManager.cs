using UnityEngine;
using System.Collections;
using ICoreManager;
using Cinemachine;

[ExecuteAlways]
public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] float followSpeed;
    [SerializeField] Transform player;
    [SerializeField] private Vector3 offset;

    public AudioSource gunSource;


    public void GunPlaySource(AudioClip clip,float volume = 0.7f)
    {
        gunSource.PlayOneShot(clip, volume);
    }



    // void StartMovement(Transform to)
    // {
    //     MovementTrue();
    //     StartCoroutine(
    //     CameraMovement(transform, to.rotation, to.position, followSpeed));
    // }

    private void OnEnable()
    {
        LevelManager.OnLevelLoaded += OnLevelLoaded;

    }

    private void OnDisable()
    {
        LevelManager.OnLevelLoaded -= OnLevelLoaded;
    }
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
            player = GameObject.FindWithTag("Player").transform;
        else
        {
            player = null;
        }
    }

    private void Update()
    {
        if (player != null)
            transform.position = Vector3.Lerp(transform.position, player.position + offset, followSpeed * Time.deltaTime);
    }
}

namespace ICoreManager
{
    public abstract class IACameraManager : Singleton<IACameraManager>
    {
        private bool _isStartMovement = false;
        public void MovementTrue() => _isStartMovement = true;

        public bool IsStartMovement => _isStartMovement;

        public IEnumerator CameraMovement(Transform player, Quaternion toRotation, Vector3 toPosition, float followSpeed)
        {
            while (_isStartMovement)
            {
                player.rotation = Quaternion.Slerp(player.rotation, toRotation, Time.deltaTime * followSpeed);
                player.position = Vector3.Slerp(player.position + toPosition, player.position, Time.deltaTime * followSpeed);

                if (Vector3.Distance(player.position, player.position) < 0.1f)
                    _isStartMovement = false;
            }
            yield return null;
        }
    }
}
