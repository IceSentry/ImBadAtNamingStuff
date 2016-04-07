using UnityEngine;
using System.Collections;
using System;

public class Job
{

    public Tile tile { get; protected set; }
    float jobTime;

    Action<Job> jobComplete;

    public Job(Tile tile, Action<Job> jobComplete, float jobTime = 0.1f)
    {
        this.tile = tile;
        this.jobComplete = jobComplete;
        this.jobTime = jobTime;
    }

    public void DoJob(float deltaTime = 0.1f)
    {
        jobTime -= deltaTime;

        if (jobTime <= 0)
        {
            if (jobComplete != null)
                jobComplete(this);
        }
    }

    public void CancelJob()
    {
        jobComplete = null;
        tile = null;
    }
}
