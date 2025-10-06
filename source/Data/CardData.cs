using Globals;
using Globals.Entities;
using Globals.Enums;
using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Data
{
    public class CardData : IData
    {
        private readonly string _savePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\My Games\\Klondike\\";

        public Stock Stock { get; private set; }
        public Dictionary<string, Pile> Piles { get; private set; }
        public Dictionary<string, Foundation> Foundations { get; private set; }

        [JsonConstructor]
        public CardData(Stock stock, Dictionary<string, Pile> piles, Dictionary<string, Foundation> foundations)
        {
            Stock = stock;
            Piles = piles;
            Foundations = foundations;
        }

        public CardData()
        {
            New();
        }

        public void New()
        {
            List<Card> temp = MakeDefault52();
            Piles = new Dictionary<string, Pile>
            {
                { "Pile 1", new(temp, 1) },
                { "Pile 2", new(temp, 2) },
                { "Pile 3", new(temp, 3) },
                { "Pile 4", new(temp, 4) },
                { "Pile 5", new(temp, 5) },
                { "Pile 6", new(temp, 6) },
                { "Pile 7", new(temp, 7) },
            };
            Foundations = new Dictionary<string, Foundation>
            {
                { "Clubs", new(CardColor.Clubs) },
                { "Diamonds", new(CardColor.Diamonds) },
                { "Hearts", new(CardColor.Hearts) },
                { "Spades", new(CardColor.Spades) },
            };
            Stock = new(temp);
        }

        private static List<Card> MakeDefault52()
        {
            var Cards = new List<Card>();
            CardColor color;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j <= 13; j++)
                {
                    color = i switch
                    {
                        0 => CardColor.Clubs,
                        1 => CardColor.Diamonds,
                        2 => CardColor.Hearts,
                        3 => CardColor.Spades,
                        _ => throw new ArgumentException("Unknown CardColor.")
                    };
                    Cards.Add(new Card(color, (CardValue)j));
                }
            }
            Cards.Shuffle();
            return Cards;
        }

        public void SaveFile(string filename)
        {
            Directory.CreateDirectory(_savePath);
            string json = JsonSerializer.Serialize(this);
            // string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            using StreamWriter writer = new($"{_savePath}{filename}.json");
            writer.Write(json);
        }

        public void LoadFile(string filename)
        {
            Directory.CreateDirectory(_savePath);
            if (File.Exists($"{_savePath}{filename}.json"))
            {
                using StreamReader reader = new($"{_savePath}{filename}.json");
                string json = reader.ReadToEnd();
                var lines = JsonSerializer.Deserialize<CardData>(json);
                Stock = lines.Stock;
                Foundations = lines.Foundations;
                Piles = lines.Piles;
            }
            else throw new ArgumentException("File not found");
        }

        public IEnumerable<string> LoadDirectory()
        {
            Directory.CreateDirectory(_savePath);
            List<string> files = new();
            foreach (var file in Directory.GetFiles(_savePath))
            {
                files.Add(file[(file.LastIndexOf('\\') + 1)..file.LastIndexOf('.')]);
            }
            return files;
        }

        public void DeleteFile(string filename)
        {
            filename = $"{_savePath}{filename}.json";
            if (File.Exists(filename)) File.Delete(filename);
        }
    }
}
