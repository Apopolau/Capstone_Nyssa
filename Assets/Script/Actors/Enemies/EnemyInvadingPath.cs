using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyInvadingPath : MonoBehaviour
{

    [SerializeField] public List<Transform> pathOneWaypoints;
    [SerializeField] public List<Transform> pathTwoWaypoints;
    [SerializeField] public List<Transform> pathThreeWaypoints;
    [SerializeField] public List<Transform> pathFourWaypoints;
    [SerializeField] public List<Transform> escapeWaypoints;
    public List<List<Transform>> pathsList = new List<List<Transform>>();
   // public List<GameObject> pathsList = new List<GameObject>();
    public int pathCount;
    public List<Transform> chosenPath;
    public List<Transform> BranchWayPoints;

    // Update is called once per frame
    public void LoadPathList()
    {
       /* foreach (GameObject path in pathSet.Items)
        {
            pathsList.Add(path);
        }*/
       pathsList.Add(pathOneWaypoints);
      pathsList.Add(pathTwoWaypoints);
       pathsList.Add(pathThreeWaypoints);
       pathsList.Add(pathFourWaypoints);
        pathCount = pathsList.Count;
        Debug.Log("path count:" + pathCount);
    }
    public List<Transform> getRandomPath()
    {

        int index = Random.Range(0, pathCount - 1);
        Debug.Log("You have chosen oath number:" + index);
       
        return pathsList[index];
    }
    public void setEnemiesPath(Enemy enemy, List<Transform> selectedPath)
    {
        enemy.chosenPath = selectedPath;
        Debug.Log("checking paths pos 2:" + selectedPath[0].transform.position);
     
     
    }

    public Transform getClosestEscapeRoute(Enemy enemy)
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
        

    public void setClosestEscapeRoute(Enemy enemy, Transform escapeWaypoint)
    {

        enemy.escapeRoute = escapeWaypoint;
    }
}
