using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;



public class AppManager : MonoBehaviour
{
    public List<(string, BoardLayout)> boardLayouts = new List<(string, BoardLayout)>();
    

    public static AppManager Instance { get; private set; }

    public MultiplayerManager multiplayerManager;

    JsonSerializerSettings serializerSettings;

    public BoardLayout selectedBoardLayout;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        boardLayouts.Add(("default",GenerateStandardChessBoardLayout()));
        RegisterToMultiplayerManager();

        JsonSerializerSettingInitialization();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RegisterToMultiplayerManager()
    {
        multiplayerManager = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>();
        if (multiplayerManager != null)
        {
            multiplayerManager.appManager = this;
        }
        else
        {
            Debug.Log("CRITICAL ERROR. Menu Manager not found");
        }
    }

    public void AddBoardLayout(string boardName,BoardLayout bl)
    {
        boardLayouts.Add((boardName,bl));
    }

    public void SelectBoardLayout(BoardLayout bl)
    {
        selectedBoardLayout = bl;
    }

    public string GetCurrentSerializedBoardLayout()
    {
        if(selectedBoardLayout != null)
        {
            return JsonConvert.SerializeObject(selectedBoardLayout,serializerSettings);
        }
        return null;
    }

    public void SetCurrentBoardLayout(string serializedData, bool whitePov)
    {
        BoardLayout newBl = JsonConvert.DeserializeObject<BoardLayout>(serializedData, serializerSettings);

        if (newBl == null)
        {
            Debug.Log("Board layout not correctly received");
            return;
            
        }

        newBl.whitePov = whitePov;

        selectedBoardLayout = newBl;
    }

    public BoardLayout GetBoardLayout(string name)
    {
        foreach (var layout in boardLayouts)
        {
            if (layout.Item1 == name)
            {
                return layout.Item2;
            }
        }
        return null;
    }

    public List<(string,BoardLayout)> GetBoardLayouts()
    {
        return boardLayouts;
    }
    private void JsonSerializerSettingInitialization()
    {
        serializerSettings = new JsonSerializerSettings();
        serializerSettings.Converters.Add(new ValueTupleKeyDictionaryConverter());
    }

    private BoardLayout GenerateStandardChessBoardLayout()
    {
        //Standard chess board setup
        Dictionary<(int, int), (bool, string)> debugDict = new Dictionary<(int, int), (bool, string)>
        {
            { (1,1),(true,"whiteRook") },
            { (1,2),(true,"whitePawn") },
            { (1,3),(true,"") },
            { (1,4),(true,"") },
            { (1,5),(true,"") },
            { (1,6),(true,"") },
            { (1,7),(true,"blackPawn") },
            { (1,8),(true,"blackRook") },
            { (2,1),(true,"whiteKnight") },
            { (2,2),(true,"whitePawn") },
            { (2,3),(true,"") },
            { (2,4),(true,"") },
            { (2,5),(true,"") },
            { (2,6),(true,"") },
            { (2,7),(true,"blackPawn") },
            { (2,8),(true,"blackKnight") },
            { (3,1),(true,"whiteBishop") },
            { (3,2),(true,"whitePawn") },
            { (3,3),(true,"") },
            { (3,4),(true,"") },
            { (3,5),(true,"") },
            { (3,6),(true,"") },
            { (3,7),(true,"blackPawn") },
            { (3,8),(true,"blackBishop") },
            { (4,1),(true,"whiteQueen") },
            { (4,2),(true,"whitePawn") },
            { (4,3),(true,"") },
            { (4,4),(true,"") },
            { (4,5),(true,"") },
            { (4,6),(true,"") },
            { (4,7),(true,"blackPawn") },
            { (4,8),(true,"blackQueen") },
            { (5,1),(true,"whiteKing") },
            { (5,2),(true,"whitePawn") },
            { (5,3),(true,"") },
            { (5,4),(true,"") },
            { (5,5),(true,"") },
            { (5,6),(true,"") },
            { (5,7),(true,"blackPawn") },
            { (5,8),(true,"blackKing") },
            { (6,1),(true,"whiteBishop") },
            { (6,2),(true,"whitePawn") },
            { (6,3),(true,"") },
            { (6,4),(true,"") },
            { (6,5),(true,"") },
            { (6,6),(true,"") },
            { (6,7),(true,"blackPawn") },
            { (6,8),(true,"blackBishop") },
            { (7,1),(true,"whiteKnight") },
            { (7,2),(true,"whitePawn") },
            { (7,3),(true,"") },
            { (7,4),(true,"") },
            { (7,5),(true,"") },
            { (7,6),(true,"") },
            { (7,7),(true,"blackPawn") },
            { (7,8),(true,"blackKnight") },
            { (8,1),(true,"whiteRook") },
            { (8,2),(true,"whitePawn") },
            { (8,3),(true,"") },
            { (8,4),(true,"") },
            { (8,5),(true,"") },
            { (8,6),(true,"") },
            { (8,7),(true,"blackPawn") },
            { (8,8),(true,"blackRook") },
        };
        BoardLayout debugLayout = new BoardLayout(3, 4, debugDict, false, true);

        return debugLayout;
    }
}

public class ValueTupleKeyDictionaryConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Dictionary<(int, int), (bool, string)>);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {

        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        var tempDict = serializer.Deserialize<Dictionary<string, (bool, string)>>(reader);
        if (tempDict == null)
        {
            throw new JsonSerializationException("Deserialized dictionary is null.");
        }

        var dict = new Dictionary<(int, int), (bool, string)>();
        foreach (var kvp in tempDict)
        {
            if (string.IsNullOrEmpty(kvp.Key))
            {
                continue; 
            }

            var keyParts = kvp.Key.Trim('(', ')').Split(',');
            if (keyParts.Length != 2 ||
                !int.TryParse(keyParts[0], out int x) ||
                !int.TryParse(keyParts[1], out int y))
            {
                throw new JsonSerializationException($"Invalid key format: {kvp.Key}");
            }

            var key = (x, y);
            dict[key] = kvp.Value;
        }

        return dict;
    }


    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var dict = (Dictionary<(int, int), (bool, string)>)value;
        var tempDict = new Dictionary<string, (bool, string)>();

        foreach (var kvp in dict)
        {
            tempDict[$"({kvp.Key.Item1}, {kvp.Key.Item2})"] = kvp.Value;
        }

        serializer.Serialize(writer, tempDict);
    }
}
