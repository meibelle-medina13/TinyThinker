using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RESPONSE_CLASSES : MonoBehaviour
{
    // SIGN UP - LOGIN //
    [Serializable]
    public class Guardian_Data
    {
        public int ID { get; set; }
    }

    [Serializable]
    public class Guardian_Root
    {
        public bool success { get; set; }
        public List<Guardian_Data> data { get; set; }
    }

    public class VerificationRoot
    {
        public bool success { get; set; }
        public string data { get; set; }
    }


    // SELECT ACCOUNT //
    [Serializable]
    public class UserData
    {
        public int ID { get; set; }
        public string username { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public string avatar_filename { get; set; }
        public int current_theme { get; set; }
        public int current_level { get; set; }
        public string relation_to_guardian { get; set; }
    }

    public class UserRoot
    {
        public bool success { get; set; }
        public List<UserData> data { get; set; }
    }

    //GET LEVEL SCORES
    [Serializable]
    public class ScoreData
    {
        public int level_num { get; set; }
        public int scores { get; set; }
    }

    public class ScoreRoot
    {
        public bool success { get; set; }
        public List<ScoreData> data { get; set; }
    }

    //GET QUARTER/THEME STATUS
    [Serializable]
    public class QuarterData
    {
        public int theme_num { get; set; }
        public int status { get; set; }
    }

    public class QuarterRoot
    {
        public bool success { get; set; }
        public List<QuarterData> data { get; set; }
    }
}
