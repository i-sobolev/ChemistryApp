using TMPro;
using UnityEngine;

public class RegistrationView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameInput;
    [SerializeField] private TMP_InputField _loginInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private TMP_InputField _passwordConfirmInput;
    [Space]
    [SerializeField] private TextMeshProUGUI _warning;

    public void Register()
    {
        if (_passwordInput.text != _passwordConfirmInput.text)
            ShowWarning("Пароли не совпадают");

        Debug.Log($"Login data: username - {_usernameInput.text}; login - {_loginInput.text}; password - {_passwordInput.text}; passwordConfirm - {_passwordInput.text};");

        StartCoroutine(APIHelper.Register(new User
        {
            name = _usernameInput.text,
            login = _loginInput.text,
            password = _passwordInput.text
        }));
    }

    public void ShowWarning(string message)
    {
        _warning.text = message;
    }
}
