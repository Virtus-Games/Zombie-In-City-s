using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EnemyNameSpace;

[System.Serializable]
public class Zombie
{
    public Sprite zombieHead;
    public ZombieType zombieType;

    [HideInInspector]
    public int deadCount;

    [Range(0, 3)]
    public float health = 1f;
}

public class DeadCount : MonoBehaviour
{
    public TextMeshProUGUI deadCountText;
    public Image currentHead;
    public Zombie dead;

    public void SetDeadCount(int val)
    {
        dead.deadCount = val;
        deadCountText.SetText("X" + dead.deadCount.ToString());
    }

    public void SetData(Zombie type)
    {
        dead = type;
        currentHead.sprite = dead.zombieHead;
        SetDeadCount(1);
    }
}
