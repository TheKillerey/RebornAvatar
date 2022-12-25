using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Ookii.Dialogs.Wpf;
using LeagueToolkit.IO.TEXFile;
using System.Drawing;
using System.Windows.Interop;
using Egorozh.ColorPicker.Dialog;
using Egorozh.ColorPicker;
using Xamarin.Essentials;
using System.Globalization;
using RebornAvatar.IO.Materials;
using LeagueToolkit.IO.MapGeometryFile;
using System.Numerics;
using System.Windows.Forms;
using LeagueToolkit.Helpers.Structures;
using LeagueToolkit.IO.WadFile;
using LeagueToolkit.Helpers;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using TreeView = System.Windows.Controls.TreeView;
using System.Runtime.Serialization.Formatters.Binary;
using Squirrel;
using System.Windows.Media.Media3D;
using RebornAvatar.IO.Models;
using HelixToolkit.Wpf;
using HelixToolkit.Wpf.SharpDX;

namespace RebornAvatar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {

            InitializeComponent();
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            Version.Content = fileVersionInfo.ProductVersion;


            byte[] fileContents = File.ReadAllBytes("Downloads/loadingscreen/srbackground.csscraps.png");
            using (var stream = new MemoryStream(fileContents))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                ImgLoadinscreen.Source = bitmap;
            }
            var defaultsettings = File.ReadAllLines("Projects/default.avproj");
            TbSetProject.Text = "Projects/Maps/";
            TbSetLCS.Text = defaultsettings[6].Replace("CSManager Folder: ", "");
            
            //Material2Tree.Extract_DefaultEnv_Flat_AlphaTest(MaterialEditor, Diffuse_Tex);

        }
        
        public MapGeometry mapfile;
        public MapGeometryModel mapmodel;
        public List<string> materiallist;
        public List<string> materiallistmaya;
        public List<string> materiallistlol;
        public string pathofmapgeo;
        public int listnumber;


        static async Task Update()
        {
            using (var mgr = new UpdateManager("C:\\Projects\\MyApp\\Releases"))
            {
                await mgr.UpdateApp();
            }
        }
        

        private void LaunchKillerSkinsSite(object sender, RoutedEventArgs e)
        {
            
            var uri = "https://www.killerskins.com";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);
            string url = uri.Replace("https://www.", "");
            ConsoleMain.AppendText("Open webpage: " + url + Environment.NewLine);

            String firstFolder = "Downloads";

            if (!Directory.Exists(firstFolder))

            {

                Directory.CreateDirectory(firstFolder);
                ConsoleMain.AppendText("Creating Downloads Folder" + Environment.NewLine);
            }

            string subFolder = System.IO.Path.Combine(firstFolder, "loadingscreen");

            if (!Directory.Exists(subFolder))

            {

                Directory.CreateDirectory(subFolder);
                ConsoleMain.AppendText("Creating Downloads/loadingscreen Folder" + Environment.NewLine);
            }

            string subFolder2 = System.IO.Path.Combine(firstFolder, "SRX");

            if (!Directory.Exists(subFolder2))

            {

                Directory.CreateDirectory(subFolder2);
                ConsoleMain.AppendText("Creating Downloads/SRX Folder" + Environment.NewLine);
            }

            string subFolder3 = System.IO.Path.Combine(firstFolder, "ARAM");

            if (!Directory.Exists(subFolder3))

            {

                Directory.CreateDirectory(subFolder3);
                ConsoleMain.AppendText("Creating Downloads/ARAM Folder" + Environment.NewLine);
            }

            string subFolder4 = System.IO.Path.Combine(firstFolder, "TFT");

            if (!Directory.Exists(subFolder4))

            {

                Directory.CreateDirectory(subFolder4);
                ConsoleMain.AppendText("Creating Downloads/TFT" + Environment.NewLine);
            }

            //Download Latest League Map Files
            ConsoleMain.AppendText("Downlaoding Content Files" + Environment.NewLine);

            var loadingscreen = "https://raw.communitydragon.org/latest/game/assets/ux/loadingscreen/srbackground.csscraps.png";
            var materialsrx = "https://raw.communitydragon.org/latest/game/data/maps/mapgeometry/map11/base_srx.materials.bin";
            var materialaram = "https://raw.communitydragon.org/latest/game/data/maps/mapgeometry/map12/base.materials.bin";
            var materialtft = "https://raw.communitydragon.org/latest/game/data/maps/mapgeometry/map22/base.materials.bin";

            WebClient client = new WebClient();
            client.DownloadFile(loadingscreen, "Downloads/loadingscreen/srbackground.csscraps.png");
            client.DownloadFile(materialsrx, "Downloads/SRX/base_srx.materials.bin");
            client.DownloadFile(materialaram, "Downloads/ARAM/base.materials.bin");
            client.DownloadFile(materialtft, "Downloads/TFT/base.materials.bin");

            if (!client.IsBusy)
            {
                ConsoleMain.AppendText("Extracting with Ritobin by moonshadow" + Environment.NewLine);
                var programpath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string input = programpath;
                string Path = input.Replace(@"\\", @"\") + @"\Downloads\SRX\base_srx.materials.bin";
                string Path2 = input.Replace(@"\\", @"\") + @"\Downloads\ARAM\base.materials.bin";
                string Path3 = input.Replace(@"\\", @"\") + @"\Downloads\TFT\base.materials.bin";
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();

                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                startInfo.FileName = "cmd.exe";
                startInfo.WorkingDirectory = "Tools/Ritobin/";
                startInfo.Arguments = @$"/c ritobin_cli.exe {Path} & ritobin_cli.exe {Path2} & ritobin_cli.exe {Path3}";

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                ConsoleMain.AppendText("Everything is Updated!" + Environment.NewLine);
            }

        }

        private void DeployCupCakes(object sender, RoutedEventArgs e)
        {
            ConsoleMain.Clear();
        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var flipview = ((FlipView)sender);
            switch (flipview.SelectedIndex)
            {
                case 0:
                    flipview.BannerText = "";
                    break;
                case 1:
                    flipview.BannerText = "";
                    break;
                case 2:
                    flipview.BannerText = "";
                    break;
            }
        }

        private void BtnSetProject_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                TbSetProject.Text = dialog.SelectedPath;
                ConsoleMain.AppendText("Project Folder set to: " + TbSetProject.Text + Environment.NewLine);
            }
        }

        private void BnSetProjectNameSR_Click(object sender, RoutedEventArgs e)
        {
            
            if (TbProjectNameSR.Text == string.Empty)
            {
                ProjectNameSR.Header = "Unknown Project";
            }
            else
            {
                ProjectNameSR.Header = TbProjectNameSR.Text.ToString();
            }
            
        }

        //Load the Project File\\
        private void BtnLoadProjectSR_Click(object sender, RoutedEventArgs e)
        {
            var programpath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @$"{programpath}\Projects";
            openFileDialog.Filter = "Avatar Project file (*.avproj)|*avproj";
            if (openFileDialog.ShowDialog() == true)
            {
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                if (lines.Contains("AvatarProjectVersion: 1"))
                {
                    var TxProjectNameSR = lines[1].Replace("Project Name SR: ", "");
                    var TxProjectNameARAM = lines[2].Replace("Project Name ARAM: ", "");
                    var TxProjectNameTFT = lines[3].Replace("Project Name TFT: ", "");
                    var TxProjectFolder = lines[5].Replace("Project Folder: ", "");
                    var TxSRLoadingscreen = lines[7].Replace("SRLoadingscreen: ", "");

                    TbProjectNameSR.Text = TxProjectNameSR;
                    ProjectNameSR.Header = TxProjectNameSR;
                    TbSetProject.Text = TxProjectFolder;
                    ImgLoadinscreen.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(TxSRLoadingscreen);
                }

                else
                {
                    this.ShowMessageAsync("Error:", "Your Project file is corrupted or not a valid file!");
                }

                

            }
                
        }

        private void BtnSaveProjectSR_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Avatar Project file (.avproj)|*.avproj";

            if (saveFileDialog.ShowDialog() == true)
            {
                var TxAvatarVersion = "AvatarProjectVersion: 1";
                var TxProjectNameSR = $"Project Name SR: {ProjectNameSR.Header}";
                var TxProjectNameARAM = $"Project Name ARAM: UnknownARAM";
                var TxProjectNameTFT = $"Project Name TFT: UnknownTFT";
                var TxProjectFolder = $"Project Folder: {TbSetProject.Text}";
                var TxSRLoadingscreen = $"SRLoadingscreen: {ImgLoadinscreen.Source}";

                var createText = TxAvatarVersion + Environment.NewLine + TxProjectNameSR + Environment.NewLine + TxProjectNameARAM + Environment.NewLine + TxProjectNameTFT + Environment.NewLine + Environment.NewLine + TxProjectFolder + Environment.NewLine + Environment.NewLine + TxSRLoadingscreen;
                File.WriteAllText(saveFileDialog.FileName, createText);
            }
            

        }


        private void BtnLoadingscreen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DDS Image (.dds)|*.dds|Riot Image Texture (.tex)|*.tex|PNG Image (.png)|*.png|All Supported images|*";
            File.Delete(@"F:\Users\theki\Source\Tools\RebornAvatar\RebornAvatar\bin\Debug\net6.0-windows\Projects\loadingscreen.dds");
            if (openFileDialog.ShowDialog() == true)
            {
                

                if (openFileDialog.FileName.Contains(".tex"))
                {
                    
                    using var fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                    TEX tes = new TEX(fs);
                    tes.ToDds("Projects/loadingscreen.dds");
                    byte[] fileContents = File.ReadAllBytes("Projects/loadingscreen.dds");
                    using (var stream = new MemoryStream(fileContents))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = stream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        ImgLoadinscreen.Source = bitmap;
                    }
                }
                else
                {
                    byte[] fileContents = File.ReadAllBytes(openFileDialog.FileName);
                    using (var stream = new MemoryStream(fileContents))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = stream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        ImgLoadinscreen.Source = bitmap;
                    }
                }
            }
        }

        private void TbSetProject_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbSetProject.Text == String.Empty)
            {
                SR.IsEnabled = false;
                ARAM.IsEnabled = false;
                TFT.IsEnabled = false;
                InfoPathSelect.Visibility = Visibility.Visible;
            }
            else
            {
                if (TbSetLCS.Text == String.Empty)
                {
                    SR.IsEnabled = false;
                }
                else
                {
                    SR.IsEnabled = true;
                    InfoPathSelect.Visibility = Visibility.Hidden;
                }
                
            }

        }

        private void BtnSunColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ColorPickerDialog
            {
                Owner = Owner,

            };

            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(21, 28, 39));
            dialog.Background = brush;
            var res = dialog.ShowDialog();
            SolidColorBrush currentColorA = new SolidColorBrush(System.Windows.Media.Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B));
            SolidColorBrush currentColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(dialog.Color.R, dialog.Color.G, dialog.Color.B));

            if (res == true)
            {
                var test = dialog.Color.ToColor();
                BtnSunColor.Background = currentColorA;
                BtnSunColor.BorderBrush = currentColor;
            }

        }

        private void BtnSkylightColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ColorPickerDialog
            {
                Owner = Owner,

            };

            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(21, 28, 39));
            dialog.Background = brush;
            var res = dialog.ShowDialog();
            SolidColorBrush currentColorA = new SolidColorBrush(System.Windows.Media.Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B));
            SolidColorBrush currentColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(dialog.Color.R, dialog.Color.G, dialog.Color.B));

            if (res == true)
            {
                var test = dialog.Color.ToColor();
                BtnSkylightColor.Background = currentColorA;
                BtnSkylightColor.BorderBrush = currentColor;
            }
        }

        private void BtnHorizonColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ColorPickerDialog
            {
                Owner = Owner,

            };

            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(21, 28, 39));
            dialog.Background = brush;
            var res = dialog.ShowDialog();
            SolidColorBrush currentColorA = new SolidColorBrush(System.Windows.Media.Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B));
            SolidColorBrush currentColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(dialog.Color.R, dialog.Color.G, dialog.Color.B));

            if (res == true)
            {
                var test = dialog.Color.ToColor();
                BtnHorizonColor.Background = currentColorA;
                BtnHorizonColor.BorderBrush = currentColor;
            }
        }

        private void BtnGroundColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ColorPickerDialog
            {
                Owner = Owner,

            };

            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(21, 28, 39));
            dialog.Background = brush;
            var res = dialog.ShowDialog();
            SolidColorBrush currentColorA = new SolidColorBrush(System.Windows.Media.Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B));
            SolidColorBrush currentColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(dialog.Color.R, dialog.Color.G, dialog.Color.B));

            if (res == true)
            {
                var test = dialog.Color.ToColor();
                BtnGroundColor.Background = currentColorA;
                BtnGroundColor.BorderBrush = currentColor;
            }
        }

        private void BtnFogColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ColorPickerDialog
            {
                Owner = Owner,

            };

            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(21, 28, 39));
            dialog.Background = brush;
            var res = dialog.ShowDialog();
            SolidColorBrush currentColorA = new SolidColorBrush(System.Windows.Media.Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B));
            SolidColorBrush currentColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(dialog.Color.R, dialog.Color.G, dialog.Color.B));

            if (res == true)
            {
                var test = dialog.Color.ToColor();
                BtnFogColor.Background = currentColorA;
                BtnFogColor.BorderBrush = currentColor;
            }
        }

        private void BtnFogAlternateColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ColorPickerDialog
            {
                Owner = Owner,

            };

            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(21, 28, 39));
            dialog.Background = brush;
            var res = dialog.ShowDialog();
            SolidColorBrush currentColorA = new SolidColorBrush(System.Windows.Media.Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B));
            SolidColorBrush currentColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(dialog.Color.R, dialog.Color.G, dialog.Color.B));

            if (res == true)
            {
                var test = dialog.Color.ToColor();
                BtnFogAlternateColor.Background = currentColorA;
                BtnFogAlternateColor.BorderBrush = currentColor;
            }
        }

        private void TbSunDirectionX_LostFocus(object sender, RoutedEventArgs e)
        {
            string result = "";
            char[] validChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.', '-' }; // these are ok
            foreach (char c in TbSunDirectionX.Text.Replace(".", ",")) // check each character in the user's input
            {
                if (Array.IndexOf(validChars, c) != -1)
                    result += c; // if this is ok, then add it to the result
            }
            
            var test = Math.Round(Convert.ToDouble(result), 6);
            TbSunDirectionX.Text = test.ToString(); // change the text to the "clean" version where illegal chars have been removed
            TbSunDirectionX.Text = TbSunDirectionX.Text.Replace(",", ".");
        }

        private void TbSunDirectionY_LostFocus(object sender, RoutedEventArgs e)
        {
            string result = "";
            char[] validChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.', '-' }; // these are ok
            foreach (char c in TbSunDirectionY.Text.Replace(".", ",")) // check each character in the user's input
            {
                if (Array.IndexOf(validChars, c) != -1)
                    result += c; // if this is ok, then add it to the result
            }
            var test = Math.Round(Convert.ToDouble(result), 6);
            TbSunDirectionY.Text = test.ToString(); ; // change the text to the "clean" version where illegal chars have been removed
            TbSunDirectionY.Text = TbSunDirectionY.Text.Replace(",", ".");
        }

        private void TbSunDirectionZ_LostFocus(object sender, RoutedEventArgs e)
        {
            string result = "";
            char[] validChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.', '-' }; // these are ok
            foreach (char c in TbSunDirectionZ.Text.Replace(".", ",")) // check each character in the user's input
            {
                if (Array.IndexOf(validChars, c) != -1)
                    result += c; // if this is ok, then add it to the result
            }
            var test = Math.Round(Convert.ToDouble(result), 6);
            TbSunDirectionZ.Text = test.ToString(); ; // change the text to the "clean" version where illegal chars have been removed
            TbSunDirectionZ.Text = TbSunDirectionZ.Text.Replace(",", ".");
        }

        private void TbSkylightScale_LostFocus(object sender, RoutedEventArgs e)
        {
            string result = "";
            char[] validChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.', '-' }; // these are ok
            foreach (char c in TbSkylightScale.Text.Replace(".", ",")) // check each character in the user's input
            {
                if (Array.IndexOf(validChars, c) != -1)
                    result += c; // if this is ok, then add it to the result
            }
            var test = Math.Round(Convert.ToDouble(result), 6);
            TbSkylightScale.Text = test.ToString(); ; // change the text to the "clean" version where illegal chars have been removed
            TbSkylightScale.Text = TbSkylightScale.Text.Replace(",", ".");
        }

        private void TbLightmapScale_LostFocus(object sender, RoutedEventArgs e)
        {
            string result = "";
            char[] validChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.', '-' }; // these are ok
            foreach (char c in TbLightmapScale.Text.Replace(".", ",")) // check each character in the user's input
            {
                if (Array.IndexOf(validChars, c) != -1)
                    result += c; // if this is ok, then add it to the result
            }
            var test = Math.Round(Convert.ToDouble(result), 6);
            TbLightmapScale.Text = test.ToString(); ; // change the text to the "clean" version where illegal chars have been removed
            TbLightmapScale.Text = TbLightmapScale.Text.Replace(",", ".");
        }

        private void TbFogEmissiveRemap_LostFocus(object sender, RoutedEventArgs e)
        {
            string result = "";
            char[] validChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.', '-' }; // these are ok
            foreach (char c in TbFogEmissiveRemap.Text.Replace(".", ",")) // check each character in the user's input
            {
                if (Array.IndexOf(validChars, c) != -1)
                    result += c; // if this is ok, then add it to the result
            }
            var test = Math.Round(Convert.ToDouble(result), 6);
            TbFogEmissiveRemap.Text = test.ToString(); ; // change the text to the "clean" version where illegal chars have been removed
            TbFogEmissiveRemap.Text = TbFogEmissiveRemap.Text.Replace(",", ".");
        }

        private void BtnLoadSunSR_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Avatar SunProperty (.avsun)|*.avsun";
            if (openFileDialog.ShowDialog() == true)
            {
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                if (lines.Contains("AvatarProjectVersion: 1"))
                {

                    var sunColor = lines[1].Split(' '); //4 5 6 7
                    var sunColorR = Convert.ToDouble(sunColor[4].Replace(",", "").Replace(".", ",")) * 255;
                    var sunColorG = Convert.ToDouble(sunColor[5].Replace(",", "").Replace(".", ",")) * 255;
                    var sunColorB = Convert.ToDouble(sunColor[6].Replace(",", "").Replace(".", ",")) * 255;
                    var sunColorA = Convert.ToDouble(sunColor[7].Replace(",", "").Replace(".", ",")) * 255;
                    BtnSunColor.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Convert.ToByte(sunColorA), Convert.ToByte(sunColorR), Convert.ToByte(sunColorG), Convert.ToByte(sunColorB)));

                    var sunDirection = lines[2].Split(' '); //4 5 6
                    var sunDirectionX = Convert.ToDouble(sunDirection[4].Replace(",", "").Replace(".", ","));
                    var sunDirectionY = Convert.ToDouble(sunDirection[5].Replace(",", "").Replace(".", ","));
                    var sunDirectionZ = Convert.ToDouble(sunDirection[6].Replace(",", "").Replace(".", ","));
                    TbSunDirectionX.Text = Convert.ToString(Math.Round(sunDirectionX, 6)).Replace(",", ".");
                    TbSunDirectionY.Text = Convert.ToString(Math.Round(sunDirectionY, 6)).Replace(",", ".");
                    TbSunDirectionZ.Text = Convert.ToString(Math.Round(sunDirectionZ, 6)).Replace(",", ".");
                    
                    
                    

                    var skyLightColor = lines[3].Split(' '); // 4 5 6 7
                    var skyLightColorR = Convert.ToDouble(skyLightColor[4].Replace(",", "").Replace(".", ",")) * 255;
                    var skyLightColorG = Convert.ToDouble(skyLightColor[5].Replace(",", "").Replace(".", ",")) * 255;
                    var skyLightColorB = Convert.ToDouble(skyLightColor[6].Replace(",", "").Replace(".", ",")) * 255;
                    var skyLightColorA = Convert.ToDouble(skyLightColor[7].Replace(",", "").Replace(".", ",")) * 255;
                    BtnSkylightColor.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Convert.ToByte(skyLightColorA), Convert.ToByte(skyLightColorR), Convert.ToByte(skyLightColorG), Convert.ToByte(skyLightColorB)));


                    var skyLightScale = lines[4].Split(' '); // 3
                    var skyLightScaleX = Convert.ToDouble(skyLightScale[3].Replace(",", "").Replace(".", ","));
                    TbSkylightScale.Text = Convert.ToString(Math.Round(skyLightScaleX, 6)).Replace(",", ".");

                    var horizonColor = lines[5].Split(' '); //4 5 6 7
                    var horizonColorR = Convert.ToDouble(horizonColor[4].Replace(",", "").Replace(".", ",")) * 255;
                    var horizonColorG = Convert.ToDouble(horizonColor[5].Replace(",", "").Replace(".", ",")) * 255;
                    var horizonColorB = Convert.ToDouble(horizonColor[6].Replace(",", "").Replace(".", ",")) * 255;
                    var horizonColorA = Convert.ToDouble(horizonColor[7].Replace(",", "").Replace(".", ",")) * 255;
                    BtnHorizonColor.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Convert.ToByte(horizonColorA), Convert.ToByte(horizonColorR), Convert.ToByte(horizonColorG), Convert.ToByte(horizonColorB)));

                    var groundColor = lines[6].Split(' '); //4 5 6 7
                    var groundColorR = Convert.ToDouble(groundColor[4].Replace(",", "").Replace(".", ",")) * 255;
                    var groundColorG = Convert.ToDouble(groundColor[5].Replace(",", "").Replace(".", ",")) * 255;
                    var groundColorB = Convert.ToDouble(groundColor[6].Replace(",", "").Replace(".", ",")) * 255;
                    var groundColorA = Convert.ToDouble(groundColor[7].Replace(",", "").Replace(".", ",")) * 255;
                    BtnGroundColor.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Convert.ToByte(groundColorA), Convert.ToByte(groundColorR), Convert.ToByte(groundColorG), Convert.ToByte(groundColorB)));

                    var lightMapColorScale = lines[7].Split(' '); //3
                    var lightMapColorScaleX = Convert.ToDouble(lightMapColorScale[3].Replace(",", "").Replace(".", ","));
                    TbLightmapScale.Text = Convert.ToString(Math.Round(lightMapColorScaleX, 6)).Replace(",", ".");

                    var fogEnabled = lines[8].Split(' '); //3
                    BtnEnableFog.IsChecked = Convert.ToBoolean(fogEnabled[3].ToString());

                    var fogStartAndEnd = lines[9].Split(' '); //4 5
                    SliderFogStart.Value = Convert.ToDouble(fogStartAndEnd[4].Replace(",", "").Replace(".", ","));
                    SliderFogEnd.Value = Convert.ToDouble(fogStartAndEnd[5].Replace(",", "").Replace(".", ","));

                    var fogColor = lines[10].Split(' '); //4 5 6 7
                    var fogColorR = Convert.ToDouble(fogColor[4].Replace(",", "").Replace(".", ",")) * 255;
                    var fogColorG = Convert.ToDouble(fogColor[5].Replace(",", "").Replace(".", ",")) * 255;
                    var fogColorB = Convert.ToDouble(fogColor[6].Replace(",", "").Replace(".", ",")) * 255;
                    var fogColorA = Convert.ToDouble(fogColor[7].Replace(",", "").Replace(".", ",")) * 255;
                    BtnFogColor.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Convert.ToByte(fogColorA), Convert.ToByte(fogColorR), Convert.ToByte(fogColorG), Convert.ToByte(fogColorB)));

                    var fogAlternateColor = lines[11].Split(' '); //4 5 6 7
                    var fogAlternateColorR = Convert.ToDouble(fogAlternateColor[4].Replace(",", "").Replace(".", ",")) * 255;
                    var fogAlternateColorG = Convert.ToDouble(fogAlternateColor[5].Replace(",", "").Replace(".", ",")) * 255;
                    var fogAlternateColorB = Convert.ToDouble(fogAlternateColor[6].Replace(",", "").Replace(".", ",")) * 255;
                    var fogAlternateColorA = Convert.ToDouble(fogAlternateColor[7].Replace(",", "").Replace(".", ",")) * 255;
                    BtnFogAlternateColor.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Convert.ToByte(fogAlternateColorA), Convert.ToByte(fogAlternateColorR), Convert.ToByte(fogAlternateColorG), Convert.ToByte(fogAlternateColorB)));

                    var fogEmissiveRemap = lines[12].Split(' '); //3
                    var fogEmissiveRemapX = Convert.ToDouble(fogEmissiveRemap[3].Replace(",", "").Replace(".", ","));
                    TbFogEmissiveRemap.Text = Convert.ToString(Math.Round(fogEmissiveRemapX, 6)).Replace(",", ".");

                    var useBloom = lines[13].Split(' '); //3
                    BtnEnableBloom.IsChecked = Convert.ToBoolean(useBloom[3].ToString());

                }


            }
        }

        private void BtnSaveSunSR_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Avatar SunProperty (.avsun)|*.avsun";
            if (saveFileDialog.ShowDialog() == true)
            {
                

                var TxAvatarVersion = "AvatarProjectVersion: 1";
                var color = ((SolidColorBrush)BtnSunColor.Background).Color;
                var color1 = ((SolidColorBrush)BtnSkylightColor.Background).Color;
                var color2 = ((SolidColorBrush)BtnHorizonColor.Background).Color;
                var color3 = ((SolidColorBrush)BtnGroundColor.Background).Color;
                var color4 = ((SolidColorBrush)BtnFogColor.Background).Color;
                var color5 = ((SolidColorBrush)BtnFogAlternateColor.Background).Color;

                var sunColor = "sunColor: vec4 = " + "{ " + $"{Math.Round(color.R / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color.G / 255.0,6)}".Replace(",", ".") + ", " + $"{Math.Round(color.B / 255.0,6)}".Replace(",", ".") + ", " + $"{Math.Round(color.A / 255.0,6)}".Replace(",", ".") + " }";
                var sunDirection = "sunDirection: vec3 = " + "{ " + $"{TbSunDirectionX.Text}, {TbSunDirectionY.Text}, {TbSunDirectionZ.Text} " + " }";
                var skyLightColor = "skyightColor: vec4 = " + "{ " + $"{Math.Round(color1.R / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color1.G / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color1.B / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color1.A / 255.0, 6)}".Replace(",", ".") + " }";
                var skyLightScale = $"skyLightScale: f32 = {TbSkylightScale.Text}";
                var horizonColor = "horizonColor: vec4 = " + "{ " + $"{Math.Round(color2.R / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color2.G / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color2.B / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color2.A / 255.0, 6)}".Replace(",", ".") + " }";
                var groundColor = "groundColor: vec4 = " + "{ " + $"{Math.Round(color3.R / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color3.G / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color3.B / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color3.A / 255.0, 6)}".Replace(",", ".") + " }";
                var lightMapColorScale = $"lightMapColorScale: f32 = {TbLightmapScale.Text}";
                var fogEnabled = $"fogEnabled: bool = {Convert.ToString(BtnEnableFog.IsChecked.Value).ToLower()}";
                var fogStartAndEnd = "fogStartAndEnd: vec2 = " + "{ " + $"{SliderFogStart.Value}, {SliderFogEnd.Value}" + " }";
                var fogColor = "fogColor: vec4 = " + "{ " + $"{Math.Round(color4.R / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color4.G / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color4.B / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color4.A / 255.0, 6)}".Replace(",", ".") + " }";
                var fogAlternateColor = "fogAlternateColor: vec4 = " + "{ " + $"{Math.Round(color5.R / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color5.G / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color5.B / 255.0, 6)}".Replace(",", ".") + ", " + $"{Math.Round(color5.A / 255.0, 6)}".Replace(",", ".") + " }";
                var fogEmissiveRemap = $"fogEmissiveRemap: f32 = {TbFogEmissiveRemap.Text}";
                var useBloom = $"useBloom: bool = {Convert.ToString(BtnEnableBloom.IsChecked.Value).ToLower()}";

                var createText = TxAvatarVersion + Environment.NewLine + 
                    sunColor + Environment.NewLine + 
                    sunDirection + Environment.NewLine + 
                    skyLightColor + Environment.NewLine + 
                    skyLightScale + Environment.NewLine + 
                    horizonColor + Environment.NewLine + 
                    groundColor + Environment.NewLine + 
                    lightMapColorScale + Environment.NewLine +
                    fogEnabled + Environment.NewLine +
                    fogStartAndEnd + Environment.NewLine +
                    fogColor + Environment.NewLine +
                    fogAlternateColor + Environment.NewLine +
                    fogEmissiveRemap + Environment.NewLine +
                    useBloom;
                File.WriteAllText(saveFileDialog.FileName, createText);
            }
        }

        private async void BtnLoadMapgeo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "League of Legends Map File (.mapgeo)|*.mapgeo";

            if (openFileDialog.ShowDialog() == true)
            {
                LayerEditor.Items.Clear();
                
                MapGeometry map = new MapGeometry(openFileDialog.FileName);
                mapfile = map;
                string path = $"Maps/KitPieces/SRX/Materials/{ProjectNameSR.Header}/";



                foreach (var file in map.Meshes)
                {
                    if (!file.Submeshes[0].Material.Contains("/Materials/"))
                    {
                        //file.Submeshes[0].Material = file.Submeshes[0].Material.Replace(file.Submeshes[0].Material, $"{path}{file.Submeshes[0].Material}");
                    }


                    TreeViewItem Name = new TreeViewItem();

                    Name.Header = file.Submeshes[0].Material;
                    
                   
                    LayerEditor.Items.Add(Name);

                    mapmodel = file;
                }
                var res = await InfoBox.ShowMessageAsync("Info:", "Do you want to use the exported Maya JSON by tarngaina?", MessageDialogStyle.AffirmativeAndNegative);
                if (res != MessageDialogResult.Affirmative)
                {
                    List<string> strings = new List<string>();
                    List<string> onlymats = new List<string>();
                    Material2Tree.GetMaterialMapgeo(System.IO.Path.GetDirectoryName(openFileDialog.FileName), System.IO.Path.GetFileName(openFileDialog.FileName), strings, onlymats);
                    List<string> onlymat = new List<string>();
                    materiallist = strings;
                }
                else
                {
                    Material2Tree.MayaJson(MayaTb);
                }
                
                pathofmapgeo = System.IO.Path.GetDirectoryName(openFileDialog.FileName);

                
            }

            
            

            Settings.IsEnabled = true;
        }

        private void LayerEditor_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //Try Loading mapgeo bin file
            try
            {
                MaterialCode.Text = "";
                var test = LayerEditor.Items.IndexOf(LayerEditor.SelectedItem);
                listnumber = test;
                //TbObjectName.Text = mapfile.Models[test].Name;
                //TbVertices.Text = mapfile.Models[test].Vertices.Count.ToString();
                //TbIndices.Text = mapfile.Models[test].Indices.Count.ToString();

                //Transformation
                //TbTransX.Text = mapfile.Models[test].Transformation.Translation.X.ToString();
                //TbTransY.Text = mapfile.Models[test].Transformation.Translation.Y.ToString();
                //TbTransZ.Text = mapfile.Models[test].Transformation.Translation.Z.ToString();
                //Rotation
                //TbRotX.Text = mapfile.Models[test].Transformation.Rotation.X.ToString();
                //TbRotY.Text = mapfile.Models[test].Transformation.Rotation.Y.ToString();
                //TbRotZ.Text = mapfile.Models[test].Transformation.Rotation.Z.ToString();
                //Scale
                //TbScaleX.Text = mapfile.Models[test].Transformation.Scale.X.ToString();
                //TbScaleY.Text = mapfile.Models[test].Transformation.Scale.Y.ToString();
                //TbScaleZ.Text = mapfile.Models[test].Transformation.Scale.Z.ToString();

                //TbMaterialName.Text = mapfile.Models[test].Submeshes[0].Material.ToString();
                //if (mapfile.Meshes[test].FlipNormals == true)
                //{
                //    CbFlippedNormals.IsChecked = true;
                //}
                //else
                //{
                //    CbFlippedNormals.IsChecked = false;
                //}


                //if ((mapfile.Meshes[test].MeshRenderFlags & MapGeometryMeshRenderFlags.HighRenderPriority) == MapGeometryMeshRenderFlags.HighRenderPriority)
                //{
                //    CobFlags.SelectedIndex = 0;
                //}

                //if ((mapfile.Models[test].Layer & MapGeometryLayer.Layer1) == MapGeometryLayer.Layer1)
                //{
                //    CbLayer1.IsChecked = true;
                //}
                //else
                //{
                //    CbLayer1.IsChecked= false;
                //}

                //if ((mapfile.Models[test].Layer & MapGeometryLayer.Layer2) == MapGeometryLayer.Layer2)
                //{
                //    CbLayer2.IsChecked = true;
                //}
                //else
                //{
                //    CbLayer2.IsChecked = false;
                //}
                //if ((mapfile.Models[test].Layer & MapGeometryLayer.Layer3) == MapGeometryLayer.Layer3)
                //{
                //    CbLayer3.IsChecked = true;
                //}
                //else
                //{
                //    CbLayer3.IsChecked = false;
                //}
                //if ((mapfile.Models[test].Layer & MapGeometryLayer.Layer4) == MapGeometryLayer.Layer4)
                //{
                //    CbLayer4.IsChecked = true;
                //}
                //else
                //{
                //    CbLayer4.IsChecked = false;
                //}
                //if ((mapfile.Models[test].Layer & MapGeometryLayer.Layer5) == MapGeometryLayer.Layer5)
                //{
                //    CbLayer5.IsChecked = true;
                //}
                //else
                //{
                //    CbLayer5.IsChecked = false;
                //}
                //if ((mapfile.Models[test].Layer & MapGeometryLayer.Layer6) == MapGeometryLayer.Layer6)
                //{
                //    CbLayer6.IsChecked = true;
                //}
                //else
                //{
                //    CbLayer6.IsChecked = false;
                //}
                //if ((mapfile.Models[test].Layer & MapGeometryLayer.Layer7) == MapGeometryLayer.Layer7)
                //{
                //    CbLayer7.IsChecked = true;
                //}
                //else
                //{
                //    CbLayer7.IsChecked = false;
                //}
                //if ((mapfile.Models[test].Layer & MapGeometryLayer.Layer8) == MapGeometryLayer.Layer8)
                //{
                //    CbLayer8.IsChecked = true;
                //}
                //else
                //{
                //    CbLayer8.IsChecked = false;
                //}
                //TbLightmap.Text = mapfile.Models[test].BakedLightTexture;
                //TbBakedPaintTexture.Text = mapfile.Models[test].BakedPaintTexture;

                //TbColorR.Text = mapfile.Models[test].Color.R.ToString();
                //TbColorG.Text = mapfile.Models[test].Color.G.ToString();
                //TbColorB.Text = mapfile.Models[test].Color.B.ToString();
                //TbColorA.Text = mapfile.Models[test].Color.A.ToString();

                //TbBakedPaintColorR.Text = mapfile.Models[test].BakedPaintColor.R.ToString();
                //TbBakedPaintColorG.Text = mapfile.Models[test].BakedPaintColor.G.ToString();
                //TbBakedPaintColorB.Text = mapfile.Models[test].BakedPaintColor.B.ToString();
                //TbBakedPaintColorA.Text = mapfile.Models[test].BakedPaintColor.A.ToString();


                //Material Editor\\
                //var matname = mapfile.Models[test].Submeshes[0].Material;
                //string result = materiallist.FirstOrDefault(s => s.Contains(matname));
                //string result2 = materiallistmaya.FirstOrDefault(s => s.Contains(matname));
                //MaterialCode.Text = result;
                //MaterialCode.Text = result2;
                //Get the ASSETS Folder of selected MAPGEO File
                
                try
                {
                    //pathofmapgeo
                    string newPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(pathofmapgeo, @"..\..\..\..\"));
                    string lightmappath = newPath  + TbLightmap.Text.Replace("/", @"\");
                    File.WriteAllText("Temps/temps.txt", MaterialCode.Text);
                    string[] lines = File.ReadAllLines("Temps/temps.txt");
                    //Diffuse
                    if (MaterialCode.Text.Contains("Diffuse"))
                    {
                        var DiffuseName = lines[7].Replace("                textureName: string = ", "").Replace(@"\","");
                        byte[] fileContents = File.ReadAllBytes(newPath + DiffuseName.Replace("/", @"\").Replace("\"", ""));
                        using (var stream = new MemoryStream(fileContents))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = stream;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            Diffuse_Tex.Source = bitmap;
                        }
                    }
                    //Emissive
                    if (MaterialCode.Text.Contains("Emissive"))
                    {
                        var EmissiveName = lines[14].Replace("                textureName: string = ", "").Replace(@"\", "");
                        byte[] fileContents = File.ReadAllBytes(newPath + EmissiveName.Replace("/", @"\").Replace("\"", ""));
                        using (var stream = new MemoryStream(fileContents))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = stream;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            Emissive_Tex.Source = bitmap;
                        }
                    }
                    //Lightmap
                    if (TbLightmap.Text != null)
                    {
                        byte[] fileContents = File.ReadAllBytes(lightmappath);
                        using (var stream = new MemoryStream(fileContents))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = stream;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            Lightmap_Tex.Source = bitmap;
                        }
                    }
                    
                }
                catch(Exception ex)
                {
                    //pathofmapgeo
                    string newPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(pathofmapgeo, @"..\..\..\..\"));
                    string lightmappath = newPath + TbLightmap.Text.Replace("/", @"\");
                    File.WriteAllText("Temps/temps.txt", MaterialCode.Text);
                    string[] lines = File.ReadAllLines("Temps/temps.txt");
                    //Diffuse
                    if (MaterialCode.Text.Contains("Diffuse"))
                    {
                        var DiffuseName = lines[6].Replace("                textureName: string = ", "").Replace(@"\", "");
                        byte[] fileContents = File.ReadAllBytes(newPath + DiffuseName.Replace("/", @"\").Replace("\"", ""));
                        using (var stream = new MemoryStream(fileContents))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = stream;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            Diffuse_Tex.Source = bitmap;
                        }
                    }
                    //Emissive
                    if (MaterialCode.Text.Contains("Emissive"))
                    {
                        var EmissiveName = lines[13].Replace("                textureName: string = ", "").Replace(@"\", "");
                        byte[] fileContents = File.ReadAllBytes(newPath + EmissiveName.Replace("/", @"\").Replace("\"", ""));
                        using (var stream = new MemoryStream(fileContents))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = stream;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            Emissive_Tex.Source = bitmap;
                        }
                    }
                    //Mask
                    if (MaterialCode.Text.Contains("Mask"))
                    {
                        var EmissiveName = lines[13].Replace("                textureName: string = ", "").Replace(@"\", "");
                        byte[] fileContents = File.ReadAllBytes(newPath + EmissiveName.Replace("/", @"\").Replace("\"", ""));
                        using (var stream = new MemoryStream(fileContents))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = stream;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            Emissive_Tex.Source = bitmap;
                        }
                    }
                    //Lightmap
                    if (TbLightmap.Text != null)
                    {
                        byte[] fileContents = File.ReadAllBytes(lightmappath);
                        using (var stream = new MemoryStream(fileContents))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = stream;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            Lightmap_Tex.Source = bitmap;
                        }
                    }
                }
                

            }
            catch
            {
                
            }
        }

        private void BtnSaveMapgeo_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "League of Legends Map File (.mapgeo)|*.mapgeo";
            if (saveFileDialog.ShowDialog() == true)
            {

                try
                {
                    MapGeometry maporiginal = new MapGeometry("Templates/Maps/SRX/base_srx.mapgeo");
                    //We will copy Original Bucket Grids from original SRX Map otherwise it will crash
                    //mapfile.BucketGrid = maporiginal.BucketGrid;
                    //mapfile.Write(saveFileDialog.FileName, 13);
                }
                    
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Error:", MessageBoxButtons.OK);
                }
            }
        }

        private void CobShader_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //We need all shaders first
        }

        private void BtnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            //ObjectName
            //mapfile.Models[listnumber].Name = TbObjectName.Text;
            //Transformation
            var x = float.Parse(TbTransX.Text);
            var y = float.Parse(TbTransY.Text);
            var z = float.Parse(TbTransZ.Text);

            Vector3 vector = new Vector3(x, y, z);
            Matrix4x4 transformation = Matrix4x4.CreateTranslation(vector);
            R3DMatrix44 r3D = new R3DMatrix44();
            //MaterialName
            //mapfile.Models[listnumber].Submeshes[0].Material = TbMaterialName.Text;
            //Flip Normals
            //if (CbFlippedNormals.IsChecked == true)
            //{
            //    mapfile.Models[listnumber].FlipNormals = true;
            //}
            //if (CbFlippedNormals.IsChecked == false)
            //{
            //    mapfile.Models[listnumber].FlipNormals = false;
            //}
            ////Layers
            //mapfile.Models[listnumber].Layer = MapGeometryLayer.NoLayer;

            //MapGeometryLayer layer1 = MapGeometryLayer.Layer1;
            //MapGeometryLayer layer2 = MapGeometryLayer.Layer2;
            //MapGeometryLayer layer3 = MapGeometryLayer.Layer3;
            //MapGeometryLayer layer4 = MapGeometryLayer.Layer4;
            //MapGeometryLayer layer5 = MapGeometryLayer.Layer5;
            //MapGeometryLayer layer6 = MapGeometryLayer.Layer6;
            //MapGeometryLayer layer7 = MapGeometryLayer.Layer7;
            //MapGeometryLayer layer8 = MapGeometryLayer.Layer8;

            //if (CbLayer1.IsChecked == true)
            //{
            //    mapfile.Models[listnumber].Layer = mapfile.Models[listnumber].Layer | layer1;
            //}
            //if (CbLayer2.IsChecked == true)
            //{
            //    mapfile.Models[listnumber].Layer = mapfile.Models[listnumber].Layer | layer2;
            //}
            //if (CbLayer3.IsChecked == true)
            //{
            //    mapfile.Models[listnumber].Layer = mapfile.Models[listnumber].Layer | layer3;
            //}
            //if (CbLayer4.IsChecked == true)
            //{
            //    mapfile.Models[listnumber].Layer = mapfile.Models[listnumber].Layer | layer4;
            //}
            //if (CbLayer5.IsChecked == true)
            //{
            //    mapfile.Models[listnumber].Layer = mapfile.Models[listnumber].Layer | layer5;
            //}
            //if (CbLayer6.IsChecked == true)
            //{
            //    mapfile.Models[listnumber].Layer = mapfile.Models[listnumber].Layer | layer6;
            //}
            //if (CbLayer7.IsChecked == true)
            //{
            //    mapfile.Models[listnumber].Layer = mapfile.Models[listnumber].Layer | layer7;
            //}
            //if (CbLayer8.IsChecked == true)
            //{
            //    mapfile.Models[listnumber].Layer = mapfile.Models[listnumber].Layer | layer8;
            //}

            //Lightmap
            //mapfile.Models[listnumber].Lightmap = TbLightmap.Text;
            //Lightmap UV Offset
            //mapfile.Models[listnumber].BakedPaintTexture = TbBakedPaintTexture.Text;

            var red = float.Parse(TbColorR.Text);
            var green = float.Parse(TbColorG.Text);
            var blue = float.Parse(TbColorB.Text);
            var alpha = float.Parse(TbColorA.Text);
            LeagueToolkit.Helpers.Structures.Color color = new LeagueToolkit.Helpers.Structures.Color(red, green, blue, alpha);
            //mapfile.Models[listnumber].Color = color;
            //BakedPaint UV Offset
            var red2 = float.Parse(TbBakedPaintColorR.Text);
            var green2 = float.Parse(TbBakedPaintColorG.Text);
            var blue2 = float.Parse(TbBakedPaintColorB.Text);
            var alpha2 = float.Parse(TbBakedPaintColorA.Text);
            LeagueToolkit.Helpers.Structures.Color color2 = new LeagueToolkit.Helpers.Structures.Color(red2, green2, blue2, alpha2);
            //mapfile.Models[listnumber].Color = color2;
        }

        private void BnSetProjectThumbnail_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.dds;*.png;*.jpg)|*.dds;*.png;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                TbProjectThumbnail.Text = openFileDialog.FileName;
            }
        }

        private void BtnSetLCS_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Custom Skin Manager (.exe)|cslol-manager.exe";
            var oldpath = TbSetLCS.Text;
            if (openFileDialog.ShowDialog() == true)
            {
                TbSetLCS.Text = openFileDialog.FileName;
                var changesettings = File.ReadAllText("Projects/default.avproj");
                var settings = changesettings.Replace($"CSManager Folder: {oldpath}", $"CSManager Folder: {TbSetLCS.Text}");
                File.WriteAllText("Projects/default.avproj", settings);
                ConsoleMain.AppendText("CSManager Folder set to: " + TbSetLCS.Text + Environment.NewLine);
            }
        }

        private void TbSetLCS_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbSetLCS.Text == String.Empty)
            {
                SR.IsEnabled = false;
                ARAM.IsEnabled = false;
                TFT.IsEnabled = false;
                InfoPathSelect.Visibility = Visibility.Hidden;
            }
            else
            {
                if (TbSetProject.Text == String.Empty)
                {
                    SR.IsEnabled = false;
                }
                else
                {
                    SR.IsEnabled = true;
                    InfoPathSelect.Visibility = Visibility.Hidden;
                }

            }
        }

        private void BtnExportProjectSR_Click(object sender, RoutedEventArgs e)
        {
            var projectname = TbProjectNameSR.Text;
            var lcspath = TbSetLCS.Text;
            var projectpath = TbSetProject.Text;
            var base_srxpath = $"{projectpath}SR/{projectname}/MAP11/data/maps/mapgeometry/map11";
            var imagespath = $"{projectpath}SR/{projectname}/MAP11/assets/maps/kitpieces/srx/{projectname.ToLower()}/textures";
            var exporthash = $"{projectpath}SR/{projectname}/hash.txt";
            var exportsun = $"{projectpath}SR/{projectname}/sunprops.avsun";
            var exportproject = $"{projectpath}SR/{projectname}/settings.avproject";
            var exportmeta = $"{projectpath}SR/{projectname}/csm_meta.json";
            var root = $"{projectpath}SR/{projectname}";
            //Export to Avatar first\\

            Directory.CreateDirectory(base_srxpath);
            Directory.CreateDirectory(imagespath);
            File.WriteAllText(exporthash, "hashes");
            File.WriteAllText(exportsun, "SunProps");
            File.WriteAllText(exportproject, "ProjectStuff");
            File.Copy(TbProjectThumbnail.Text, root + "/thumbnail.png");
        }

        private void CreateWad_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                
            }
        }

        private void BtnConvertMat_Click(object sender, RoutedEventArgs e)
        {
            Material2Tree.Convert2Lol(MayaTb, materiallistmaya, MaterialCode, ProjectNameSR, BtnEnableFog, CbNoLightmap);
        }

       
        

    }
}
