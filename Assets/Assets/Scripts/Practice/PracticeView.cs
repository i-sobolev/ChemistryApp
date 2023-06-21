using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PracticeView : MonoBehaviour
{
    [SerializeField] private PracticeConfig _config;
    [Space]
    [Header("Layouts")]
    [Header("Practice chapters list")]
    [SerializeField] private GameObject _practiceButtonsListLayout;
    [Space]
    [SerializeField] private PracticeChapterButton _practiceButtonTemplate;
    [SerializeField] private Transform _practiceButtonsRoot;
    [Header("Tasks list")]
    [SerializeField] private GameObject _tasksListLayout;
    [Space]
    [SerializeField] private PracticeTaskButton _taskButtonTemplate;
    [SerializeField] private Transform _taskButtonsRoot;
    [Header("Task content")]
    [SerializeField] private GameObject _taskContentLayout;
    [Space]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _contentText;
    [Space]
    [SerializeField] private TMP_InputField _answerText;
    [SerializeField] private Color _correctAnswerColor;
    [SerializeField] private Color _incorrectAnswerColor;
    [SerializeField] private Image _inputBackground;
    [SerializeField] private Button _answerButton;

    private PracticeTask _currentTask = null;

    private PracticeChapter _currentChapter = null;

    private void OnEnable()
    {
        InstantiatePracticeChapterButtons(_config.PracticeChapters);
    }

    public void RefreshCurrentTasks() => InstantiatePracticeTaskButtons(_currentChapter);
    public void RefreshChapters() => InstantiatePracticeChapterButtons(_config.PracticeChapters);

    public void InstantiatePracticeChapterButtons(IEnumerable<PracticeChapter> practiceChapters)
    {
        if (_practiceButtonsRoot.childCount != 0)
        {
            foreach (var button in _practiceButtonsRoot.GetComponentsInChildren<PracticeChapterButton>())
                Destroy(button.gameObject);
        }

        LoadingScreen.Instance.SetActive(true);

        StartCoroutine(APIHelper.GetCompletedTasks((result) =>
        {
            var completedTasks = result.ToList();

            foreach (var chapter in practiceChapters)
            {
                var button = Instantiate(_practiceButtonTemplate, _practiceButtonsRoot);

                button.Set(chapter.Title, false);
                button.LinkedPracticeChapter = chapter;
                button.Clicked += OnPracticeChapterButtonClicked;

                button.SetTaskProgress(
                    completed: chapter.Tasks.Count(x => completedTasks.Contains(x.Id)), 
                    all: chapter.Tasks.Count());
            }

            LoadingScreen.Instance.SetActive(false);
        },
        onError: () => { }));
    }

    private void InstantiatePracticeTaskButtons(PracticeChapter practiceChapter)
    {
        if (_practiceButtonsRoot.childCount != 0)
        {
            foreach (var button in _taskButtonsRoot.GetComponentsInChildren<PracticeTaskButton>())
                Destroy(button.gameObject);
        }

        _currentChapter = practiceChapter;

        LoadingScreen.Instance.SetActive(true);

        StartCoroutine(APIHelper.GetCompletedTasks(
            onSuccess: (result) =>
            {
                var completedTasks = result.ToList();

                var count = 1;

                foreach (var task in practiceChapter.Tasks)
                {
                    var button = Instantiate(_taskButtonTemplate, _taskButtonsRoot);

                    var taskName = $"Задание №{count++}";
                    task.Name = taskName;

                    button.Set(taskName, completedTasks.Contains(task.Id));
                    button.LinkedPracticeTask = task;
                    button.Clicked += OnPracticeTaskButtonClicked;
                }

                LoadingScreen.Instance.SetActive(false);
            },
            onError: () => { }));
    }

    private void OnPracticeChapterButtonClicked(ButtonWithCompletionMarker clickedButton)
    {
        var theoryButton = clickedButton as PracticeChapterButton;

        InstantiatePracticeTaskButtons(theoryButton.LinkedPracticeChapter);

        _tasksListLayout.SetActive(true);
        _practiceButtonsListLayout.SetActive(false);
    }

    private void OnPracticeTaskButtonClicked(ButtonWithCompletionMarker clickedButton)
    {
        var taskButton = clickedButton as PracticeTaskButton;

        ShowTaskContent(taskButton.LinkedPracticeTask);
    }

    private void ShowTaskContent(PracticeTask practiceTask)
    {
        _currentTask = practiceTask;

        _titleText.SetText(practiceTask.Name);
        _contentText.SetText(practiceTask.TextContent);

        _tasksListLayout.SetActive(false);
        _taskContentLayout.SetActive(true);

        StartCoroutine(APIHelper.GetCompletedTasks(
            onSuccess: (result) =>
            {
                var completedTasks = result.ToList();

                SetCompletedView(completedTasks.Contains(practiceTask.Id));
            },
            onError: () => { }));
    }

    private void SetCompletedView(bool completed)
    {
        _answerButton.gameObject.SetActive(!completed);
        _answerText.enabled = !completed;
        _inputBackground.raycastTarget = !completed;
        _inputBackground.color = completed ? _correctAnswerColor : _incorrectAnswerColor;

        if (completed)
            _answerText.text = _currentTask.CorrectAnswer;
    }

    public void Answer()
    {
        var answer = _answerText.text;

        SetCompletedView(_currentTask.CorrectAnswer == answer);

        Debug.Log($"Answer given: {_currentTask.CorrectAnswer} == {answer} = {_currentTask.CorrectAnswer == answer}");

        StartCoroutine(APIHelper.AddTask(_currentTask.Id));
    }
}