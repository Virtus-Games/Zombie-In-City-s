
using System.Collections.Generic;
using UnityEngine;
using EnemyNameSpace;
using System;

public class DeadCountManager : Singleton<DeadCountManager>
{
    [HideInInspector]
    public List<DeadCount> deads;
    public List<GameObject> deadsObj;
    public GameObject ParentForDeadCount;
    public GameObject deadPrefab;
    public Transform WinnerDeadCount;
    public Transform PlayPanelDeadCount;

    public void DeadController(Zombie dead)
    {
        ParentForDeadCount.SetActive(true);

        if (deads.Count != 0 && deads.Find(x => x.dead.zombieType == dead.zombieType) != null)
        {
            DeadCount deadCount = deads.Find(x => x.dead.zombieType == dead.zombieType);
            deadCount.SetDeadCount(deadCount.dead.deadCount + 1);
            ZombieValue--;
        }
        else
        {
            GameObject obj = Instantiate(deadPrefab, ParentForDeadCount.transform);
            DeadCount newDeadCount = obj.GetComponent<DeadCount>();
            newDeadCount.SetData(dead);
            deads.Add(newDeadCount);
            deadsObj.Add(obj);
            ZombieValue--;
        }
    }





    private void UpdateGameState(GAMESTATE obj)
    {

        if (obj == GAMESTATE.VICTORY)
        {
            if (deadsObj.Count != 0)
            {
                foreach (GameObject child in deadsObj)
                {
                    child.transform.SetParent(WinnerDeadCount.transform);
                }
            }
        }
        if (obj == GAMESTATE.START)
        {
            DestroyChilds();
            deads.Clear();
            deadsObj.Clear();
        }

        if (obj == GAMESTATE.PLAY)
        {

        }
    }
    public int ZombieValue;
    private void OnLevelLoaded(bool arg0)
    {
        if (arg0)
        {
            ZombieValue = FindObjectsOfType<EnemyController>().Length;
        }
        else
            ZombieValue = 0;
    }

    public void DestroyChilds()
    {

        if (deadsObj.Count != 0)

            foreach (var child in deadsObj)
                Destroy(child);
    }


    private void OnEnable()
    {
        GameManager.OnGameStateChanged += UpdateGameState;
        LevelManager.OnLevelLoaded += OnLevelLoaded;
    }



    private void OnDisable() => GameManager.OnGameStateChanged -= UpdateGameState;
}


