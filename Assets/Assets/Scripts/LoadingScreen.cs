using UnityEngine;

public class LoadingScreen : Singleton<LoadingScreen>
{
    [SerializeField] private GameObject _screenRoot;

    public void SetActive(bool value) => _screenRoot.gameObject.SetActive(value);
}
