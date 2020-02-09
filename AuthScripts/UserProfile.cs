using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserProfile
{
  
private static UserProfile user1 = new UserProfile();  //singleton variable

public  static UserProfile user
{
    get { return user1; }
    set
    {
        user1 = value;
    }

}



//creates a new user object to access the class objects 


    public string UserMessage;
    public   string UserName;
    public  string UserEmail;
    public Uri UserImageLink;
    public  string UserId;
    public string newText; 
   
//ok
    
    
    public UserProfile()
    { 
        
        
        
        
    }


    public UserProfile(string UserName,string UserEmail, string UserMessage, string UserId , string newText )
    {
    

        this.UserName = UserName;
        this.UserEmail = UserEmail;
        this.UserMessage = UserMessage;
        this.UserId = UserId;
        this.newText = newText;


    }

    
   
}
