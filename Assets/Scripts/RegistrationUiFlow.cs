using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RegistrationUiFlow : MonoBehaviour
{
	[SerializeField] private InputField _emailField;
	[SerializeField] private InputField _passwordField;
	[SerializeField] private InputField _verifyPasswordField;
	
	public State CurrentState {get; private set;}
	public StateChangedEvent OnStateChanged = new StateChangedEvent();
	
	public string Email => _emailField.text;
	public string Password => _passwordField.text;
	
	private void Start()
	{
		_emailField.onValueChanged.AddListener(HandleValueChanged);
		_passwordField.onValueChanged.AddListener(HandleValueChanged);
		_verifyPasswordField.onValueChanged.AddListener(HandleValueChanged);
		ComputeState();
	}
	private void HandleValueChanged (string _string)
	{
		ComputeState();
	}
	
	private void ComputeState()
	{
		if(string.IsNullOrEmpty(_emailField.text))
		{
			SetState(State.EnterEmail);
		}
		else if(string.IsNullOrEmpty(_passwordField.text))
		{
			SetState(State.EnterPassword);
		}
		else if(_passwordField.text != _verifyPasswordField.text)
		{
			SetState(State.PasswordsDontMatch);
		}
		else
		{
			SetState(State.Ok);
		}
	}
	
	private void SetState(State state)
	{
		CurrentState = state;
		OnStateChanged.Invoke(state);
	}
	
	public enum State{
		EnterEmail,
		EnterPassword,
		PasswordsDontMatch,
		Ok
	}
	
	[System.Serializable]
	public class StateChangedEvent : UnityEvent<State>
	{
	}
}