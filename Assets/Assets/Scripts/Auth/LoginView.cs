using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LoginView : MonoBehaviour
{
    public UnityEvent OnSucces;

    [SerializeField] private TMP_InputField _loginInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [Space]
    [SerializeField] private TextMeshProUGUI _warning;

    public void Login()
    {
        Debug.Log($"Login data: login - {_loginInput.text}; password - {_passwordInput.text}");

        LoadingScreen.Instance.SetActive(true);

        StartCoroutine(APIHelper.Login(
            login: _loginInput.text,
            password: _passwordInput.text,
            onSuccess: () =>
            {
                OnSucces?.Invoke();
                LoadingScreen.Instance.SetActive(false);

            },
            onError: () =>
            {
                ShowWarning("Что-то пошло не так");
                LoadingScreen.Instance.SetActive(false);
            }));
    }

    public void ShowWarning(string message)
    {
        _warning.text = message;
    }
}