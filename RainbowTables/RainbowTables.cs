using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RainbowTables
{
    public partial class RainbowTables : Form
    {
        MD5CryptoServiceProvider MD5hash;
        Random random;
        const String NUMERIC = "0123456789";
        const String ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const String LOWERALPHA = "abcdefghijklmnopqrstuvwxyz";


        public RainbowTables()
        {
            random = new Random();
            MD5hash = new MD5CryptoServiceProvider();
            InitializeComponent();
        }

        private void GenerateValues_Button_Click(object sender, EventArgs e)
        {
            List<String> plainValues = GenerateRandomStringList((int) GeneratedValuesNumber_NumericUpDown.Value, (int) GeneratedValuesMaximumLength_NumericUpDown.Value);
            List<String> hashedValues = MD5HashStringList(plainValues);
            GeneratedValues_Plain_TextBox.Text = string.Join("\r\n", plainValues);
            GeneratedValues_Hashed_TextBox.Text = string.Join("\r\n", hashedValues);
            GenerateInputFileWithHashes();
        }

        private List<String> GenerateRandomStringList(int randomValuesNumber, int maximumValueLength)
        {
            List<String> randomValues = new List<String>();
            String characters = GetAvailableCharacters();
            for (int i = 0; i < randomValuesNumber; i++)
            {
                StringBuilder generatigValue = new StringBuilder();

                for (int j = 0; j < random.Next(1, maximumValueLength); j++)
                {
                    int charIndex = random.Next(0, characters.Length);
                    Console.WriteLine(charIndex);
                    generatigValue.Append(characters.Substring(charIndex, 1));
                }
                randomValues.Add(generatigValue.ToString());
            }
            return randomValues;
        }

        private String GetAvailableCharacters() {
            String characters = "";
            characters += Numeric_Radio.Checked ? NUMERIC : "";
            characters += Alpha_Radio.Checked ? ALPHA : "";
            characters += LowerAlpha_Radio.Checked ? LOWERALPHA : "";
            return characters;
            }
        private List<String> MD5HashStringList(List<String> plainValues)
        {
            List<String> hashedValues = new List<String>();
            foreach (String value in plainValues)
            {
                byte[] valueInBytes = ASCIIEncoding.ASCII.GetBytes(value);
                byte[] valueHashed = MD5hash.ComputeHash(valueInBytes);
                hashedValues.Add(BitConverter.ToString(valueHashed).Replace("-", "").ToLowerInvariant());
            }
            return hashedValues;
        }

        private void Numeric_Radio_CheckedChanged(object sender, EventArgs e)
        {
            GenerateValues_Button.Enabled = Numeric_Radio.Checked || Alpha_Radio.Checked || LowerAlpha_Radio.Checked;
        }

        private void Alpha_Radio_CheckedChanged(object sender, EventArgs e)
        {
            GenerateValues_Button.Enabled = Numeric_Radio.Checked || Alpha_Radio.Checked || LowerAlpha_Radio.Checked;
        }

        private void LowerAlpha_Radio_CheckedChanged(object sender, EventArgs e)
        {
            GenerateValues_Button.Enabled = Numeric_Radio.Checked || Alpha_Radio.Checked || LowerAlpha_Radio.Checked;
        }

        private void RainbowTablesDirectory_TextBox_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog rainbowTablesFolder = new FolderBrowserDialog();
            if (rainbowTablesFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                RainbowTablesDirectory_TextBox.Text = rainbowTablesFolder.SelectedPath;
        }

        private void Crack_Button_Click(object sender, EventArgs e)
        {
            StartCracking();
        }

        private void StartCracking()
        {
            ProcessStartInfo crackingProcessInfo = new ProcessStartInfo();
            string rcrackLocation = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Resources\RainbowCrack\rainbowcrack-1.8-win64\rcrack.exe";
            Console.WriteLine(rcrackLocation);
            crackingProcessInfo.UseShellExecute = false;
            crackingProcessInfo.RedirectStandardOutput = true;
            crackingProcessInfo.RedirectStandardError = true;
            crackingProcessInfo.WorkingDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Resources\RainbowCrack\rainbowcrack-1.8-win64";
            crackingProcessInfo.FileName = rcrackLocation;
            crackingProcessInfo.Arguments = RainbowTablesDirectory_TextBox.Text + " -l " + Environment.CurrentDirectory + @"\InputHashes.txt";
            Console.WriteLine(crackingProcessInfo.Arguments);
            Process crackingProcess = new Process();
            crackingProcess.StartInfo = crackingProcessInfo;
            //crackingProcess.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            //crackingProcess.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            crackingProcess.Start();
            crackingProcess.WaitForExit();
            //crackingProcess.BeginOutputReadLine();
            //crackingProcess.BeginErrorReadLine();
            CrackingResults_TextBox.Text += "\r\n" + crackingProcess.StandardOutput.ReadToEnd();
            CrackingResults_TextBox.Text += "\r\n" + crackingProcess.StandardError.ReadToEnd();
        }

        private void GenerateInputFileWithHashes()
        {
            string fileName = Environment.CurrentDirectory + @"\InputHashes.txt";
            Console.WriteLine(fileName);

            try
            {   
                if (File.Exists(fileName))
                {
                //    File.Delete(fileName);
                }

                // Create a new file     
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.Write(GeneratedValues_Hashed_TextBox.Text);
                }

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
    }
}
