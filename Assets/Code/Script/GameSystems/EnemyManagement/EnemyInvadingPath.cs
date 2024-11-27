using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyInvadingPath : MonoBehaviour
{

    //[SerializeField] public List<Transform> pathOneWaypoints;
    //[SerializeField] public List<Transform> pathTwoWaypoints;
    //[SerializeField] public List<Transform> pathThreeWaypoints;
    //[SerializeField] public List<Transform> pathFourWaypoints;
    [SerializeField] private List<List<Transform>> pathsList = new List<List<Transform>>();
    [SerializeField] private List<GameObject> waypointClusters = new List<GameObject>();
    [SerializeField] private List<Transform> escapeWaypoints;
    // public List<GameObject> pathsList = new List<GameObject>();
    public int pathCount;
    //public List<Transform> chosenPath;
    //public List<Transform> BranchWayPoints;

    private void Start()
    {
        LoadPathList();
    }

    // Update is called once per frame
    public void LoadPathList()
    {
        int i = 0;
        foreach(GameObject go in waypointClusters)
        {
            //pathsList[i] = new List<Transform>();
            List<Transform> wayPointList = new List<Transform>();
            pathsList.Add(wayPointList);
            foreach(Transform childTransform in waypointClusters[i].transform)
            {
                pathsList[i].Add(childTransform);
            }
            i++;
        }
        pathCount = pathsList.Count;
    }

    public List<Transform> GetPath(int index)
    {
        //int index = Random.Range(0, pathCount - 1);
        return pathsList[index];
        // return pathsList[index];
    }

    public Transform GetEscapePoint(int index)
    {
        return escapeWaypoints[index];
    }

    /*
    public void setEnemiesPath(KidnappingEnemy enemy, List<Transform> selectedPath)
    {
        enemy.SetChosenPath(selectedPath);
    }
    */

    public Transform getClosestEscapeRoute(KidnappingEnemy enemy)
    {
        float minDistance = 5000;
        Transform minEscapeWPoint = null;
        foreach (Transform escapeWPoint in escapeWaypoints)
        {
            float distance = Vector3.Distance(escapeWPoint.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minEscapeWPoint = escapeWPoint;
                minDistance = distance;
            }
        }
        return minEscapeWPoint;
    }


    public void setClosestEscapeRoute(KidnappingEnemy enemy, List<Transform> escapeWaypoint)
    {

        enemy.SetEscapeRoute(escapeWaypoint);
    }
}
