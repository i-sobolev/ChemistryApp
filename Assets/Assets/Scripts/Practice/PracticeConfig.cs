using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PracticeConfig", menuName = "ChemistryApp/PracticeConfig")]
public class PracticeConfig : ScriptableObject
{
    [SerializeField] private List<PracticeChapter> _practiceChapters;

    [NonSerialized] private bool _initialized = false;

    public IEnumerable<PracticeChapter> PracticeChapters
    {
        get
        {
            if (!_initialized)
                Initialize();

            return _practiceChapters;
        }
    }

    public void Initialize()
    {
        int id = 1;

        foreach (var chapter in _practiceChapters)
            foreach (var task in chapter.Tasks)
                task.Id = id++;

        _initialized = true;
    }
}

[System.Serializable]
public class PracticeChapter
{
    [SerializeField] private string _title;
    [Space]
    [SerializeField] private List<PracticeTask> _tasks;

    public string Title => _title;
    public IEnumerable<PracticeTask> Tasks => _tasks;
}

[System.Serializable]
public class PracticeTask
{
    [TextArea(1, 20)]
    [SerializeField] private string _taskÑontent;
    [Space]
    [SerializeField] private string _correctAnswer;

    public int Id { get; set; }
    public string TextContent => _taskÑontent;
    public string CorrectAnswer => _correctAnswer;
}
