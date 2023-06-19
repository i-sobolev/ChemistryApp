using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    [Header("Layouts")]
    [Header("Main")]
    [SerializeField] private TextMeshProUGUI _title;
    [Space]
    [SerializeField] private LeaderBoard _leaderBoard;
    [Header("UserPage")]
    [SerializeField] private GameObject _userPageLayout;
    [Space]
    [SerializeField] private TextMeshProUGUI _userPageUsername;
    [SerializeField] private TextMeshProUGUI _userPageId;
    [SerializeField] private TextMeshProUGUI _userPageCompletedTasks;
    [SerializeField] private TextMeshProUGUI _userPageCompletedTheory;
    [SerializeField] private TextMeshProUGUI _userPageFriendsList;
    [Header("Friends")]
    [SerializeField] private GameObject _friendsLayout;
    [Space]
    [SerializeField] private TextMeshProUGUI _friendsUserId;
    [SerializeField] private TMP_InputField _friendsFriendId;
    [SerializeField] private Button _friendsAddButton;
    [SerializeField] private GameObject _friendsWarningMessage;
    [Header("LeaderBoard")]
    [SerializeField] private GameObject _leaderBoardLayout;
    [Space]

    private User _currentUser;

    private void OnEnable()
    {
        LoadingScreen.Instance.SetActive(true);

        StartCoroutine(APIHelper.GetUser((user) =>
        {
            _currentUser = user;

            _title.text = $"Так держать, {_currentUser.name}!";

            _userPageUsername.text = _currentUser.name;
            _userPageId.text = $"#{_currentUser.id}";
            _userPageCompletedTasks.text = $"Задачи {_currentUser.completedTasksAmount}";
            _userPageCompletedTheory.text = $"Главы {_currentUser.completedChaptersAmount}";

            _friendsUserId.text = $"#{_currentUser.id}";

            LoadingScreen.Instance.SetActive(false);

            UpdateFriendsList();
        }));
    }

    public void AddFriend()
    {
        StartCoroutine(APIHelper.AddFriend(Convert.ToInt32(_friendsFriendId.text)));
        UpdateFriendsList();
    }

    private void UpdateFriendsList()
    {
        StartCoroutine(APIHelper.GetFriends(
            onSuccess: (result) =>
            {
                string allFriendsNames = string.Empty;

                foreach (var name in result)
                    allFriendsNames += name + "\n";

                _userPageFriendsList.text = allFriendsNames;
            },
            onError: () => { }));
    }
}