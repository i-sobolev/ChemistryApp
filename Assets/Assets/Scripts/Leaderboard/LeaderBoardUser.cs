using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardUser : MonoBehaviour
{
    [SerializeField] private Image _medalIco;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _score;
    [Space]
    [SerializeField] private Sprite[] _medalsSprites;

    public void SetData(int place, string name, int score)
    {
        _medalIco.sprite = GetMedalSprite(place);
        _name.text = name;
        _score.text = score.ToString();
    }

    public Sprite GetMedalSprite(int place) => _medalsSprites[Mathf.Clamp(place, 0, _medalsSprites.Length - 1)];
}