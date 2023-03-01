using UnityEngine;


public class Waypoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextIndex(i);

            Gizmos.color = Color.blue;

            Gizmos.DrawSphere(GetPosition(i).position, 0.15f);

            Gizmos.DrawLine(GetPosition(i).position, transform.GetChild(j).position);
        }
    }

    public int GetNextIndex(int i)
    {
        int j = i + 1;

        if (j == transform.childCount)
        {
            j = 0;
            lastPosition(j);
        }

        return j;
    }

    public Transform GetPosition(int i) => transform.GetChild(i);

    public bool lastPosition(int j)
    {
        if (j == transform.childCount - 1)
            return true;
            
        else return false;
    }
}
