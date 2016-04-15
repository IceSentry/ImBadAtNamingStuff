using System.Collections.Generic;
using UnityEngine;

public class JobManager : MonoBehaviour {
    private string JobMode = "Default";

    public static JobManager Instance { get; protected set; }

    private Queue<Job> Jobs;

    private void Start() {
        if (Instance != null) {
            Debug.LogError("There should never be more than 1 JobManager.");
        }
        Instance = this;

        Jobs = new Queue<Job>();
    }

    private void Update() {
        //TODO remove this temporary hack to complete job instantly
        if (Jobs.Count > 0) {
            Job job = Jobs.Dequeue();
            job.DoJob();
        }
    }

    public void SetJobMode(string mode) {
        JobMode = mode;
    }

    public void CreateJobAt(Tile tile) {
        //TODO Use an action or something similar for the currentJob
        string jobMode = JobMode;
        Job job = new Job(tile, j => {
            switch (jobMode) {
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