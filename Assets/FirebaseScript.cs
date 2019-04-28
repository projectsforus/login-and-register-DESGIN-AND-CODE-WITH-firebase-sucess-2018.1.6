using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FirebaseScript : MonoBehaviour
{
    public InputField usernameInput, passowrdInput, emailInput;
    public InputField usernameInputLogin, passowrdInputLogin;
    public Text logintxt,regtext;
    public GameObject Reg, Log;
    DatabaseReference reference;

    // Start is called before the first frame update
    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://invertible-now-124909.firebaseio.com/");

        // Get the root reference location of the database.
     reference = FirebaseDatabase.DefaultInstance.RootReference;

      

       // 
    }

    

        public void OnClickRigester()
    {
        saveDataAsJson();
        regtext.text = "success";
       // launchApp();
    }

    public void OnClickLogin()
    {
        retrieveData();

    }

    public void OnClickSwitchRegister()
    {
        Reg.SetActive(true);
        Log.SetActive(false);
    }

    public void OnClickSwitchLog()
    {
        Reg.SetActive(false);
        Log.SetActive(true);
    }


    private string saveDataAsJson()
    {
        User user = new User(usernameInput.text,passowrdInput.text,emailInput.text);
        string json = JsonUtility.ToJson(user);

        // you can use it like this too
        // string json = "{\"name\":\"Lama\",\"city\":\"Jeddah\"}";

        reference.Child("users").Push().SetRawJsonValueAsync(json);

        return "Save data to firebase Done.";
    }





    private void retrieveData()
    {


        reference.Child("users").GetValueAsync().ContinueWith(task =>
        {
            //
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;


                foreach (DataSnapshot user in snapshot.Children)
                {
                    IDictionary dictUser = (IDictionary)user.Value;

                    if(usernameInputLogin.text.ToString()== dictUser["name"].ToString()&&passowrdInputLogin.text.ToString()== dictUser["password"].ToString())
                    {
                        logintxt.text = "success";
                       // launchApp();
                    }
                    else
                    {
                        logintxt.text = "failure";
                    }
                    Debug.Log("" + dictUser["email"] + " - " + dictUser["password"]);

                


                }
            }
        });
        

    }

    public void launchApp()
    {

        bool fail = false;
        string bundleId = "com.gdv.sadf"; // your target bundle id
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
        }
        catch (System.Exception e)
        {
            fail = true;
        }

        if (fail)
        { //open app in store
          //   Application.OpenURL("https://google.com");
        }
        else //open the app
            ca.Call("startActivity", launchIntent);

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();


    }


}



public class User
{
    public string name;
    public string password;
    public string email;

    public User()
    {
    }

    public User(string name, string password, string email)
    {
        this.name = name;
        this.password = password;
        this.email = email;

    }



}