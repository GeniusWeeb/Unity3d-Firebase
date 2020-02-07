using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Auth;
using Firebase;
using Google;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
   [SerializeField] private string LoadLevel = "LoginScene";
    private FirebaseAuth auth;
    private FirebaseUser user;
    private GoogleSignIn gso;
    [SerializeField] private TextMeshProUGUI email;

    [SerializeField] private TextMeshProUGUI name;
    // Start is called before the first frame update
    void Awake()
    {    
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        if(LoginManager.GSignIn)
        {gso = GoogleSignIn.DefaultInstance;}

        name.text = user.DisplayName;
        
        Debug.Log(user.DisplayName);
        email.text = user.Email;
    

    }



  public   void SignOut()
  {    
        Debug.Log("sign out from Game manager");
        if (LoginManager.GSignIn)
        {    Debug.Log("yay ! g Sign out");
            gso.SignOut();
            
        }
        auth.SignOut();
        

        // SceneManager.LoadSceneAsync(LoadLevel);

    }



    // Update is called once per frame
        
}
