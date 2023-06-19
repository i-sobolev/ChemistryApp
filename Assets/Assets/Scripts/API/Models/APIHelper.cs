using Assets.Assets.Scripts.API.Models;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class APIHelper
{
    private const string _apiUri = "https://localhost:7164/api";
    private static string _token = string.Empty;

    public static IEnumerator Register(User user)
    {
        string json = JsonUtility.ToJson(user);

        var webRequest = new UnityWebRequest(_apiUri + "/User/Create", "POST");

        var post = Encoding.UTF8.GetBytes(json);

        webRequest.uploadHandler = new UploadHandlerRaw(post);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-type", "application/json");

        yield return webRequest.SendWebRequest();
    }

    public static IEnumerator Login(string login, string password, Action onSuccess, Action onError)
    {
        var webRequest = UnityWebRequest.Get(_apiUri + $"/User/Authenticate?login={login}&password={password}");
        webRequest.SetRequestHeader("Content-type", "application/json");

        yield return webRequest.SendWebRequest();

        while (!webRequest.isDone)
            yield return null;

        byte[] result = webRequest.downloadHandler.data;

        if (result != null && webRequest.responseCode == 200)
        {
            _token = Encoding.UTF8.GetString(result);
            onSuccess?.Invoke();
        }
        else
        {
            onError?.Invoke();
        }
    }

    public static IEnumerator GetUser(Action<User> onSuccess)
    {
        var webRequest = UnityWebRequest.Get(_apiUri + $"/User/GetInfo");
        webRequest.SetRequestHeader("Content-type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

        yield return webRequest.SendWebRequest();

        while (!webRequest.isDone)
            yield return null;

        byte[] result = webRequest.downloadHandler.data;

        if (result != null && webRequest.responseCode == 200)
        {
            Debug.Log($"Loaded user: {Encoding.UTF8.GetString(result)}");

            var user = JsonUtility.FromJson<User>(Encoding.UTF8.GetString(result));
            onSuccess?.Invoke(user);
        }
    }

    public static IEnumerator AddChapter(int chapterId)
    {
        var webRequest = new UnityWebRequest(_apiUri + $"/Chapters/Add?completedChapterId={chapterId}", "POST");
        webRequest.SetRequestHeader("Content-type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

        Debug.Log($"Adding completed chapter: {chapterId}");

        yield return webRequest.SendWebRequest();
    }

    public static IEnumerator AddFriend(int friendId)
    {
        var webRequest = new UnityWebRequest(_apiUri + $"/Friends/Add?friendId={friendId}", "POST");
        webRequest.SetRequestHeader("Content-type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

        Debug.Log($"Adding completed chapter: {friendId}");

        yield return webRequest.SendWebRequest();
    }

    public static IEnumerator AddTask(int taskId)
    {
        var webRequest = new UnityWebRequest(_apiUri + $"/Tasks/Add?completedTaskId={taskId}", "POST");
        webRequest.SetRequestHeader("Content-type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

        yield return webRequest.SendWebRequest();
    }

    public static IEnumerator GetFriends(Action<string[]> onSuccess, Action onError)
    {
        var webRequest = UnityWebRequest.Get(_apiUri + $"/Friends/Get");
        webRequest.SetRequestHeader("Content-type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

        yield return webRequest.SendWebRequest();

        while (!webRequest.isDone)
            yield return null;

        byte[] result = webRequest.downloadHandler.data;

        if (result != null && webRequest.responseCode == 200)
        {
            Debug.Log($"Loaded friends list: {Encoding.UTF8.GetString(result)}");

            var json = Encoding.UTF8.GetString(result);

            JsonArrayHelper.FixJson(ref json);

            var friends = JsonArrayHelper.FromJson<string>(json);
            onSuccess?.Invoke(friends);
        }
        else
        {
            onError?.Invoke();
        }
    }

    public static IEnumerator GetCompletedChapters(Action<int[]> onSuccess, Action onError)
    {
        var webRequest = UnityWebRequest.Get(_apiUri + $"/Chapters/GetCompletedChapters");
        webRequest.SetRequestHeader("Content-type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

        yield return webRequest.SendWebRequest();

        while (!webRequest.isDone)
            yield return null;

        byte[] result = webRequest.downloadHandler.data;

        if (result != null && webRequest.responseCode == 200)
        {
            var json = Encoding.UTF8.GetString(result);

            JsonArrayHelper.FixJson(ref json);

            Debug.Log($"Loaded completed chapters: {json}");

            var chapters = JsonArrayHelper.FromJson<int>(json);
            onSuccess?.Invoke(chapters);
        }
        else
        {
            onError?.Invoke();
        }
    }

    public static IEnumerator GetCompletedTasks(Action<int[]> onSuccess, Action onError)
    {
        var webRequest = UnityWebRequest.Get(_apiUri + $"/Tasks/GetCompletedTasks");
        webRequest.SetRequestHeader("Content-type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

        yield return webRequest.SendWebRequest();

        while (!webRequest.isDone)
            yield return null;

        byte[] result = webRequest.downloadHandler.data;

        if (result != null && webRequest.responseCode == 200)
        {
            Debug.Log($"Loaded completed tasks: {Encoding.UTF8.GetString(result)}");

            string json = Encoding.UTF8.GetString(result);

            JsonArrayHelper.FixJson(ref json);

            var chapters = JsonArrayHelper.FromJson<int>(json);
            onSuccess?.Invoke(chapters);
        }
        else
        {
            onError?.Invoke();
        }
    }

    public static IEnumerator GetLeaders(Action<Leader[]> onSuccess)
    {
        var webRequest = UnityWebRequest.Get(_apiUri + $"/Tasks/GetLeaders");
        webRequest.SetRequestHeader("Content-type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

        yield return webRequest.SendWebRequest();

        while (!webRequest.isDone)
            yield return null;

        byte[] result = webRequest.downloadHandler.data;

        if (result != null && webRequest.responseCode == 200)
        {
            Debug.Log($"Loaded user: {Encoding.UTF8.GetString(result)}");

            string json = Encoding.UTF8.GetString(result);

            JsonArrayHelper.FixJson(ref json);

            var leaders = JsonArrayHelper.FromJson<Leader>(json);

            foreach (var leader in leaders)
            {
                Debug.Log(leader.userName);
            }
            onSuccess?.Invoke(leaders);
        }
    }

    public static IEnumerator GetOtherUsers(Action<User[]> onSuccess)
    {
        var webRequest = UnityWebRequest.Get(_apiUri + $"/User/GetOtherUsers");
        webRequest.SetRequestHeader("Content-type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

        yield return webRequest.SendWebRequest();

        while (!webRequest.isDone)
            yield return null;

        byte[] result = webRequest.downloadHandler.data;

        if (result != null && webRequest.responseCode == 200)
        {
            Debug.Log($"Loaded other users in system: {Encoding.UTF8.GetString(result)}");

            string json = Encoding.UTF8.GetString(result);

            JsonArrayHelper.FixJson(ref json);

            var otherUsers = JsonArrayHelper.FromJson<User>(json);
            onSuccess?.Invoke(otherUsers);
        }
    }
}