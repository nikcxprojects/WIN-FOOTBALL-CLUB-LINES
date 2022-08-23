using UnityEngine;

[CreateAssetMenu(fileName = "New Translation", menuName = "Create new Translation", order = 51)]
public class Translation : ScriptableObject
{
    public string startBtnString;
    public string difficultString;
    public string languageString;

    public string backString;

    public string easyString;
    public string mediumString;
    public string hardcoreString;

    public string easyDescrString;
    public string mediumDescrString;
    public string hardcoreDescrString;

    public string scoreString;

    public string restartString;
    public string exitToMenuString;
}
