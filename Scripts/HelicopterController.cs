using System.Collections;
using System.Collections.Generic;
using PlayerNameSpace;
using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    [Tooltip("Player in Scene")]
    public GameObject player;

    [Tooltip("Animation lerp speed")]
    public float lerpSpeed;
    [Tooltip("Max speed")]
    public float maxSpeed;
    public Animator helikopterAnimator;
    private float animationSpeed;



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && DeadCountManager.Instance.ZombieValue < 5)
        {

            StartCoroutine(Fly());
            if (other.CompareTag("Player"))
            {
                other.gameObject.SetActive(false);
                other.gameObject.GetComponent<Health>().Close(false);
            }

            GetComponent<ParticleSystem>().Stop();
        }
    }

    IEnumerator Fly()
    {
        StartCoroutine(ShowWin());
        while (true)
        {
            animationSpeed = Mathf.Lerp(animationSpeed, maxSpeed, Time.deltaTime * lerpSpeed);
            helikopterAnimator.SetFloat("animationSpeed", animationSpeed);
            if (animationSpeed > 5)
                helikopterAnimator.SetBool("fly", true);

            yield return null;
        }
    }

    private IEnumerator ShowWin()
    {
        CameraManager.Instance.CharackterStatus(false);
        yield return new WaitForSeconds(5);
        GameManager.Instance.UpdateGameState(GAMESTATE.VICTORY);
    }
}
