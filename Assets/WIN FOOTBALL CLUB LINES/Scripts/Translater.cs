using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;

public class Translater : MonoBehaviour
{
    private static Translater instance;
    public static Translater Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<Translater>();
            }

            return instance;
        }
    }

    Translation currentTranslation;

    [Space(10)]
    [SerializeField] SimpleScrollSnap simpleScrollSnap;

    [Space(10)]
    [SerializeField] Translation[] translations;

    [Space(10)]
    [SerializeField] TranslateME[] translateMEs;

    void Start()
    {
        simpleScrollSnap.onPanelChanged.AddListener(() =>
        {
            SetLanguage(simpleScrollSnap.TargetPanel);
        });

        SetLanguage(0);
    }

    void SetLanguage(int i)
    {
        currentTranslation = translations[i];
        UpdateElements();
    }

    void UpdateElements()
    {
        foreach (TranslateME t in translateMEs)
        {
            t.Translate();
        }
    }

    public string TryGetTranslation(int id) => id switch
    {
        0 => currentTranslation.startBtnString,
        1 => currentTranslation.difficultString,
        2 => currentTranslation.languageString,

        3 => currentTranslation.backString,

        4 => currentTranslation.easyString,
        5 => currentTranslation.mediumString,
        6 => currentTranslation.hardcoreString,

        7 => currentTranslation.easyDescrString,
        8 => currentTranslation.mediumDescrString,
        9 => currentTranslation.hardcoreDescrString,

        10 => currentTranslation.scoreString,

        11 => currentTranslation.restartString,
        12 => currentTranslation.exitToMenuString,

        _ => "NULL"
    };
}
