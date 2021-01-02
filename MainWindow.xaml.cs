using System.Windows;
using System.Windows.Input;

// CUSTOM SYSTEM IMPORTS
using System.IO;
using System.Collections.Generic;
using System;
using Microsoft.Win32;

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

        public void processDemo(string sPath)
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

            Console.WriteLine(demoReader.Header.Header);

            string sServerName;

            if (serverIpToName.ContainsKey(demoReader.Header.ServerName))
            {
                sServerName = serverIpToName[demoReader.Header.ServerName];
            } else
            {
                sServerName = demoReader.Header.ServerName + " | (ERROR RESOLVING IP TO SERIOUS GMOD SERVER NAME";
            }

            demoFileInfo.Text = string.Format("Demo Start: {0}\nDemo End: {1}\nServer: {2}\nMap: {3}", File.GetCreationTime(sPath), File.GetLastWriteTime(sPath), sServerName, demoReader.Header.MapName);

            MessageBox.Show("Demo loaded and displayed");
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
                processDemo(demoFileName.Text);
            }
        }

        private void selectDemoFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Filter = "Demo files |*.dem";

            Nullable<bool> result = fileDialog.ShowDialog();

            if (result == true)
            {
                string sFilePath = fileDialog.FileName;
                demoFileName.Text = sFilePath;
                processDemo(sFilePath);
            }
        }
    }
}
