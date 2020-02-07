using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSignout : MonoBehaviour
{
    private FirebaseAuth auth;
    private string LoadLevel = "LoginScene";
    private FirebaseUser User;
    // Start is called before the first frame update
    void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        User = auth.CurrentUser;


        init();
    }


    void init()
    {


        auth.StateChanged += SignOutChanged;
    }


    private void OnDestroy()
    {
        auth.StateChanged -= SignOutChanged;
    }

    void SignOutChanged(object sender, EventArgs e)
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null      )
        {
            // GoogleSignIn.DefaultInstance.SignOut();
            Debug.Log("signing out");
            SceneManager.LoadScene(LoadLevel);
        }


    }

}
