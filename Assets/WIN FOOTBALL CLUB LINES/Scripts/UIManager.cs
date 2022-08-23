using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject select_language;
    [SerializeField] GameObject select_DIFFICULTY;
    [SerializeField] GameObject game;

    [Space(10)]
    [SerializeField] SimpleScrollSnap diffSnap;

    [Space(10)]
    [SerializeField] TranslateME diffGame;

    void Start()
    {
        diffSnap.onPanelChanged.AddListener(() =>
        {
            simple_math_3_game.Manager.Instance.SetDifficult(diffSnap.CurrentPanel);
            diffGame.idFoTranslate = 4 + diffSnap.TargetPanel;
            diffGame.Translate();
        });

        simple_math_3_game.Manager.Instance.SetDifficult(0);
        diffGame.idFoTranslate = 4 + 0;
        diffGame.Translate();
    }

    public void OpenGame()
    {
        menu.SetActive(false);
        game.SetActive(true);

        simple_math_3_game.Manager.Instance.Start_Game();
    }

    public void SelectLanguage()
    {
        menu.SetActive(false);
        select_language.SetActive(true);
    }

    public void Select_Difficult()
    {
        menu.SetActive(false);
        select_DIFFICULTY.SetActive(true);
    }

    public void SaveAndBack()
    {
        select_DIFFICULTY.SetActive(false);
        select_language.SetActive(false);
        menu.SetActive(true);
    }

    public void ExitInMenu()
    {
        game.SetActive(false);
        menu.SetActive(true);
    }
}
