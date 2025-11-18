using UnityEngine;

public class LevelCheat : MonoBehaviour
{
    [SerializeField] ProgressManager progressManager;
    public string code;
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) code += "s";
        if (Input.GetKeyDown(KeyCode.T)) code += "t";
        if (Input.GetKeyDown(KeyCode.A)) code += "a";
        if (Input.GetKeyDown(KeyCode.R)) code += "r";

        if (code == "star")
        {
            PlayerPrefs.SetInt("CompletedLevels", 3);
            PlayerPrefs.SetInt("CompletedTutorial", 1);
            //PlayerPrefs.SetInt("AccordeonUnlocked", 1);
            //PlayerPrefs.SetInt("SaxofonUnlocked", 1);
            PlayerPrefs.Save();

            if (progressManager != null)
            {
                progressManager.UnlockAll();
            }

            Destroy(gameObject);
        }
    }
}
