using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace wttrwidget.Config
{
    /*
        I know this code could be written more efficiently, but, hey, this WORKS
        (If you want to suggest any modifications, feel free to do so...)
     */
    public class ConfigParser
    {
        List<string> configFileLines;
        private string filePath;
        public ConfigParser(string filePath)
        {
            this.filePath = filePath;
            if (File.Exists(filePath))
                this.configFileLines = new List<string>(File.ReadAllLines(filePath));
            else
            {
                var assembly = Assembly.GetEntryAssembly();
                Stream resourceStream = assembly.GetManifestResourceStream("wttrwidget.__res.defaultconfig.conf");
                StreamReader resourceReader = new StreamReader(resourceStream);
                string defaultConfig = resourceReader.ReadToEnd();
                this.configFileLines = new List<string>(defaultConfig.Split(Environment.NewLine));
                File.WriteAllText(filePath, defaultConfig);
            }
        }

        public Config ParseToConfig()
        {
            Dictionary<string, string> parsedConfigFile = new Dictionary<string, string>();
            Config outputConfig;
            foreach (string line in this.configFileLines)
            {
                string cleanedLine = line.Replace(" ", "").Replace("\t", "");
                if (cleanedLine.StartsWith('#') || cleanedLine == "")
                    continue;
                string[] splittedLine = cleanedLine.Split("=");
                string key = splittedLine[0];
                string value = splittedLine[1];

                parsedConfigFile.Add(key, value);
            }
            try
            {
                outputConfig = new Config(float.Parse(parsedConfigFile["version"], CultureInfo.InvariantCulture));
            }
            catch (Exception e)
            {
                if (MessageBox.Show(e.Message + Environment.NewLine +
                                    "Do you want to continue anyways ?",
                                    "Error occurred when loading config file",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Error)
                    == DialogResult.Yes)
                {
                    outputConfig = new Config(0, true);
                }
                else throw;
            }

            outputConfig.LoadConfig(parsedConfigFile);

            return outputConfig;
        }

        public void WriteConfig(Config config)
        {
            // Get all values in a string array
            Dictionary<string, string> currentConfigDict = config.GetConfigAsDict();
            List<string> checkedKeys = new List<string>();
            for (int i = 0; i < this.configFileLines.Count; i++)
            {
                string line = this.configFileLines[i];
                string cleanedLine = line.Replace(" ", "").Replace("\t", "");
                if (cleanedLine.StartsWith('#') || cleanedLine == "")
                    continue;
                string[] splittedLine = cleanedLine.Split("=");
                string key = splittedLine[0];
                string value = splittedLine[1];

                if (currentConfigDict.ContainsKey(key))
                {
                    checkedKeys.Add(key);
                    if (currentConfigDict[key] != value)
                    {
                        this.configFileLines[i] = $"{key}={currentConfigDict[key]}";
                    }
                }
            }

            foreach (KeyValuePair<string, string> keyAndValue in currentConfigDict)
            {
                if (!checkedKeys.Contains(keyAndValue.Key)) {
                    this.configFileLines.Add($"{keyAndValue.Key}={keyAndValue.Value}");
                }
            }

            // Write everything to the file
            File.WriteAllLines(this.filePath, this.configFileLines);
        }
    }
}