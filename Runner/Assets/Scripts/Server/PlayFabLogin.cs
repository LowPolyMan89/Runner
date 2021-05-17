using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private string Username = "DonDrakone";
    [SerializeField] private string Password = "kaQuetcu";
    [SerializeField] private bool isUsername = false;
    [SerializeField] private bool isPassword = false;

    private IEnumerator GetUsername()
    {
        yield return new WaitForSeconds(0.1f);

        if(!string.IsNullOrEmpty(Username))
        {
            isUsername = true;
        }
        else
            StartCoroutine(GetUsername());
    }

    private IEnumerator GetPassword()
    {
        yield return new WaitForSeconds(0.1f);

        if (!string.IsNullOrEmpty(Password))
        {
            isPassword = true;
        }
        else
            StartCoroutine(GetPassword());
    }

    private IEnumerator Auth()
    {
        yield return new WaitForSeconds(0.1f);

        if (isUsername && isPassword)
        {
            Login();
        }
        else
            StartCoroutine(Auth());
    }

    private void Login()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = "B0011";
        }
        var request = new LoginWithEmailAddressRequest { Email = Username, Password = Password };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    [ContextMenu("StartPlayFabLogin")]
    public void StartPlayFabLogin()
    {
        StartCoroutine(GetUsername());
        StartCoroutine(GetPassword());
        StartCoroutine(Auth());
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Play Fab LogIn!");
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Redister complited!");
        var request = new LoginWithEmailAddressRequest { Email = Username, Password = Password };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnRegisterFail(PlayFabError error)
    {
        Debug.Log("Registration is fail!");
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());

        if(error.ErrorMessage == "User not found")
        {
            var request = new RegisterPlayFabUserRequest { Email = Username, Password = Password, RequireBothUsernameAndEmail = false };
            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFail);
        }
    }
}