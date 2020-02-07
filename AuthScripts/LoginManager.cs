using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using TMPro;
using UnityEngine.SceneManagement;


public class LoginManager : MonoBehaviour
{    
    
    #region Private Variables
    [SerializeField] private string LoadLevel = "Game";
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private GoogleSignInConfiguration config;
    
    //the client id string ends with "...googleusercontent.com"
    private string clientId = "enter your client auth id (type3) from googleservices.json file from firebase console.";
    public static bool GSignIn ;
    
    
    
    
    #endregion
   
   

   void Start()
    { 
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            { 

                InitFireBase();
            
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }

   void InitFireBase()
   {
       
       Debug.Log("we are ready to authenticate");
       GSignIn = false; 
       auth = FirebaseAuth.DefaultInstance;
       auth.StateChanged += AuthStateChanged;
       AuthStateChanged(this, null);

   }
   
   
   void OnDestroy()
   {
       if (auth != null)
           auth.StateChanged -= AuthStateChanged;

   }

   void AuthStateChanged(object sender, EventArgs e)
   {
    
        Debug.Log("listening for auth state changed");
       if (auth.CurrentUser != null)
        {  
            Debug.Log("Auth.currentUser"+ auth.CurrentUser.Email);

            SceneManager.LoadSceneAsync(LoadLevel);
        }

    }



   #region Signup
    public void signUp()
    {
        
        sign();
        


    }

   void  sign()
   {
       FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
       {

           if (task.IsCanceled)
           {
               Debug.Log("canceled");
               return;
           }

           if (task.IsFaulted)
           {
               Debug.Log("network issues");
           }
           
           
           //FIREBASE USER

            user = task.Result;
            Debug.LogFormat("created user {0}({1})",user.Email,user.UserId);

    });

   }
#endregion

    #region SignInTheUser



    public void SignInUser()
    {
       
        SignIn();


    }


    void SignIn()
    {
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email.text,                 password.text).ContinueWithOnMainThread(task =>
        {

            if (task.IsCanceled)
            {Debug.LogError(" sign in cancelled");
            }

            if (task.IsFaulted)
            {Debug.LogError("task is faulted");
            }

            user = task.Result;
            
            Debug.LogFormat("the user signed in is {0},{1}",user.Email , user.UserId);
          
              change();

        });

    void change()
    {
        SceneManager.LoadScene(LoadLevel);
        
        }

    }







    #endregion

//--------------------------------------


    #region GooogleSignIn

    public void SignInWithGoogle()
    {
    
        SignInG();
    }

    private void  SignInG()
    {
        config = new GoogleSignInConfiguration()
        {
            WebClientId = clientId,
            RequestIdToken = true,
            RequestEmail = true
        };

        GoogleSignIn.Configuration = config;
        GoogleSignIn.Configuration.RequestEmail = true;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.UseGameSignIn = false;
    
    
    
    
    
    
    
        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(OnAuth);
    
    // NOTE :: SignSilently method below signs your in with any account pop up UI , like in the normal SignIn above
    
    
    //to use this "SignInSilently" method ,COMMENT the above method and UNCOMMENT the below one.
    // GoogleSignIn.DefaultInstance.SignInSilently().ContinueWithOnMainThread(OnAuth);
    




    }



    void OnAuth(Task<GoogleSignInUser>task)
    {

        if (task.IsFaulted)     //when we click here , UI pops up ,and closes and wont open again unless we restart the app
                                            // needs to be worked on !
        {Debug.Log("network issues");
            config = null;
        }
        else if (task.IsCanceled)
        {    Debug.Log("did not select a profile");
            
        }

        else if (task.IsCompleted)
        {Debug.Log("succesfuly chosen ID");
            SignInWithFirebase(task.Result.IdToken);


        }


        void SignInWithFirebase(string token)
        {  
            var credential = Firebase.Auth.GoogleAuthProvider.GetCredential(token, null);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task1 =>
            {

                if (task1.IsCompleted)
                {Debug.LogFormat("Google sign in Success{0}",task1.Result.DisplayName);
                GSignIn = true;
                    
                    user = task1.Result;
                    // as soon as user gets assigned, the auth variable changes and control is shifted to the 
                    // AuthStateChanged above,which debugs and returns the controls back here
                    //so we can change scene here or when the control shifts

                    ChangeScene();
                }

             });


        }

        void ChangeScene()
        {
            SceneManager.LoadSceneAsync(LoadLevel);
        }


    }

    #endregion

    
    
    
    
   
}
