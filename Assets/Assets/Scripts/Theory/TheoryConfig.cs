using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TheoryConfig", menuName = "ChemistryApp/TheoryConfig")]
public class TheoryConfig : ScriptableObject
{
    [SerializeField] private List<TheoryChapter> _organicChemistry;
    [SerializeField] private List<TheoryChapter> _nonorganicChemistry;

    [System.NonSerialized]
    private bool _initialized = false;

    public IEnumerable<TheoryChapter> OrganicChemistry
    {
        get
        {
            if (!_initialized)
                Initialize();

            return _organicChemistry;
        }
    }

    public IEnumerable<TheoryChapter> NonorganicChemistry
    {
        get
        {
            if (!_initialized)
                Initialize();

            return _nonorganicChemistry;
        }
    }

    public void Initialize()
    {
        var allChapters = new List<TheoryChapter>();

        allChapters.AddRange(_organicChemistry);
        allChapters.AddRange(_nonorganicChemistry);

        int id = 1;

        foreach (var chapter in  allChapters)
        {
            chapter.Id = id++;
            Debug.Log(chapter.Id);
        }

        _initialized = true;
    }
}

[System.Serializable]
public class TheoryChapter
{
    [SerializeField] private string _title;
    [Space]
    [SerializeField] private string _text—ontent;

    public int Id { get; set; }
    public string Title => _title;
    public string TextContent => _text—ontent;
}