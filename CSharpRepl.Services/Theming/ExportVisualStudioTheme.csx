﻿//Expecting path to .vstheme file in first argument.
//.vstheme is xml file that can be generated by https://marketplace.visualstudio.com/items?itemName=idex.colorthemedesigner2022
//This script will extract C# highlighting informations and will generate appropriate json file alongside original vstheme file.

#r "nuget: Microsoft.CodeAnalysis.Workspaces.Common"

using System;
using System.IO;
using System.Xml.Linq;
using System.Text.Json;
using System.Reflection;
using Microsoft.CodeAnalysis.Classification;

const string UnknownValue = "???";

var vsTheme = XElement.Load(args[0]);
string GetVsColor(string name)
{
    XElement group;
    if (name.Equals("text", StringComparison.OrdinalIgnoreCase) || 
        name.Equals("identifier", StringComparison.OrdinalIgnoreCase))
    {
        GetPlainTextInfo(out group, out name);
    }
    else
    {
        group = vsTheme;
    }
    
    var colorElem = GetColorElement(group, name, out var hasValue);
    if (!hasValue)
    {
        if (name.Equals("record class name", StringComparison.OrdinalIgnoreCase))
        {
            name = "class name";
        }
        else if (name.Equals("record struct name", StringComparison.OrdinalIgnoreCase))
        {
            name = "struct name";
        }
        else if (
            name.Equals("field name", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("enum member name", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("constant name", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("property name", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("event name", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("namespace name", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("label name", StringComparison.OrdinalIgnoreCase))
        {
            //no special coloring in VS
            GetPlainTextInfo(out group, out name);
        }

        colorElem = GetColorElement(group, name, out hasValue);
        if (!hasValue) return UnknownValue;
    }

    var color = colorElem.Attribute("Source")?.Value ?? UnknownValue;
    if (color.Length == 8) color = color.Substring(2); //throw alpha channel away (eg. from FF8BE9FD)
    return "#" + color;

    static XElement GetColorElement(XElement group, string name, out bool hasValue)
    {
        var colorElem = group
           .Descendants("Color")
           .SingleOrDefault(e => e.Attribute("Name")?.Value.Equals(name, StringComparison.OrdinalIgnoreCase) == true)
           ?.Element("Foreground");
        hasValue = colorElem != null && colorElem.Attribute("Type")?.Value == "CT_RAW";
        return colorElem;
    }

    void GetPlainTextInfo(out XElement group, out string name)
    {
        group = vsTheme.Descendants().Where(e => e.Attribute("Name")?.Value == "Text Editor Text Manager Items").Single();
        name = "plain text";
    }
}

record Color(string name, string foreground);
record Colors(Color[] syntaxHighlightingColors);

var colors =
    typeof(ClassificationTypeNames)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.FieldType == typeof(string))
        .Select(f => (string)f.GetValue(null))
        .Where(name =>
            !name.Equals("whitespace", StringComparison.OrdinalIgnoreCase) &&
            !name.Equals("static symbol", StringComparison.OrdinalIgnoreCase)) //???
        .Select(
            name =>
            {
                Console.WriteLine(name);
                return new Color(name, GetVsColor(name));
            })
        .ToArray();

var jsonString = JsonSerializer.Serialize(new Colors(colors), new JsonSerializerOptions() { WriteIndented = true });
File.WriteAllText(Path.ChangeExtension(args[0], "json"), jsonString);