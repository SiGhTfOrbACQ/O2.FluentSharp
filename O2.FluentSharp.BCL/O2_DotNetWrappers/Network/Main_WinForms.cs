﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using O2.DotNetWrappers.Windows;
using System.Drawing.Imaging;

namespace O2.DotNetWrappers.Network
{
    public class Main_WinForms
    {
        public static List<String> createAttachmentsForRemoteSupport(RichTextBox richTextBoxWithLog,
                                                             PictureBox screenShotToSend, string sFile_LogViews,
                                                             string sFile_LogViewsTxt, string sFile_ScreenShot)
        {
            var lsAttachements = new List<string>();
            if (richTextBoxWithLog != null && richTextBoxWithLog.Text != "")
            {
                Files.WriteFileContent(sFile_LogViews, richTextBoxWithLog.Rtf);
                lsAttachements.Add(sFile_LogViews);
                Files.WriteFileContent(sFile_LogViewsTxt, richTextBoxWithLog.Text);
                lsAttachements.Add(sFile_LogViewsTxt);
            }

            if (screenShotToSend.BackgroundImage != null)
            {
                screenShotToSend.BackgroundImage.Save(sFile_ScreenShot, ImageFormat.Jpeg);
                lsAttachements.Add(sFile_ScreenShot);
            }
            return lsAttachements;
        }
    }
}
