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



//----------------------------
    private void Awake()
    {    
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        DontDestroyOnLoad(this.gameObject);
        
    }

    private void Start()
    {
       
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("your firebase databse url here");
        
        
        //getting the database root reference i.e for ex  if ur url is : "...firebase.com/_root_"
        //_root_ is the reference location we are assigning to this variable , then below we define 
        //AppUsers at root
        dRef = FirebaseDatabase.DefaultInstance.RootReference;
        

        InitUser();
    }


    void InitUser()
    {    // storing the data from online authenetication object to a static UserProfile.cs file
        //instead of using the auth object everywhere we would just use the UserProfile class members
    
    
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
        
        Debug.Log(UserProfile.user.UserMessage);
        Debug.Log(UserProfile.user.UserEmail);
     



         // perform Firebase functions here

         string json = JsonUtility.ToJson(UserProfile.user);
        
        
        //AppUsers is the RootNode 
        //+AppUsers---
         //  +UserId(UID)
          //     +UserName:
          //     +UserEmail:
          //     +UserId:
          //     +UserPassword:
          
          // the below line creates the format as above from the "UserProfile.cs" Script, as we convert 
          // the Script (C# object) into Json and send it to firebase.
        
         dRef.Child("AppUsers").Child(user.UserId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
         {
             if (task.IsCompleted)
             {    //here we can give a pop up to the users for successful updation or some sort of things
                 Debug.Log("values have been sent");
             }

         });

        //SetRawJsonValueAsync overwritescomplete nodes 
        //SetValueAsync updates a specific field without overwritting the entire object as shown below




        }

     public void Update_Name()
    {
        UserProfile.user.UserName = Name.text;
        dRef.Child("AppUsers").Child(user.UserId).Child("UserName").SetValueAsync(UserProfile.user.UserName);
                                                    //it goes into the field "UserName" as sets it value to what we provide through
                                                    //the game

    }
    
    //go Back
    public void BackScene()
    {
        SceneManager.LoadSceneAsync(BackLevel);
    }

    //go and view profile
    public void check_profile()
    {

        SceneManager.LoadSceneAsync(NextLevel);
    }
    
    
    
    

}
