using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class RegistrationButton : MonoBehaviour
{
    [SerializeField] private RegistrationUiFlow _registrationFlow;
    [SerializeField] private Button _registrationButton;

    private Coroutine _registrationCoroutine;

    public UserRegisteredEvent OnUserRegistered = new UserRegisteredEvent();
    public UserRegistrationFailedEvent OnUserRegistrationFailed = new UserRegistrationFailedEvent();

    private void Reset()
    {
        _registrationFlow = FindObjectOfType<RegistrationUiFlow>();
        _registrationButton = GetComponent<Button>();
    }

    private void Start()
    {
        _registrationFlow.OnStateChanged.AddListener(HandleRegistrationStateChanged);
        _registrationButton.onClick.AddListener(HandleRegistrationButtonClicked);

        UpdateInteractable();
    }

    private void OnDestroy()
    {
        _registrationFlow.OnStateChanged.RemoveListener(HandleRegistrationStateChanged);
        _registrationButton.onClick.RemoveListener(HandleRegistrationButtonClicked);
    }

    private void UpdateInteractable()
    {
        _registrationButton.interactable = _registrationFlow.CurrentState == RegistrationUiFlow.State.Ok
            && _registrationCoroutine == null;
    }

    private void HandleRegistrationStateChanged(RegistrationUiFlow.State registrationState)
    {
        UpdateInteractable();
    }

    private void HandleRegistrationButtonClicked()
    {
        _registrationCoroutine = StartCoroutine(RegisterUser(_registrationFlow.Email, _registrationFlow.Password));
        UpdateInteractable();
    }

    private IEnumerator RegisterUser(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {registerTask.Exception}");
            OnUserRegistrationFailed.Invoke(registerTask.Exception);
        }
        else
        {
            Debug.Log($"Sucessfully registered user {registerTask.Result.Email}");
            OnUserRegistered.Invoke(registerTask.Result);
        }

        _registrationCoroutine = null;
        UpdateInteractable();
    }

    [System.Serializable]
    public class UserRegisteredEvent : UnityEvent<FirebaseUser>
    {
    }

    [System.Serializable]
    public class UserRegistrationFailedEvent : UnityEvent<FirebaseUser>
    {
        internal void Invoke(AggregateException exception)
        {
            throw new NotImplementedException();
        }
    }
}