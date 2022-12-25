using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Intersoft.Crosslight;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;

namespace RebornAvatar.IO.Materials
{
    public static class Material2Tree
    {
        
        //OpenFileDialog opf
        public static void GetMaterialMapgeo(string mapgeopath, string mapgeoname, List<string> matlist, List<string> onlymats)
        {
            string materialpath = mapgeopath.Replace(@"\\", @"\");
            string matname = mapgeoname.Replace(".mapgeo", ".materials.bin");
            string pyname = matname.Replace(".materials.bin", ".materials.py");

            var path = "Tools/Ritobin/ritobin_cli.exe";
            var pi = new ProcessStartInfo(path)
            {
                Arguments = Path.GetFileName(path),
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(path),
                FileName = materialpath + @"\" + matname,
                Verb = "OPEN"
            };
            var tes = Process.Start(pi);
            tes.WaitForExit();
            //Convert bin to py

            var start = "Maps/KitPieces";
            var end =
            "}\n" +
            "            }\n" +
            "        }\n" +
            "    }\n" +
            "    ";
            
            var body = File.ReadAllText(materialpath + @"\" + pyname);
            var test = matlist;
            int indexStart = 0;
            int indexEnd = 0;

            bool exit = false;
            while (!exit)
            {
                indexStart = body.IndexOf(start);

                if (indexStart != -1)
                {
                    indexEnd = indexStart + body.Substring(indexStart).IndexOf(end);
                    
                    matlist.Add("Maps/KitPieces" + body.Substring(indexStart + start.Length, indexEnd - indexStart - start.Length) + "}\n" +
"            }\n" +
"        }\n" +
"    }\n" +
"    ");

                    body = body.Substring(indexEnd + end.Length + 1);
                    
                }
                else
                {
                    exit = true;
                }
            }

        }
        public static void Convert2Lol(TextBox textBox, List<string> mayaList, TextBox output, TabItem projectNameItem, CheckBox EnableFog, CheckBox NoLightmap)
        {
            //First we need to get the content
            string input = textBox.Text;

            //Load Shaders
            string Default_Emissive = File.ReadAllText("Shaders/Default_Emissive.py");
            string SRX_Blend_Hextech_Island = File.ReadAllText("Shaders/SRX_Blend_Hextech_Island.py");
            string Emissive_Basic = File.ReadAllText("Shaders/Emissive_Basic.py");
            string FlickerAlpha_FlipBook = File.ReadAllText("Shaders/FlickerAlpha_FlipBook.py");
            string Hologram = File.ReadAllText("Shaders/Hologram.py");
            string Hologram_Rotate = File.ReadAllText("Shaders/Hologram_Rotate.py");
            string SinFade_Alpha = File.ReadAllText("Shaders/SinFade_Alpha.py");
            string SRXBlendMaster = File.ReadAllText("Shaders/SRX_Blend_Master.py");


            //Now we want to make a list
            input = String.Join("", input.Split('"'));

            Regex regex = new Regex(@"(?<=\{)[^}]*(?=\})", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(input);

            // Results include braces (undesirable)
            var result = matches.Cast<Match>().Select(m => m.Value).Distinct().ToList();
            foreach (var mat in result)
            {
                
                string[] test = mat.Split(' ').ToArray();
                if (test[18].Contains("diffuse"))
                {
                    var matnamee = test[9].Replace(",\r\n", "");
                    var type = test[18].Replace(",\r\n", "");
                    var diffusetex = test[27].Replace("\r\n", "");

                    string material = SRXBlendMaster;
                    string mat1 = "";
                    string mat2 = "";
                    string mat3 = "";
                    string mat4 = "";

                    //Replace Material Name
                    if (!matnamee.Contains("/Materials/"))
                    {
                        mat1 = material.Replace("Material_Name", $"Maps/KitPieces/SRX/{projectNameItem.Header}/{matnamee}");
                    }
                    else
                    {
                        mat1 = material.Replace("Material_Name", $"{matnamee}");
                    }
                    //Replace Diffuse
                    mat2 = mat1.Replace("Texture_Name", diffusetex);

                    //Replace No Baked Lighting
                    if (NoLightmap.IsChecked == true)
                    {
                        mat3 = mat2.Replace("NOBAKEDLIGHTINGTEMP", "\"NO_BAKED_LIGHTING\" = \"1\"");
                    }
                    else
                    {
                        
                        mat3 = mat2.Replace("NOBAKEDLIGHTINGTEMP", "");
                    }
                    //---------------------------------------\\
                    //Replace Fog
                    if (EnableFog.IsChecked == false)
                    {
                        mat4 = mat3.Replace("DISABLEDEPTHFOG", "\"DISABLE_DEPTH_FOG\" = \"1\"");
                    }
                    else
                    {
                        
                        mat4 = mat3.Replace("DISABLEDEPTHFOG", "");
                    }

                    output.AppendText(mat4 + Environment.NewLine);
                }

                if (test[18].Contains("emissive"))
                {
                    //DefaultEmissive
                    if (test[26].Contains("diffuse_texture") && test[35].Contains("emissive_texture"))
                    {
                        var matnamee = test[9].Replace(",\r\n", "");
                        var type = test[18].Replace(",\r\n", "");
                        var diffusetex = test[27].Replace(",\r\n", "");
                        var emissivetex = test[36].Replace("\r\n", "");

                        string material = SRX_Blend_Hextech_Island; //Default Emissive was older one
                        string mat1 = "";
                        string mat2 = "";
                        string mat3 = "";
                        string mat4 = "";
                        string mat5 = "";

                        //Replace Material Name
                        if (!matnamee.Contains("/Materials/"))
                        {
                            mat1 = material.Replace("Material_Name", $"Maps/KitPieces/SRX/{projectNameItem.Header}/{matnamee}");
                        }
                        else
                        {
                            mat1 = material.Replace("Material_Name", $"{matnamee}");
                        }


                        //Replace Diffuse
                        mat2 = mat1.Replace("Diffuse_Name", diffusetex);

                        //Replace Emissive
                        mat3 = mat2.Replace("Emissive_Name", emissivetex);

                        //Replace No Baked Lighting
                        if (NoLightmap.IsChecked == true)
                        {
                            mat4 = mat3.Replace("NOBAKEDLIGHTINGTEMP", "\"NO_BAKED_LIGHTING\" = \"1\"");
                        }
                        else
                        {

                            mat4 = mat3.Replace("NOBAKEDLIGHTINGTEMP", "");
                        }
                        //---------------------------------------\\
                        //Replace Fog
                        if (EnableFog.IsChecked == false)
                        {
                            mat5 = mat4.Replace("DISABLEDEPTHFOG", "\"DISABLE_DEPTH_FOG\" = \"1\"");
                        }
                        else
                        {

                            mat5 = mat4.Replace("DISABLEDEPTHFOG", "");
                        }
                        output.AppendText(mat5 + Environment.NewLine);
                    }

                    //EmissiveBasic
                    if (test[26].Contains("emissive_color"))
                    {
                        var matnamee = test[9].Replace(",\r\n", "");
                        var type = test[18].Replace(",\r\n", "");
                        var red = test[39].Replace(",\r\n", "");
                        var green = test[51].Replace(",\r\n", "");
                        var blue = test[63].Replace("\r\n", "");

                        string material = Emissive_Basic;
                        string mat1 = "";
                        string mat2 = "";

                        //Replace Material Name
                        if (!matnamee.Contains("/Materials/"))
                        {
                            mat1 = material.Replace("Material_Name", $"Maps/KitPieces/SRX/{projectNameItem.Header}/{matnamee}");
                        }
                        else
                        {
                            mat1 = material.Replace("Material_Name", $"{matnamee}");
                        }
                        //Replace Color
                        mat2 = mat1.Replace("EM_Red, EM_Green, EM_Blue", $"{red}, {green}, {blue}");
                        output.AppendText(mat2 + Environment.NewLine);
                    }

                    //SinFade_Alpha
                    if (test[26].Contains("diffuse_texture") && test[35].Contains("emissive_color"))
                    {
                        var matnamee = test[9].Replace(",\r\n", "");
                        var type = test[18].Replace(",\r\n", "");
                        var maskname = test[27].Replace(",\r\n", "");
                        var red = test[48].Replace(",\r\n", "");
                        var green = test[60].Replace(",\r\n", "");
                        var blue = test[72].Replace("\r\n", "");

                        string material = SinFade_Alpha;
                        string mat1 = "";
                        string mat2 = "";
                        string mat3 = "";

                        //Replace Material Name
                        if (!matnamee.Contains("/Materials/"))
                        {
                            mat1 = material.Replace("Material_Name", $"Maps/KitPieces/SRX/{projectNameItem.Header}/{matnamee}");
                        }
                        else
                        {
                            mat1 = material.Replace("Material_Name", $"{matnamee}");
                        }

                        mat2 = mat1.Replace("Mask_Name", maskname);
                        //Replace Color
                        mat3 = mat2.Replace("EM_Red, EM_Green, EM_Blue", $"{red}, {green}, {blue}");
                        output.AppendText(mat3 + Environment.NewLine);
                    }
                }
                
            }
            

        }

        public static async void MayaJson(TextBox textBox)
        {
            
            //First load the file\\
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Maya Map Material (.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                
                var body = File.ReadAllText(openFileDialog.FileName);
                textBox.Text = body;
            }
            
        }
        
        
    }
    
}
