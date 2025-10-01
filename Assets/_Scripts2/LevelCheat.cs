using UnityEngine;

public class LevelCheat : MonoBehaviour
{
    public string code;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) code += "s";
        if (Input.GetKeyDown(KeyCode.T)) code += "t";
        if (Input.GetKeyDown(KeyCode.A)) code += "a";
        if (Input.GetKeyDown(KeyCode.R)) code += "r";

        if (code == "star")
        {
            PlayerPrefs.SetInt("CompletedLevels", 2);
            PlayerPrefs.SetInt("CompletedTutorial", 1);
            PlayerPrefs.SetInt("AccordeonUnlocked", 1);
            PlayerPrefs.Save();

            ProgressManager pm = FindObjectOfType<ProgressManager>();
            if (pm != null)
            {
                pm.RefreshProgress();
            }

            Destroy(gameObject);
        }
    }
}
