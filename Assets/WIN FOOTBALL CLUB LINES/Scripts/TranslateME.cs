using UnityEngine;
using UnityEngine.UI;

public class TranslateME : MonoBehaviour
{
    public int idFoTranslate;

    public void Translate()
    {
        GetComponent<Text>().text = Translater.Instance.TryGetTranslation(idFoTranslate);
    }
}