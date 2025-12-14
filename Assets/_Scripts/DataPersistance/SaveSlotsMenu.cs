using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class SaveSlotsMenu : MonoBehaviour
{
    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopUpMenu confirmationPopUpMenu;

    private SaveSlot[] saveSlots;

    private MenuPage menuPage;

    private void Awake()
    {
        menuPage = GetComponent<MenuPage>();
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DataPersistanceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if (!DataPersistanceManager.Instance.HasGameData())
            DataPersistanceManager.Instance.NewGame();

        DataPersistanceManager.Instance.SaveGame();
        AsyncSceneLoader.Instance.LoadLevel(1);
    }

    public void OnClearClicked(SaveSlot saveSlot)
    {
        confirmationPopUpMenu.ActivateMenu( "Are you sure you want to delete this saved data?",
            ()=>
            {
                DataPersistanceManager.Instance.DeleteProfileData(saveSlot.GetProfileId());
                ActivateMenu();
                menuPage.ChangeObjectSelected();
            },
            () =>
            {
                ActivateMenu();
                menuPage.ChangeObjectSelected();
            }
        );

        
    }

    public void ActivateMenu()
    {
        //Load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistanceManager.Instance.GetAllProfilesGameData();

        //loop through each save slot in the UI and set the content appropriatley.
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
        }
    }
}
