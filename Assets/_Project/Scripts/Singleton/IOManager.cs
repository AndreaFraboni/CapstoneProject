using System.IO;
using UnityEngine;

[System.Serializable]
public class AudioSettingData
{
    public float masterVolValue;
    public float musicVolValue;
    public float sfxVolValue;
}

//[System.Serializable]
//public class PlayerData
//{
//    public string Name;
//    public int Time;
//}
//[System.Serializable]
//public class HighscoreEntry
//{
//    public int time;
//    public string name;
//}

//[System.Serializable]
//public class Highscores
//{
//    public List<HighscoreEntry> highscoreEntryList;
//}

public class IOManager : GenericSingleton<IOManager>
{
    //public PlayerData mPlayerData = new PlayerData();
    public AudioSettingData mAudioSettings = new AudioSettingData();

    //public Highscores mHighscores = new Highscores();
    //public List<Transform> highscoreEntryTransformList;

    //private string _savePlayerFile;
    private string _saveAudioSettingsFile;

    private void Start()
    {
        //_savePlayerFile = Application.persistentDataPath + "/GameData.json";
        _saveAudioSettingsFile = Application.persistentDataPath + "/audiosettings.json";
    }

    //public void SetPlayerName(string name)
    //{
    //    mPlayerData.Name = name;
    //}
    //public void SetPlayerTime(int Playertime)
    //{
    //    mPlayerData.Time = Playertime;
    //    AddHighscoreEntry(mPlayerData.Time, mPlayerData.Name);
    //    if (SavePlayerDataFile())
    //    {
    //        Debug.LogWarning("PLAYER DATA SAVED !!");
    //    }
    //    else
    //    {
    //        Debug.LogError("PROBLEMA NEL SALVATAGGIO DEI DATI DEL PLAYER !!!");
    //    }
    //}
    //public void SetCoins(int num)
    //{
    //}    

    //******************************************************************************************//
    //********************* AUDIO SETTINGS LOAD & SAVE AUDIO DATA*******************************//
    //******************************************************************************************//
    public bool SaveAudioSettings(float masterVol, float musicVol, float sfxVol)
    {
        mAudioSettings.masterVolValue = masterVol;
        mAudioSettings.musicVolValue = musicVol;
        mAudioSettings.sfxVolValue = sfxVol;

        try
        {
            string json = JsonUtility.ToJson(mAudioSettings);
            File.WriteAllText(_saveAudioSettingsFile, json);
            Debug.Log("File di salvataggio Audio Settings è stato scritto in: " + _saveAudioSettingsFile);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Errore nel salvataggio degli Audio Settings: " + e.Message);
            return false;
        }
    }

    public bool LoadAudioSettings(ref float MasterVol, ref float MusicVol, ref float SFXVol)
    {
        AudioSettingData mAudioSettings = new AudioSettingData();

        if (!File.Exists(_saveAudioSettingsFile))
        {
            Debug.Log("Loading problem: il file json DELL'AUDIO SETTINGS non esiste.");
            return false;
        }

        try
        {
            string json = File.ReadAllText(_saveAudioSettingsFile);

            if (string.IsNullOrWhiteSpace(json))
            {
                Debug.LogWarning("file json dell'AUDIO SETTINGS è vuoto ????");
                return false;
            }

            mAudioSettings = JsonUtility.FromJson<AudioSettingData>(json);

            if (mAudioSettings == null)
            {
                Debug.LogWarning("problema : file json dei settaggi AUDIO in lettura non è valido !!!");
                return false;
            }
            else
            {
                MasterVol = mAudioSettings.masterVolValue;
                MusicVol = mAudioSettings.musicVolValue;
                SFXVol = mAudioSettings.sfxVolValue;
                return true;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Errore nel caricamento dei dati AUDIO per un errore : " + e.Message);
            return false;
        }

    }

    //******************************************************************************************//
    //*************************  PLAYER DATA LOAD & SAVE PLAYER DATA ***************************//
    //******************************************************************************************//
    //public bool LoadPlayerDataFile(ref string playerName, ref int playerScore)
    //{
    //    if (!File.Exists(_savePlayerFile))
    //    {
    //        Debug.Log("Player Data Loading problem: file json in lettura non esiste !!!");
    //        return false;
    //    }
    //    try
    //    {
    //        string jsonloadingtext = File.ReadAllText(_savePlayerFile);
    //        if (string.IsNullOrWhiteSpace(jsonloadingtext))
    //        {
    //            Debug.LogWarning("file json in lettura è un file vuoto ????");
    //            return false;
    //        }
    //        mPlayerData = JsonUtility.FromJson<PlayerData>(jsonloadingtext);
    //        if (mPlayerData == null)
    //        {
    //            Debug.LogWarning("problema con il file json in lettura : non è valido !!!");
    //            return false;
    //        }
    //        else
    //        {
    //            Debug.LogWarning("creo dei dati di default !!!");
    //            playerName = "Player";
    //            playerScore = 0;
    //        }
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError("Errore nella lettura dei dati per un errore : " + e.Message);
    //        return false;
    //    }
    //    return true;
    //}
    //public bool SavePlayerDataFile() // string PlayerName, int Time
    //{
    //    try
    //    {
    //        string jsonwritingText = JsonUtility.ToJson(mPlayerData);
    //        File.WriteAllText(_savePlayerFile, jsonwritingText);
    //        Debug.Log("File di salvataggio Player Data scritto in: " + _savePlayerFile);
    //        return true;
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError("Errore nel salvataggio del Player data: " + e.Message);
    //        return false;
    //    }
    //}
    //*********************************************************************************************//
    //******************************* LEDEARBOARD ............ ************************************//
    //*********************************************************************************************//
    //public void AddHighscoreEntry(int Time, string Name)
    //{
    //    string saveFilePath = Path.Combine(Application.persistentDataPath, "LeaderboardData.json");
    //    HighscoreEntry highscoreEntry = new HighscoreEntry { time = Time, name = Name }; // Nuovo punteggio da salvare .....
    //    if (!File.Exists(saveFilePath))
    //    {
    //        // Il file non esiste, creiamo un nuovo file con una lista vuota
    //        Debug.Log("File Leaderboard non esistente, ne creo uno nuovo.");
    //        Highscores highscores = new Highscores();
    //        highscores.highscoreEntryList = new List<HighscoreEntry>();  // inizializzo lista di high score ....
    //        highscores.highscoreEntryList.Add(highscoreEntry); // Aggiungi il nuovo punteggio
    //        string json = JsonUtility.ToJson(highscores, true);
    //        File.WriteAllText(saveFilePath, json);
    //        Debug.Log("Leaderboard creata e salvata in: " + saveFilePath);
    //    }
    //    else
    //    {
    //        string json = File.ReadAllText(saveFilePath);
    //        Highscores highscores = JsonUtility.FromJson<Highscores>(json);
    //        if (highscores == null)
    //        {
    //            Debug.LogWarning("Errore nel caricamento dei dati: il JSON non è valido.");
    //            return;
    //        }
    //        if (highscores.highscoreEntryList == null)
    //        {
    //            highscores.highscoreEntryList = new List<HighscoreEntry>(); // inizializzo lista di high score .... 
    //        }
    //        highscores.highscoreEntryList.Add(highscoreEntry); // AGGIUNGI NUOVO TEMPO PLAYER ......
    //        // Save updated Highscores     
    //        json = JsonUtility.ToJson(highscores,true);
    //        File.WriteAllText(saveFilePath, json);
    //        Debug.Log("Leaderboard salvata in: " + saveFilePath);
    //    }
    //}   

}