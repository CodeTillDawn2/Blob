using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{



    private void Awake()
    {

    }

    private void Start()
    {

        PlayerTentacles.TentacleCreatedEvent += HandleTentacleCreated;

    }

    private string HandleTentacleCreated(object sender, EventArgs e)
    {
        return "a string to return";
    }





}