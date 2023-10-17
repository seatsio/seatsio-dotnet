﻿using System.Collections.Generic;

namespace SeatsioDotNet.Charts;

public class Category
{
    public object Key { get; set; }
    public string Label { get; set; }
    public string Color { get; set; }
    public bool? Accessible { get; set; }

    public Category()
    {
    }

    public Category(long key, string label, string color, bool? accessible = false)
    {
        Key = key;
        Label = label;
        Color = color;
        Accessible = accessible;
    }

    public Category(string key, string label, string color, bool? accessible = false)
    {
        Key = key;
        Label = label;
        Color = color;
        Accessible = accessible;
    }

    public Dictionary<string, object> AsDictionary()
    {
        var dictionary = new Dictionary<string, object>();

        if (Key != null)
        {
            dictionary.Add("key", Key);
        }

        if (Label != null)
        {
            dictionary.Add("label", Label);
        }

        if (Color != null)
        {
            dictionary.Add("color", Color);
        }

        if (Accessible != null)
        {
            dictionary.Add("accessible", Accessible);
        }

        return dictionary;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != typeof(Category))
        {
            return false;
        }

        Category cat = (Category) obj;
        return Key.Equals(cat.Key) &&
               Label.Equals(cat.Label) &&
               Color.Equals(cat.Color) &&
               Accessible.Equals(cat.Accessible);
    }
}