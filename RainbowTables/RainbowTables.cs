using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RainbowTables
{
    public partial class RainbowTables : Form
    {
        UnicodeEncoding UE;
        MD5CryptoServiceProvider MD5hash;
        Random random;
        const String NUMERIC = "0123456789";
        const String ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const String LOWERALPHA = "abcdefghijklmnopqrstuvwxyz";


        public RainbowTables()
        {
            UE = new UnicodeEncoding();
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
                byte[] valueInBytes = UE.GetBytes(value);
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
    }
}
