using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using System.Drawing;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class ConfigFiles_ExtensionMethods
    {
        public static string localScriptFile(this string file)
        {
            if (PublicDI.CurrentScript.valid())
                return PublicDI.CurrentScript.directoryName().pathCombine(file);
            return null;
        }
        public static Dictionary<string, string> localConfig_Load(this string file)
        {
            var configFile = file.localScriptFile();
            if (configFile.fileExists())
                return configFile.configLoad();
            return null;
        }

        public static Dictionary<string, string> localConfig_Save(this Dictionary<string, string> dictionary, string file)
        {
            var configFile = file.localScriptFile();
            "Saving {0} items to file: {1}".info(dictionary.Count, configFile);
            return dictionary.configSave(configFile);
        }        
    }
}
