using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TheoryView : MonoBehaviour
{
    [SerializeField] private TheoryConfig _config;
    [Space]
    [Header("Layouts")]
    [Header("Theory list")]
    [SerializeField] private GameObject _theoryListLayout;
    [Space]
    [SerializeField] private TheoryChapterButton _theoryButtonTemplate;
    [SerializeField] private Transform _buttonsRoot;
    [Header("Theory content")]
    [SerializeField] private GameObject _theoryContentLayout;
    [Space]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _contentText;

    private IEnumerable<TheoryChapter> _currentChapters = null;

    public void InstantiateTheoryButtons(IEnumerable<TheoryChapter> theoryChapters)
    {
        if (_buttonsRoot.childCount != 0)
        {
            foreach (var button in _buttonsRoot.GetComponentsInChildren<TheoryChapterButton>())
                Destroy(button.gameObject);
        }

        LoadingScreen.Instance.SetActive(true);

        _currentChapters = theoryChapters;

        StartCoroutine(
            APIHelper.GetCompletedChapters(
                onSuccess: (result) =>
                {
                    var completedTheory = result != null ? result.ToList() : new List<int>();

                    foreach (var chapter in theoryChapters)
                    {
                        var button = Instantiate(_theoryButtonTemplate, _buttonsRoot);

                        button.Set(chapter.Title, completedTheory.Contains(chapter.Id));
                        button.LinkedTheoryChapter = chapter;
                        button.Clicked += OnTheoryButtonClicked;
                    }

                    LoadingScreen.Instance.SetActive(false);
                },
                onError: () => { }));
    }

    public void RefreshCurrentButtons()
    {
        InstantiateTheoryButtons(_currentChapters);
    }

    public void ShowOrganicChemistryChaptersButtons()
    {
        InstantiateTheoryButtons(_config.OrganicChemistry);
    }

    public void ShowInorganicChemistryChaptersButtons()
    {
        InstantiateTheoryButtons(_config.NonorganicChemistry);
    }

    private void OnTheoryButtonClicked(ButtonWithCompletionMarker clickedButton)
    {
        var theoryButton = clickedButton as TheoryChapterButton;

        ShowTheoryContent(theoryButton.LinkedTheoryChapter);
    }

    private void ShowTheoryContent(TheoryChapter theoryChapter)
    {
        _titleText.SetText(theoryChapter.Title);
        _contentText.SetText(theoryChapter.TextContent);

        _theoryListLayout.SetActive(false);
        _theoryContentLayout.SetActive(true);

        StartCoroutine(APIHelper.AddChapter(theoryChapter.Id));
    }
}
