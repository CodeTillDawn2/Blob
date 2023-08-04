using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class BlobTentacles : MonoBehaviour
{

    [Header("Stat Block")]
    [Serialize] public IntegerVariable TentacleCount;
    [Serialize] public IntegerVariable CurrentMaxTentacles;
    [Serialize] public GameObjectVariable parentRB;
    [Serialize] public FloatVariable MaxTentacleReach;
    [Serialize] public FloatVariable MinTentacleReach;
    [Serialize] public Vector3Variable BlobDims;
    [Serialize] public PlayerStatsBase StartingStats;
    [Serialize] public Dict_GameObjectToLastSeen ObjectsSeen;


    public GameObject FrontSide;
    public GameObject BackSide;
    public GameObject LeftSide;
    public GameObject RightSide;
    public GameObject TopSide;
    public GameObject BottomSide;

    public GameObject TentaclePrefab;

    private int TentacleID = 1;

    private List<Bounds> TentacleSpawnRegions;

    private List<SmoothTentacle> existingTentacles;

 
    private void OnEnable()
    {
        TentacleCount.Value = 0;
    }

    private void Start()
    {
        MaxTentacleReach.Value = StartingStats.MaxTentacleReach;
        MinTentacleReach.Value = StartingStats.MinTentacleReach;
        CurrentMaxTentacles.Value = StartingStats.MaxTentacles;
        SetTentacleSpawnRegions();

    }

    private void SetTentacleSpawnRegions()
    {
        TentacleSpawnRegions = new List<Bounds>();
        foreach (GameObject side in new List<GameObject>() { FrontSide, BackSide, LeftSide, RightSide }) 
        {
            BoxCollider boxcol = side.GetComponent<BoxCollider>();
            if (boxcol != null)
            {

                Bounds bound = new Bounds(boxcol.transform.position, boxcol.bounds.size);
                TentacleSpawnRegions.Add(bound);

            }
        }



    }


    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        CreateTentacles();
    }



    public void CreateTentacles()
    {
        List<GameObject> potentialvictims = ObjectsSeen.Value.Keys.Where(x => x != null &&
            (x.transform.position - gameObject.transform.position).sqrMagnitude <= MaxTentacleReach.Value * MaxTentacleReach.Value &&
            (x.transform.position - gameObject.transform.position).sqrMagnitude >= MinTentacleReach.Value * MinTentacleReach.Value &&
            x.GetComponent<IAmImmuneToTargetingByTentacles>() == null).ToList();

        existingTentacles = GetComponentsInChildren<SmoothTentacle>().ToList();

        for (int i = CurrentMaxTentacles.Value; i < existingTentacles.Count; i++)
        {
            DespawnTentacle(existingTentacles, existingTentacles[i]);
        }

        for (int tentacleID = existingTentacles.Count; tentacleID < CurrentMaxTentacles.Value && tentacleID < potentialvictims.Count; tentacleID++)
        {
            CreateTentacle(tentacleID);
        }
    }

    private void CreateTentacle(int TentacleID)
    {
        GameObject tentacleobj = Instantiate(TentaclePrefab, parentRB.Value.transform.position + new Vector3(0, -1000, 0), Quaternion.identity);
        SmoothTentacle smoothTentacle = tentacleobj.GetComponent<SmoothTentacle>();
        if (smoothTentacle != null)
        {
            existingTentacles.Add(smoothTentacle);
            tentacleobj.name = "Tentacle" + UnityEngine.Random.Range(1, 100); // TentacleID;
            Rigidbody rb = parentRB.Value.GetComponent<Rigidbody>();
            smoothTentacle.Go(gameObject, rb, TentacleSpawnRegions, ObjectsSeen);
        }
        else
        {
            Destroy(tentacleobj);
        }

    }

   
    private void DespawnTentacle(List<SmoothTentacle> tentacles, SmoothTentacle tentacle)
    {

        tentacle.IsAlive.Value = false;
        tentacles.Remove(tentacle);

    }




}
