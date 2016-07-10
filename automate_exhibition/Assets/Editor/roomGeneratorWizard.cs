using UnityEditor;
using UnityEngine;

public class WizardCreateLight : ScriptableWizard
{

    public GameObject RoomGenerator;

    [MenuItem("GameObject/Create Light Wizard")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create", "Apply");
        //If you don't want to use the secondary button simply leave it out:
        //ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
    }

    void OnWizardCreate()
    {
        RoomGenerator.GetComponent<generateRoom>().generateBasicRoom();
        RoomGenerator.GetComponent<objectGenerator>().generateObjects(2);
    }

    void OnWizardUpdate()
    {
        helpString = "Please set the color of the light!";
    }

    // When the user pressed the "Apply" button OnWizardOtherButton is called.
    void OnWizardOtherButton()
    {
    }
}