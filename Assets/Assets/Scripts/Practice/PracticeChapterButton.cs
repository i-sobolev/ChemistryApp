using TMPro;
using UnityEngine;

public class PracticeChapterButton : ButtonWithCompletionMarker
{
    [SerializeField] private TextMeshProUGUI _tasksProgress;
    public PracticeChapter LinkedPracticeChapter { get; set; }

    public void SetTaskProgress(int completed, int all)
    {
        _tasksProgress.text = $"{completed}/{all}";
    }
}