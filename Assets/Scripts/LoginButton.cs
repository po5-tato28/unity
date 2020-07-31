using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class LoginButton : MonoBehaviour
{
    [SerializeField] private Button _loginButton;
    [SerializeField] private InputField _emailField;
    [SerializeField] private InputField _passwordField;

    private Coroutine _loginCoroutine;

    public UserLoginEvent OnUserLoginSucceeded = new UserLoginEvent();
    public UserLoginFailedEvent OnLoginFailed = new UserLoginFailedEvent();

    private void Reset()
    {
        _loginButton = GetComponent<Button>();
    }

    private void Start()
    {
        _emailField.onValueChanged.AddListener(HandleValueChanged);
        _passwordField.onValueChanged.AddListener(HandleValueChanged);
        _loginButton.onClick.AddListener(HandleButtonClicked);
    }

    private void HandleValueChanged(string _)
    {
        UpdateButtonInteracable();
    }

    private void OnDestroy()
    {
        _loginButton.onClick.RemoveListener(HandleButtonClicked);
        _emailField.onValueChanged.AddListener(HandleValueChanged);
        _passwordField.onValueChanged.AddListener(HandleValueChanged);
    }

    private void UpdateButtonInteracable()
    {
        _loginButton.interactable = _loginCoroutine == null
            && !string.IsNullOrEmpty(_emailField.text)
            && !string.IsNullOrEmpty(_passwordField.text);
    }

    private void HandleButtonClicked()
    {
        if (_loginCoroutine == null)
        {
            _loginCoroutine = StartCoroutine(LoginCoroutine(_emailField.text, _passwordField.text));
            UpdateButtonInteracable();
        }
    }

    private IEnumerator LoginCoroutine(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogWarning($"Login failed with {loginTask.Exception}");
            OnLoginFailed.Invoke(loginTask.Exception);
        }
        else
        {
            Debug.Log($"Login scucceeded with {loginTask.Result.Email}");
            OnUserLoginSucceeded.Invoke(loginTask.Result);
        }

        _loginCoroutine = null;
        UpdateButtonInteracable();
    }


    [System.Serializable]
    public class UserLoginEvent : UnityEvent<FirebaseUser>
    {
    }

    [System.Serializable]
    public class UserLoginFailedEvent : UnityEvent<FirebaseUser>
    {
        internal void Invoke(AggregateException exception)
        {
            throw new NotImplementedException();
        }
    }
}