using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Data/Game Settings", order = 1)]
public class GameSettings : ScriptableObject {
    [Header("Player")]
    public int StartingSanityValue = 100;
    public int MaxSanityValue = 100;
    public int MaxBatteryAmount = 3;

    [Header("Enemy")]
    public int EnemySpottedPlayerSanityLoss = 10;
    public int RespawnTime = 60;

    [Header("Enemy Perception")]
    public float DetectionDistance = 10f;
    public float PerceptionRadius = 90f;
    public float PerceptionRadiusChasing = 180f;
    public float TorchOnDistanceIncrease = 2f;
    public float SusTime = 3f;

    [Header("Checkpoint")]
    public int CheckpointAddSanityOnRevive = 100;

    [Header("ConditionLight")]
    public float ConditionLightTickRate = 0.5f;
    public int ConditionLightAmountPerTick = 1;
}

