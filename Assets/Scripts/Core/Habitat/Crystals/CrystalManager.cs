using System.Collections.Generic;
using UnityEngine;

public class CrystalManager : MonoBehaviour
{
    [SerializeField] private List<CrystalSpawn> crystals = null;
    [SerializeField] private float _spawnDelay = 20;
    private Habitat _habitat = null;
    private ExpeditionManager _expeditionManager = null;
    private int _tracker = 0;

    public void SetExpeditionManager(ExpeditionManager expeditionManager) { _expeditionManager = expeditionManager; }

    public void Initialize(Habitat habitat)
    {
        _habitat = habitat;

        foreach (CrystalSpawn crystal in crystals)
        {
            crystal.Initialize();
        }
    }

    public void SpawnTick()
    {
        if (_expeditionManager.CurrentHabitatProgress == 0)
        {
            return;
        }

        if (++_tracker >= _spawnDelay)
        {
            _tracker = 0;
            SpawnCrystal();
        }
    }

    private void SpawnCrystal()
    {
        int activeCount = 0;
        List<CrystalSpawn> inactiveCrystals = new List<CrystalSpawn>();

        foreach (CrystalSpawn crystal in crystals)
        {
            if (crystal.IsActive == true)
            {
                ++activeCount;
            }
            else
            {
                inactiveCrystals.Add(crystal);
            }
        }

        if (inactiveCrystals.Count == 0)
        {
            Debug.LogError($"Error: No more inactive crsytals.");
            return;
        }

        if (activeCount < _habitat.CurrentTier * 2 + 1)
        {
            int rand = Random.Range(0, inactiveCrystals.Count);
            inactiveCrystals[rand].Activate(_habitat.CurrentTier);
        }
    }

    public void TripleSpawn()
    {
        SpawnCrystal();
        SpawnCrystal();
        SpawnCrystal();
    }
}