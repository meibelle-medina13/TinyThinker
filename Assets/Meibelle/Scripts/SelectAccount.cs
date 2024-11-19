using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static OptionSelection;


public class SelectAccount : MonoBehaviour
{
    //string URL = "http://localhost:3000/users";

    [SerializeField]
    private Sprite[] avatars = new Sprite[10];
    [SerializeField]
    private Sprite[] container = new Sprite[2];

    public GameObject user;
    string[] username;
    string[] gender;
    string[] avatar_filename;
    int[] user_id;
    int[] current_theme;
    int[] current_level;
    int no_user;
    

    private void Start()
    {
        Debug.Log(user.transform.position);
        StartCoroutine(getUsers());
    }

    void DisplayUsers(int num)
    {
        int y = 350;
        for (int i = 0; i < avatars.Length; i++)
        {
            Debug.Log(avatars[i].name);
            if (avatars[i].name == avatar_filename[0])
            {
                user.GetComponentInChildren<SpriteRenderer>().GetComponentInChildren<Canvas>().GetComponentInChildren<SpriteRenderer>().sprite = avatars[i];
                user.GetComponentInChildren<SpriteRenderer>().GetComponentInChildren<Canvas>().GetComponentInChildren<TMP_Text>().text = username[0].ToUpper();
                user.GetComponent<Button>().onClick.AddListener(() => LogIn(0));
            }
        }
        for (int i = 1; i < num; i++)
        {
            Debug.Log(i);
            GameObject Clone = Instantiate(user);
            y = y - 70;
            Clone.transform.position = new Vector3(394F, y, 0f);
            int index = i;

            for (int j = 0; j < avatars.Length; j++)
            {
                if (avatars[j].name == avatar_filename[i])
                {
                    Clone.GetComponentInChildren<SpriteRenderer>().GetComponentInChildren<Canvas>().GetComponentInChildren<SpriteRenderer>().sprite = avatars[j];
                    Clone.GetComponentInChildren<SpriteRenderer>().GetComponentInChildren<Canvas>().GetComponentInChildren<TMP_Text>().text = username[i].ToUpper();
                    Clone.GetComponent<Button>().onClick.AddListener(() => LogIn(index));
                }
            }
        }
    }




    [Serializable]
    public class UserData
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string gender { get; set; }
        public string avatar_filename { get; set; }
        public int current_theme { get; set; }
        public int current_level { get; set; }
    }

    public class UserRoot
    {
        public bool success { get; set; }
        public List<UserData> data { get; set; }
    }

    IEnumerator getUsers()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/users?guardian_ID=" + 1))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Received: " + www.downloadHandler.text);
                UserRoot json = JsonConvert.DeserializeObject<UserRoot>(www.downloadHandler.text);
                no_user = json.data.Count;

                //username.AddRange(json.data);
                username = new string[json.data.Count];
                gender = new string[json.data.Count];
                user_id = new int[json.data.Count];
                avatar_filename = new string[json.data.Count];
                current_theme = new int[json.data.Count];
                current_level = new int[json.data.Count];

                for (int i = 0; i <  json.data.Count; i++)
                {
                    username[i] = json.data[i].username;
                    gender[i] = json.data[i].gender;
                    user_id[i] = json.data[i].ID;
                    avatar_filename[i] = json.data[i].avatar_filename;
                    current_theme[i] = json.data[i].current_theme;
                    current_level[i] = json.data[i].current_level;
                }
                DisplayUsers(json.data.Count);
            }
        }
    }

    private void LogIn(int index)
    {
        PlayerPrefs.SetInt("Current_user", user_id[index]);
        PlayerPrefs.SetInt("Current_theme", current_theme[index]);
        PlayerPrefs.SetInt("Current_level", current_level[index]);

        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }
}
