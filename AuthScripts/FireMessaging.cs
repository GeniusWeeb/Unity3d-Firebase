using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Messaging;
using TMPro;
using UnityEngine;

public class FireMessaging : MonoBehaviour
{
    public TextMeshProUGUI toggle;
    private string topic = "TestTopic";
    // Start is called before the first frame update



    protected  bool LogTaskCompletion(Task task, string operation)
    {
        bool complete = false;
    
        if(task.IsCanceled)
            Debug.Log("MCancelled");
        else if(task.IsFaulted)
            Debug.Log("MFaulted");
        else if (task.IsCompleted)
        {
            Debug.Log(operation+" completed");
            complete = true;
        }


        return complete;
    }

    void Start()
    {   
             
           //ss Debug.Log(LoginManager.IsReady);
            Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenRecieved;
            Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

            FirebaseMessaging.SubscribeAsync(topic).ContinueWithOnMainThread(task =>
                {
                    
                    
                    LogTaskCompletion(task,"SubscribeAsync");
                }

            );
            
            Debug.Log("Firebase Messaging");
            toggle.text = FirebaseMessaging.TokenRegistrationOnInitEnabled.ToString();
            FirebaseMessaging.DeliveryMetricsExportedToBigQueryEnabled = true;


            FirebaseMessaging.RequestPermissionAsync().ContinueWithOnMainThread(
                task =>
                {
                    LogTaskCompletion(task, "RequestPermissionAsync"); 
                    
                });
            
            
            Debug.Log(LoginManager.IsReady);
            LoginManager.IsReady = true;
          

    }

//click to subscribe
    public void subscribe()
    {
        
        Firebase.Messaging.FirebaseMessaging.SubscribeAsync(topic).ContinueWithOnMainThread(
            task => {
                LogTaskCompletion(task, "SubscribeAsync");
            }
        );
        //ToggleTokenOnInit();
       // var val = FirebaseMessaging.TokenRegistrationOnInitEnabled;
      // toggle.text = val.ToString();
       //    Debug.Log("Subscribed to " + topic);

    }




    //goes on a button //Toggle
    public void ToggleTokenOnInit()
    {
            
         var newVal = !FirebaseMessaging.TokenRegistrationOnInitEnabled;
        
        Firebase.Messaging.FirebaseMessaging.TokenRegistrationOnInitEnabled = newVal;
        Debug.Log("Token Enabled " + newVal);
        toggle.text = newVal.ToString();
    }





    // Update is called once per frame

    public virtual void OnTokenRecieved(object sender , Firebase.Messaging.TokenReceivedEventArgs token) 
    {
        
        
     UnityEngine.Debug.Log("recieving a registration token" +  token.Token);
    }
    
    public void OnMessageReceived(object sender , Firebase.Messaging.MessageReceivedEventArgs e) 
    {
        
        
    // UnityEngine.Debug.Log("recieving a message " +  message.Message.From);
    Debug.Log("Received a new message");
   var sound =  e.Message.Notification.Sound;
   if (e.Message.Notification != null) {
        
        Debug.Log("title: " + e.Message.Notification.Title);
        Debug.Log("body: " + e.Message.Notification.Body);
        var android = e.Message.Notification.Android;
        if (android != null) {
            Debug.Log("android channel_id: " + android.ChannelId);
           
        }
    }
    if (e.Message.From.Length > 0)
        Debug.Log("from: " + e.Message.From);
     
    if(e.Message.NotificationOpened)
        Debug.Log("opened notification");
    
    
        
        if (e.Message.Link != null) 
        {
        Debug.Log("link: " + e.Message.Link.ToString());
          }
    if (e.Message.Data.Count > 0) {
        Debug.Log("data:");
        foreach (System.Collections.Generic.KeyValuePair<string, string> iter in
            e.Message.Data) {
            Debug.Log("  " + iter.Key + ": " + iter.Value);
        }
    }
    }

    
    
    
    
    
    
    
    public void OnDestroy() {
        FirebaseMessaging.MessageReceived -= OnMessageReceived;
        FirebaseMessaging.TokenReceived -= OnTokenRecieved;
        
    }
    
}



