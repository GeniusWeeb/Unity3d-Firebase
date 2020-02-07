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
    private string clientId = "901661532615-1l25n23p1ihk28r1ng5hd23d2q9vrcse.apps.googleusercontent.com";
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
    





                // Set a flag here tase is ready to use by your app.
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
        {            Debug.Log("Auth.currentUser"+ auth.CurrentUser.Email);

            SceneManager.LoadSceneAsync(LoadLevel);


       }



   }



   #region Signup
    public void signUp()
    {
        //var auth = FirebaseAuth.DefaultInstance;
        sign();
        


    }

   void  sign()
   {
       FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
       {

           if (task.IsCanceled)
           {Debug.Log("canceled");
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
        //ar auth = FirebaseAuth.DefaultInstance;
        SignIn();


    }


    void SignIn()
    {
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWithOnMainThread(task =>
        {

            if (task.IsCanceled)
            {Debug.LogError(" sign in cancelled");
            }

            if (task.IsFaulted)
            {Debug.LogError("task is faulted");
            }

            user = task.Result;
            
           // user = FirebaseAuth.DefaultInstance.CurrentUser; 

            Debug.LogFormat("the user signed in is {0},{1}",user.Email , user.UserId);
           // user  = FirebaseAuth.DefaultInstance.CurrentUser;
          //s  Debug.Log(user);
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
        // GoogleSignIn.DefaultInstance.SignInSilently().ContinueWithOnMainThread(OnAuth);
    




    }



    void OnAuth(Task<GoogleSignInUser>task)
    {

        if (task.IsFaulted)
        {Debug.Log("network issues");
            config = null;
        }
        else if (task.IsCanceled)
        {    Debug.Log("did not select a profile");
            //config = null;
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
