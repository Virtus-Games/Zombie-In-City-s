using System;
using UnityEngine;

public class Money : MonoBehaviour
{
    public float speed = 2.0f;

    private bool isPlay;

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void TranslateController()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        isPlay = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Played();
        }
    }


    public void Played()
    {
        transform.localScale = new Vector3(transform.localScale.x - 0.5f, transform.localScale.y - 0.5f, transform.localScale.z - 0.5f);
        isPlay = true;
    }
    void Update()
    {
        if (isPlay)
            TranslatetoObJToCanvasPoint();
    }



    public void TranslatetoObJToCanvasPoint()
    {

        transform.position = Camera.main.WorldToScreenPoint(transform.position);
        isPlay = false;
        UIManager.Instance.TransformMoney(transform.position);
        Destroy(gameObject);
    }
}
