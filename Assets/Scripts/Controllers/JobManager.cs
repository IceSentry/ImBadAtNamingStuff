using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JobManager : MonoBehaviour
{
    string JobMode = "Default";

    public static JobManager Instance { get; protected set; }

    List<Job> Jobs;

    // Use this for initialization
    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There should never be more than 1 JobManager.");
        }
        Instance = this;

        Jobs = new List<Job>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetJobMode(string mode)
    {
        JobMode = mode;
    }

    public void CreateJobAt(Tile tile)
    {

    }
}
