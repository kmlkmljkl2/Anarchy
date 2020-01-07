﻿using System.IO;
using System.Linq;
using Anarchy.Configuration.Storage;
using Anarchy.Localization;
using Anarchy.UI;
using UnityEngine;
using static Anarchy.UI.GUI;

namespace Anarchy.Configuration.Presets
{
    public class ForestPreset : SkinPreset
    {
        public static readonly string ForestPath = Application.dataPath + "/Configuration/ForestSkins/";
        private const int Length = 17;

        private string[] data;

        public bool RandomizePairs { get; set; }
        public string LinkedSkybox { get; set; }
        public string Ground => data[0];

        public string[] Trees
        {
            get
            {
                string[] result = new string[8];
                int i = 0;
                for(int j = 0; j < 8; j++)
                {
                    result[j] = data[i++];
                }
                return result;
            }
        }

        public string[] Leaves
        {
            get
            {
                string[] result = new string[data.Length - Trees.Length - 1];
                for (int i = Trees.Length + 1; i < data.Length; i++)
                {
                    result[i - Trees.Length - 1] = data[i];
                }
                return result;
            }
        }

        public ForestPreset(string name) : base(name, ForestPath)
        {
            data = new string[Length].Select(x => string.Empty).ToArray();
            LinkedSkybox = "$Not define$";
        }

        public override void Draw(SmartRect rect, Locale locale)
        {
            int index = 0;
            if (LinkedSkybox != "$Not define$")
            {
                Label(rect, locale.Format("linkedBox", LinkedSkybox), true);
            }
            RandomizePairs = ToggleButton(rect, RandomizePairs, locale["forestRandomize"], true);
            data[index] = TextField(rect, data[index++], locale["ground"], Style.LabelOffset, true);
            int count = 1;
            for (int i = index; i < data.Length - Leaves.Length; i++)
            {
                data[i] = TextField(rect, data[i], locale["tree"] + " #" + (count++).ToString(), Style.LabelOffset, true);
                index++;
            }
            count = 1;
            for (int i = index; i < data.Length; i++)
            {
                data[i] = TextField(rect, data[i], locale["leave"] + " #" +  (count++).ToString(), Style.LabelOffset, true);
                index++;
            }
        }

        public static SkinPreset[] GetAllPresets()
        {
            DirectoryInfo info = new DirectoryInfo(ForestPath);
            FileInfo[] files = info.GetFiles();
            if (files.Length == 0)
                return null;
            SkinPreset[] result = new SkinPreset[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                result[i] = new ForestPreset(files[i].Name.Replace(Extension, string.Empty));
                result[i].Load();
            }
            return result;
        }

        public override void Load()
        {
            using (var file = new AnarchyStorage(FullPath, '`', false))
            {
                file.Load();
                file.AutoSave = false;
                data[0] = file.GetString("ground", string.Empty);
                for (int i = 1; i < data.Length - Leaves.Length; i++)
                {
                    data[i] = file.GetString("tree" + (i - 1).ToString(), string.Empty);
                }
                for (int i = data.Length - Leaves.Length; i < data.Length; i++)
                {
                    data[i] = file.GetString("leaves" + (i - 1).ToString(), string.Empty);
                }
                LinkedSkybox = file.GetString("skybox", "$Not define$");
                if(LinkedSkybox != "$Not define$")
                {
                    SkyboxPreset set = new SkyboxPreset(LinkedSkybox);
                    if (!set.Exists())
                    {
                        LinkedSkybox = "$Not define$";
                    }
                }
                RandomizePairs = file.GetBool("randomizePairs", false);
            }
        }

        public override void Save()
        {
            using (var file = new AnarchyStorage(FullPath, '`', true))
            {
                file.SetString("ground", data[0]);
                for (int i = 1; i < data.Length - Leaves.Length; i++)
                {
                    file.SetString("tree" + (i - 1).ToString(), data[i]);
                }
                for (int i = data.Length - Leaves.Length; i < data.Length; i++)
                {
                    file.SetString("leaves" + (i - 1).ToString(), data[i]);
                }
                file.SetString("skybox", LinkedSkybox);
                file.SetBool("randomizePairs", RandomizePairs);
            }
        }

        public override string[] ToSkinData()
        {
            string[] result = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[i] = data[i];
            }
            return result;
        }
    }
}