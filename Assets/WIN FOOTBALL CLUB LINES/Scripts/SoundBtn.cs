using UnityEngine.UI;
using UnityEngine;

public class SoundBtn : MonoBehaviour
{
    Image img;
    Button btn;

    [Space(10)]
    [SerializeField] Sprite enable;
    [SerializeField] Sprite disable;

    [Space(10)]
    [SerializeField] AudioSource source;

    private void Start()
    {
        img = GetComponent<Image>();
        btn = GetComponent<Button>();

        img.sprite = source.mute ? disable : enable;

        btn.onClick.AddListener(() =>
        {
            source.mute = !source.mute;
            img.sprite = source.mute ? disable : enable;
        });
    }
}
