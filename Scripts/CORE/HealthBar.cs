using PlayerNameSpace;
using UnityEngine;




public class HealthBar : MonoBehaviour
{

    private float plusHealth;
    [SerializeField] private GameObject Particle;

    public Vector3 offsetUp;


    public void SetData(float health, float timeForDestroy)
    {
        plusHealth = health;
        Destroy(gameObject, timeForDestroy);
    }

    private GameObject Player;
    public float Distance = 3;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        offsetUp = new Vector3(0, 2, 0);


    }

    private void Update()
    {

        if (Vector3.Distance(transform.position, Player.transform.position) < Distance)
            transform.position = Vector3.Slerp(transform.position, Player.transform.position + offsetUp, Time.deltaTime * 2);
        else
            transform.position = Vector3.Lerp(transform.position, transform.position + offsetUp, Time.deltaTime * 2);

    }

    private void OnCollisionEnter(Collision other)
    {
        Particle.SetActive(true);

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IHealth>().SetDamage(plusHealth);
            Destroy(gameObject);
        }
    }


    public float GetHealth()
    {
        return plusHealth;
    }
}
