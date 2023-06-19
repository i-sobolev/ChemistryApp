using Assets.Assets.Scripts.API.Models;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private LeaderBoardUser _userTemplate;
    [SerializeField] private Transform _usersRoot;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        StartCoroutine(APIHelper.GetLeaders((result) =>
        {
            InstantiateUsers(result);
        }));
    }

    private void InstantiateUsers(IEnumerable<Leader> leaders)
    {
        if (_usersRoot.childCount != 0)
        {
            foreach (var button in _usersRoot.GetComponentsInChildren<LeaderBoardUser>())
                Destroy(button.gameObject);
        }

        var count = 0;

        foreach (var leader in leaders)
        {
            var user = Instantiate(_userTemplate, _usersRoot);

            user.SetData(count++, leader.userName, leader.completedTasksAmount);
        }
    }
}