using System.Windows;
using System.Windows.Input;

// CUSTOM SYSTEM IMPORTS
using System.IO;
using System.Collections.Generic;
using System;
using Ookii.Dialogs.Wpf;

namespace SeriousTTT_Demo_Reviewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Server IP -> Name

        public MainWindow()
        {
            InitializeComponent();
        }

        public class DemoInfo
        {
            public string fileName { get; set; }
            public string server { get; set; }
            public string mapName { get; set; }
            public string demoStart { get; set; }
            public string demoEnd { get; set; }
        }

        /// <summary>
        ///  Handles filepath of a demo and adds it to the data grid
        /// </summary>
        /// <param name="sPath">Full file path + filename + extension of the demo file to read</param>
        public void processDemoAndAddToList(string sPath)
        {
            Dictionary<string, string> serverIpToName = new Dictionary<string, string>();

            // Regular IPs
            serverIpToName["192.184.57.2:27015"] = "Serious TTT West";
            serverIpToName["172.106.104.11:27015"] = "Serious TTT East";
            serverIpToName["192.184.57.6:27015"] = "Serious TTT Vanilla";
            serverIpToName["172.106.104.13:27015"] = "Serious TTT Vanilla 2";
            serverIpToName["172.106.104.14:27015"] = "SGM Deathrun East";

            // URL redirects
            serverIpToName["west.seriousttt.com"] = "Serious TTT West";
            serverIpToName["east.seriousttt.com"] = "Serious TTT East";
            serverIpToName["vanilla.seriousttt.com"] = "Serious TTT Vanilla";
            serverIpToName["vanilla2.seriousttt.com"] = "Serious TTT Vanilla 2";
            serverIpToName["east.seriousdr.com"] = "SGM Deathrun East";

            if (!File.Exists(sPath))
            {
                MessageBox.Show("The demo file could not be found (File not found)", "ERROR: File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Stop!
            }

            var demoReader = new DemoReader(sPath);
            demoReader.ReadHeader();

            string sServerName;

            if (serverIpToName.ContainsKey(demoReader.Header.ServerName))
            {
                sServerName = serverIpToName[demoReader.Header.ServerName];
            } else
            {
                sServerName = demoReader.Header.ServerName + " | (ERROR RESOLVING IP TO SERIOUS GMOD SERVER NAME";
            }

            string[] explodedFilePath = sPath.Split('\\');
            string sFileName = explodedFilePath[explodedFilePath.Length - 1]; // The last '\' is the file name

            DemoInfo demoInfo = new DemoInfo();
            demoInfo.fileName = sFileName;
            demoInfo.server = sServerName;
            demoInfo.mapName = demoReader.Header.MapName;
            demoInfo.demoStart = File.GetCreationTime(sPath).ToString();
            demoInfo.demoEnd = File.GetLastWriteTime(sPath).ToString();

            dataFromDemoFolder.Items.Add(demoInfo);
        }

        /// <summary>
        /// Take input of demo file and read header of demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void demoFileName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                // processDemoAndAddToList(demoFileName.Text);
            }
        }
        
        private void copyPlayConsoleCommand_Click(object sender, RoutedEventArgs e)
        {
            int iSelectedIndex = dataFromDemoFolder.SelectedIndex;
            DemoInfo currentlySelectedRow = dataFromDemoFolder.SelectedItem as DemoInfo;
            Clipboard.SetDataObject("playdemo garrysmod\\demos\\" + currentlySelectedRow.fileName);
            MessageBox.Show("Console command copied to clipboard! Paste into your GMod console");
        }

        private void selectDemoFile_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowser = new VistaFolderBrowserDialog();

            bool? success = folderBrowser.ShowDialog();
            if (success == true)
            {
                // Clear out datagrid just incase we had any previous loaded folders
                dataFromDemoFolder.Items.Clear();
                currentDemoPathTextBbox.Text = folderBrowser.SelectedPath;

                string[] fileEntries = Directory.GetFiles(folderBrowser.SelectedPath);

                foreach(string sFile in fileEntries)
                {
                    string sFileExtension = sFile.Substring(sFile.Length - 4);

                    if (sFileExtension != ".dem")
                    {
                        continue; // This file isn't a demo. Continue to our next file
                    }

                    processDemoAndAddToList(sFile);
                }

                MessageBox.Show("Folder has been processed and all demos in the contained folder have been loaded and displayed");
            }
        }
    }
}
