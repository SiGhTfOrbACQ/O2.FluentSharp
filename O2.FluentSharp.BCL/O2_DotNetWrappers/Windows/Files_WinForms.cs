﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using O2.DotNetWrappers.O2Misc;
using O2.Kernel;
using System.Windows.Forms;

namespace O2.DotNetWrappers.Windows
{
    public class Files_WinForms : Files
    {
        public static List<String> loadSourceFileIntoList(String pathToSourceCodeFile)
        {
            return loadSourceFileIntoList(pathToSourceCodeFile, true);
        }

        public static List<String> loadSourceFileIntoList(String pathToSourceCodeFile, bool useFileCacheIfPossible)
        {
            if (useFileCacheIfPossible && DI.dFilesLines.ContainsKey(pathToSourceCodeFile))
                return DI.dFilesLines[pathToSourceCodeFile];
            // in case the file is a reference, try to map it
            string mappedSourceCodeFile = SourceCodeMappingsUtils.mapFile(pathToSourceCodeFile);
            var lsSourceCode = new List<string>();
            string sFileContents = getFileContents(mappedSourceCodeFile);
            if (sFileContents != "")
            {
                sFileContents = sFileContents.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
                // fix the files only use \n for line breaks
                lsSourceCode.AddRange(sFileContents.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
                if (false == DI.dFilesLines.ContainsKey(pathToSourceCodeFile))
                    DI.dFilesLines.Add(pathToSourceCodeFile, lsSourceCode);
                else
                    DI.dFilesLines[pathToSourceCodeFile] = lsSourceCode;
            }
            return lsSourceCode;
        }

        public static String getLineFromSourceCode(string pathToSourceCodeFile, UInt32 uLineNumber, bool useFileCacheIfPossible)
        {
            return getLineFromSourceCode(uLineNumber, loadSourceFileIntoList(pathToSourceCodeFile, useFileCacheIfPossible));
        }

        public static String getLineFromSourceCode(string pathToSourceCodeFile, UInt32 uLineNumber)
        {
            return getLineFromSourceCode(pathToSourceCodeFile, uLineNumber, true);
        }

        public static long getFilesSize(List<string> filesToProcess)
        {
            long filesSize = 0;
            foreach (var file in filesToProcess)
            {
                string mappedSourceCodeFile = SourceCodeMappingsUtils.mapFile(file);
                if (File.Exists(mappedSourceCodeFile))
                {
                    var fileInfo = new FileInfo(mappedSourceCodeFile);
                    filesSize += fileInfo.Length;
                }
            }
            return (filesSize == 0) ? -1 : filesSize;
        }

        public static long getFileSize(string fileToProcess)
        {
            string mappedSourceCodeFile = SourceCodeMappingsUtils.mapFile(fileToProcess);
            if (File.Exists(mappedSourceCodeFile))
            {
                var fileInfo = new FileInfo(mappedSourceCodeFile);
                return fileInfo.Length;
            }
            return -1;
        }

        public static bool deleteFiles(List<string> filesToDelete, bool askUserForConfirmation)
        {
            if (DialogResult.Yes == MessageBox.Show(
                                        String.Format("Are you sure you want to delete {0} file(s)", filesToDelete.Count),
                                        "Delete Files", MessageBoxButtons.YesNo))
            {
                PublicDI.log.debug("Deleting {0} files", filesToDelete.Count);
                foreach (var fileToDelete in filesToDelete)
                    deleteFile(fileToDelete);
                return true;
            }
            return false;
        }

        public static List<String> loadLargeSourceFileIntoList(String pathToSourceCodeFile, bool useFileCacheIfPossible)
        {
            if (DI.dFilesLines.ContainsKey(pathToSourceCodeFile))
                return DI.dFilesLines[pathToSourceCodeFile];
            var lsSourceCode = new List<string>();
            
            TextReader textReader = new StreamReader(pathToSourceCodeFile);
            var itemsProcessed = 0;
            while (textReader.Peek() > 0)
            {                
                lsSourceCode.Add(textReader.ReadLine());
                if (itemsProcessed++ % 100000 == 0)
                    PublicDI.log.info("in loadLargeSourceFileIntoList, # lines loaded so far :{0}", itemsProcessed);
            }
            PublicDI.log.info("in loadLargeSourceFileIntoList, total # lines loaded:{0}", itemsProcessed);
            textReader.Close();
            DI.dFilesLines.Add(pathToSourceCodeFile, lsSourceCode);
            return lsSourceCode;
        }
    }
}
