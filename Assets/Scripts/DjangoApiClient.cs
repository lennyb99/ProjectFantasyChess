using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Text;
using Unity.VisualScripting.FullSerializer;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

/// <summary>
/// Enthält Daten für das Speichern eines Spielbretts.
/// </summary>
[Serializable]
public class BoardData
{
    public string title;        
    public string position_data; 
}

/// <summary>
/// Enthält das Token-Response-Objekt vom Django-Backend.
/// </summary>
[Serializable]
public class TokenResponse
{
    public string access;
    public string refresh;
}

[Serializable]
public class RegisterRequest
{
    public string username;
    public string password;

    public RegisterRequest(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

[Serializable]
public class LoginRequest
{
    public string username;
    public string password;

    public LoginRequest(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

/// <summary>
/// Beispiel-API für den Zugriff auf das Django-Backend.
/// </summary>
public static class DjangoBackendAPI
{
    // Passe diesen Basis-URL an deinen Server an (z.B. https://dein-server.de/ oder http://127.0.0.1:8000/)
    private static readonly string baseUrl = "http://127.0.0.1:8000/";

    // Hier wird nach dem Login der Access Token gespeichert, 
    // sodass er für weitere API-Aufrufe gesetzt werden kann.
    private static string accessToken = "";


    /// <summary>
    /// Registriert einen Benutzer im Backend.
    /// </summary>
    /// <param name="username">Benutzername</param>
    /// <param name="password">Passwort</param>
    /// <param name="onComplete">
    /// Callback, das bei Abschluss des Requests aufgerufen wird.
    /// Übergibt:
    ///  - bool success: true, wenn Erfolg, false sonst
    ///  - string response: Erfolgsmeldung oder Fehlermeldung
    /// </param>
    /// 
    public static IEnumerator RegisterUser(string username, string password, Action<bool, string> onComplete)
    {
        // Request-URL
        string url = $"{baseUrl}api/users/register/";

        // JSON-Body erstellen
        RegisterRequest body = new RegisterRequest(username, password);
        string jsonData = JsonUtility.ToJson(body);


        // UnityWebRequest für POST
        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");

            // Request senden
            yield return request.SendWebRequest();

            // Ergebnis prüfen
            if (request.result == UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(true, request.downloadHandler.text);
            }
            else
            {
                onComplete?.Invoke(false, request.error);
            }
        }
    }

    /// <summary>
    /// Loggt einen Benutzer ein und speichert das erhaltene Access-Token.
    /// </summary>
    /// <param name="username">Benutzername</param>
    /// <param name="password">Passwort</param>
    /// <param name="onComplete">
    /// Callback, das bei Abschluss des Requests aufgerufen wird.
    /// Übergibt:
    ///  - bool success: true, wenn Erfolg, false sonst
    ///  - string response: Erfolgsmeldung oder Fehlermeldung
    /// </param>
    public static IEnumerator Login(string username, string password, Action<bool, string> onComplete)
    {
        string url = $"{baseUrl}api/users/token/";

        LoginRequest body = new LoginRequest(username, password);
        string jsonData = JsonUtility.ToJson(body);

        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");


            request.redirectLimit = 0;
            request.useHttpContinue = false;

            // Timeout kann sinnvoll sein
            request.timeout = 5;

            yield return request.SendWebRequest();  // <-- Hier wartet die Coroutine, bis Antwort kommt

            // Ab hier kannst du den Status auswerten, ohne erneut SendWebRequest() aufzurufen:
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Request failed: {request.error}");
                onComplete?.Invoke(false, request.error);
            }
            else
            {
                // Token aus dem JSON lesen
                TokenResponse tokenResponse = JsonUtility.FromJson<TokenResponse>(request.downloadHandler.text);

                if (!string.IsNullOrEmpty(tokenResponse.access))
                {
                    accessToken = tokenResponse.access;
                    onComplete?.Invoke(true, "Login erfolgreich. Access-Token gespeichert.");
                }
                else
                {
                    onComplete?.Invoke(false, "Login fehlgeschlagen, Token nicht gefunden.");
                }
            }
        }
    }

    /// <summary>
    /// Schickt ein selbst designtes Schachbrett (Board) an das Backend, um es für einen bestimmten User zu speichern.
    /// </summary>
    /// <param name="username">Der Benutzer, dem das Board zugeordnet werden soll.</param>
    /// <param name="title">Name des Boards</param>
    /// <param name="positionData">String, der das Board beschreibt (z.B. in deiner eigenen Notation)</param>
    /// <param name="onComplete">Callback bei Abschluss</param>
    public static IEnumerator SaveBoard(string username, string title, string positionData, Action<bool, string> onComplete)
    {
        string url = $"{baseUrl}api/boardposition/{username}/";

        BoardData boardData = new BoardData
        {
            title = title,
            position_data = positionData
        };
        string jsonData = JsonUtility.ToJson(boardData);
        //Debug.Log("BOARDDATA: " + jsonData);

        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.SetRequestHeader("Authorization", $"Bearer {accessToken}");
            }

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(true, request.downloadHandler.text);
            }
            else
            {
                onComplete?.Invoke(false, request.error);
            }
        }
    }

    /// <summary>
    /// Holt alle gespeicherten Spielbretter eines bestimmten Benutzers vom Backend.
    /// </summary>
    /// <param name="username">Benutzername</param>
    /// <param name="onComplete">Callback, das bei Abschluss aufgerufen wird. Übergibt JSON oder Fehler.</param>
    public static IEnumerator GetBoards(string username, Action<bool, string> onComplete)
    {
        string url = $"{baseUrl}api/boardposition/{username}/";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Token setzen, falls erforderlich
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.SetRequestHeader("Authorization", $"Bearer {accessToken}");
            }

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(true, request.downloadHandler.text);
            }
            else
            {
                onComplete?.Invoke(false, request.error);
            }
        }
    }

    public static string[] SplitJsonObjects(string jsonString)
    {
        List<string> jsonObjects = new List<string>();
        int braceCount = 0;
        int startIndex = -1;

        for (int i = 0; i < jsonString.Length; i++)
        {
            char c = jsonString[i];

            // Beginn eines JSON-Objekts
            if (c == '{')
            {
                // Wenn noch kein Objekt begonnen hat, merke den Startindex
                if (braceCount == 0)
                {
                    startIndex = i;
                }
                braceCount++;
            }
            // Ende eines JSON-Objekts
            else if (c == '}')
            {
                braceCount--;
                // Wenn alle geöffneten Klammern geschlossen wurden, ist das Objekt komplett
                if (braceCount == 0 && startIndex != -1)
                {
                    int length = i - startIndex + 1;
                    string objString = jsonString.Substring(startIndex, length);
                    jsonObjects.Add(objString);
                    startIndex = -1;
                }
            }
        }

        return jsonObjects.ToArray();
    }
}

[Serializable]
public class BoardResponse
{
    public string title;
    public string user;          
    public string position_data;
}


