using EnemyNameSpace;
using UnityEngine;

public class ParticulGirly : MonoBehaviour
{
    public float range;
    public float timer;
    private float _timer;

    private EnemyHealth health;

    private void Start() => _timer = timer;
    public void GivePlayer(EnemyHealth enemyHealth, float range)
    {
        health = enemyHealth;
        this.range = range;
    }

    private GameObject _Player;

    private void OnTriggerStay(Collider other)
    {
        timer -= Time.deltaTime;
        if (other.gameObject.CompareTag("Player"))
        {
            _Player = other.gameObject;
            if (timer <= 0 && _Player != null)
            {
                _Player.gameObject.GetComponent<IHealth>().SetDamage(-range);
                health.SetDamage(-range);
                timer = _timer;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _Player = null;
            timer = _timer;
            gameObject.SetActive(false);

        }
    }
}
