using System;
using System.Collections;
using System.IO;
using System.Xml;
 
using UnityEngine;
 
public class Lang: SingletonMonoBehaviour<Lang>
{
    private Hashtable Strings;
    TextAsset xmlText;

    public void setLanguage ( int textType /*TextAsset xmlText*/, string language) {
        //0 for menus
        //1 for history
        if(textType == 0){
            xmlText = Resources.Load<TextAsset>("Language/menusLang");
        }
        else if(textType == 1){
            xmlText = Resources.Load<TextAsset>("Language/historyLang");
        }

        XmlDocument xml = new XmlDocument ();
        xml.LoadXml ( xmlText.text );
    
        Strings = new Hashtable();
        var element = xml.DocumentElement[language];
        if (element != null) {
            var elemEnum = element.GetEnumerator();
            while (elemEnum.MoveNext()) {
                var xmlItem = (XmlElement)elemEnum.Current;
                Strings.Add(xmlItem.GetAttribute("name"), xmlItem.InnerText);
            }
        } else {
            Debug.LogError("The specified language does not exist: " + language);
        }
    }

    public string getString (string name) {

        if (!Strings.ContainsKey(name)) {
            Debug.LogError("The specified string does not exist: " + name);
         
            return "";
        }
 
        return (string)Strings[name];
    }
 
}