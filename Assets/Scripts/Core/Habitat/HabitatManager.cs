using System.Collections.Generic;
using UnityEngine;

public class HabitatManager : MonoBehaviour
{
    private List<ChimeraData> _chimeraDataList = null;
    private List<FacilityData> _facilityDataList = null;
    private HabitatData _habitatData = new HabitatData();
    private Collections _collections = new Collections();
    private PersistentData _persistentData = null;
    private Habitat _currentHabitat = null;
    private HabitatUI _habitatUI = null;
    private float _tickTimer = 0.06f;

    public HabitatData HabitatData { get => _habitatData; }
    public List<FacilityData> FacilitiesInHabitat { get => _facilityDataList; }
    public List<ChimeraData> ChimerasInHabitat { get => _chimeraDataList; }
    public Collections Collections { get => _collections; }
    public Habitat CurrentHabitat { get => _currentHabitat; }
    public float TickTimer { get => _tickTimer; }

    public bool IsFacilityBuilt(UpgradeNode upgradeNode)
    {
        foreach (FacilityData facilityData in _facilityDataList)
        {
            if (facilityData.Type == upgradeNode.FacilityType && facilityData.CurrentTier >= upgradeNode.Tier)
            {
                return true;
            }
        }

        return false;
    }

    public int GetFacilityTier(FacilityType facilityType)
    {
        foreach (FacilityData facilityData in _facilityDataList)
        {
            if (facilityData.Type == facilityType)
            {
                return facilityData.CurrentTier;
            }
        }

        return 0;
    }

    public void SetHabitatUI(HabitatUI habiatUI) { _habitatUI = habiatUI; }

    public void SetCurrentHabitat(Habitat habitat)
    {
        _currentHabitat = habitat;
    }

    public void SetExpeditionProgress(int essence, int fossil, int habitat)
    {
        _habitatData.ExpeditionEssenceProgress = essence;
        _habitatData.ExpeditionHabitatProgress = habitat;
        _habitatData.ExpeditionFossilProgress = fossil;
    }

    public void SetHabitatUIProgressFacility(bool cave, bool rune, bool waterfall)
    {
        _habitatData.CaveUnlocked = cave;
        _habitatData.RuneUnlocked = rune;
        _habitatData.WaterfallUnlocked = waterfall;
    }

    public void SetHabitatTier(int tier) { _habitatData.CurrentTier = tier; }

    public HabitatManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _persistentData = ServiceLocator.Get<PersistentData>();

        LoadHabitatData();

        return this;
    }

    public void LoadHabitatData()
    {
        _habitatData = _persistentData.HabitatData;
        _chimeraDataList = _persistentData.ChimeraData;
        _facilityDataList = _persistentData.FacilityData;
        _collections.LoadData(_persistentData.CollectionData);
    }

    public void ResetHabitatData()
    {
        _chimeraDataList.Clear();
        _facilityDataList.Clear();
    }

    public void UpdateCurrentChimeras()
    {
        if (_currentHabitat == null)
        {
            return;
        }

        _chimeraDataList.Clear();

        foreach (Chimera chimera in _currentHabitat.ActiveChimeras)
        {
            AddNewChimera(chimera);
        }
    }

    public void UpdateCurrentFacilities()
    {
        if (_currentHabitat == null)
        {
            return;
        }

        _facilityDataList.Clear();

        foreach (Facility facility in _currentHabitat.Facilities)
        {
            AddNewFacility(facility);
        }
    }

    public void AddNewChimera(Chimera chimeraToSave)
    {
        ChimeraData chimeraSavedData = new ChimeraData(chimeraToSave);

        _chimeraDataList.Add(chimeraSavedData);
    }

    public void AddNewFacility(Facility facilityToSave)
    {
        FacilityDeleteCheck(facilityToSave.Type);

        FacilityData facilitySavedData = new FacilityData(facilityToSave);

        _facilityDataList.Add(facilitySavedData);
    }

    public void AddNewFacility(FacilityData facilityToSave)
    {
        FacilityDeleteCheck(facilityToSave.Type);

        _facilityDataList.Add(facilityToSave);
    }

    private void FacilityDeleteCheck(FacilityType facilityToSave)
    {
        foreach (FacilityData facility in _facilityDataList)
        {
            if (facilityToSave == facility.Type)
            {
                _facilityDataList.Remove(facility);

                return;
            }
        }
    }

    public void SpawnChimerasForHabitat()
    {
        _currentHabitat.CreateChimerasFromData(_chimeraDataList);

        _habitatUI.DetailsManager.DetailsStatGlow();
    }

    public void BuildFacilitiesForHabitat()
    {
        _currentHabitat.CreateFacilitiesFromData(_facilityDataList);
    }
}