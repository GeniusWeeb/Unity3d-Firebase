using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Firebase.Unity.Editor;


public class EditManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    // UserProfile UserProfile.user = new UserProfile();   //creating object of user class
    
    
    #region private_variables

    private static EditManager Mrefold;

    public static EditManager Mref
    {
        get { return Mrefold; }
        set { Mrefold = value; }

    }  //this is the main reference to the 

    private FirebaseAuth auth;
    private FirebaseUser user;
    
    
   
    public  DatabaseReference dRef;  //database default reference


     [SerializeField] private TMP_InputField data;
     [SerializeField] private TMP_InputField Name;
    private string text;
    private string BackLevel = "Game";
    private string NextLevel = "Profile";

#endregion
    private void Awake()
    {    
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        DontDestroyOnLoad(this.gameObject);
        
    }

    private void Start()
    {
       
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://macbook-6af54.firebaseio.com/");
        
        dRef = FirebaseDatabase.DefaultInstance.RootReference;
        

        InitUser();
    }


    void InitUser()
    {    
        UserProfile.user.UserEmail = user.Email;
        UserProfile.user.UserName = user.DisplayName;
        UserProfile.user.UserId = user.UserId;
        UserProfile.user.UserImageLink = user.PhotoUrl;
        





    }


    public void Save()
        {
            
        Debug.Log("hi");
         text = data.text;
         UserProfile.user.UserMessage= text;
         //  Debug.Log( GameManagerScript.Gm.UserProfile.user.UserEmail);
        Debug.Log(UserProfile.user.UserMessage);
        Debug.Log(UserProfile.user.UserEmail);
       // Debug.Log("New User's email is " +UserProfile.user2.UserEmail);



         // perform Firebase functions here

         string json = JsonUtility.ToJson(UserProfile.user);

         dRef.Child("AppUsers").Child(user.UserId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
         {
             if (task.IsCompleted)
             {    //here we can give a pop up to the users for successful updation or some sort of things
                 Debug.Log("values have been sent");
             }

         });






        }

     public void Update_Name()
    {
        UserProfile.user.UserName = Name.text;
        dRef.Child("AppUsers").Child(user.UserId).Child("UserName").SetValueAsync(UserProfile.user.UserName);
   

    }


    public void BackScene()
    {
        SceneManager.LoadSceneAsync(BackLevel);
    }


    public void check_profile()
    {

        SceneManager.LoadSceneAsync(NextLevel);
    }
    
    
    
    

}
