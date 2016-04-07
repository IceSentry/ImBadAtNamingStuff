using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class JobManager : MonoBehaviour
{
    string JobMode = "Default";

    public static JobManager Instance { get; protected set; }

    Queue<Job> Jobs;

    // Use this for initialization
    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There should never be more than 1 JobManager.");
        }
        Instance = this;

        Jobs = new Queue<Job>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Jobs.Count > 0)
        {
            Job job = Jobs.Dequeue();
            job.DoJob();
        }
    }

    public void SetJobMode(string mode)
    {
        JobMode = mode;
    }

    public void CreateJobAt(Tile tile)
    {
        string jobMode = JobMode;
        Job job = new Job(tile, j =>
        {
            switch (jobMode)
            {
                case "Mine":
                    WorldActionManager.MineAt(j.tile);
                    break;
                default:
                    break;
            }

        });

        Jobs.Enqueue(job);
    }
}
