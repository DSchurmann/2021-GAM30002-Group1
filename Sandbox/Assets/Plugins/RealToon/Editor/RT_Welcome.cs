//MJQStudioWorks
//2020

using UnityEditor;
using System.IO;

namespace RealToon.Editor.Welcome
{

    [InitializeOnLoad]
    class RT_Welcome
    {

        #region RT_Welcome
        static readonly string rt_welcome_settings = "Assets/RealToon/Editor/RTW.sett";

        static RT_Welcome()
        {
            if (File.Exists(rt_welcome_settings))
            {
                if (File.ReadAllText(rt_welcome_settings) == "0")
                {
                    if (File.Exists(rt_welcome_settings))
                    {
                        EditorApplication.delayCall += Run_Welcome;
                    }
                }
            }
        }

        static void Run_Welcome()
        {

            if (EditorUtility.DisplayDialog(

               "Thank you for purchasing and using RealToon Shader",

               "*The default imported RealToon Shader, Effects and Example are for Built-In RP.\n\n" +

               "*If you are an unity 2019 user and beyond and SRP user, read the 'For Unity 2019 - Beyond and SRP's users.txt' text file. \n\n" +

               "*If you are a VRoid user, read the 'For VRoid users please read.txt' text file.\n\n" +

               "*For video tutorials and user guide, see the bottom part of RealToon Inspector panel.\n\n" +

               "*If you need some help/support, just send an email including the invoice number.\n" +
               "See the 'User Guide.pdf' file for the links and email support.\n\n" +

               "*If you want RealToon Shader to work on PS4 or future PS console, just send an email and i'll help you to make it work.\n" +
               "There are some things that are needed to be done before it works on PS4/PS."
               ,

               "Ok") )

            {

                File.WriteAllText(rt_welcome_settings, "1");
                AssetDatabase.Refresh();

            }

        }

        #endregion

    }

}