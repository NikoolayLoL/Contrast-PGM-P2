using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLEASE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "255";
            textBox4.Text = "255";
            textBox5.Text = "255";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;

                try
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        // Read the first three lines (assuming the format is consistent)
                        string format = reader.ReadLine();
                        string comment = reader.ReadLine();
                        string sizeLine = reader.ReadLine();

                        // Parse the width, height, and max color value
                        string[] sizeTokens = sizeLine.Split(' ');
                        int width = int.Parse(sizeTokens[0]);
                        int height = int.Parse(sizeTokens[1]);
                        int maxColorValue = int.Parse(reader.ReadLine());
                        textBox5.Text = maxColorValue.ToString();


                        // Create a Bitmap with the specified width and height
                        Bitmap bmp = new Bitmap(width, height);

                        // Read and process the pixel values
                        for (int y = 0; y < height; y++)
                        {
                            string line = reader.ReadLine();
                            string[] pixelTokens = line.Split(' ');

                            // Process each pixel value
                            for (int x = 0; x < width; x++)
                            {
                                int pixelValue = int.Parse(pixelTokens[x]);

                                // Set the pixel color in the Bitmap
                                Color pixelColor = Color.FromArgb(pixelValue, pixelValue, pixelValue);
                                bmp.SetPixel(x, y, pixelColor);
                            }
                        }

                        pictureBox1.Image = bmp;
                        pictureBox1.Refresh();
                        pictureBox2.Image = bmp;
                        pictureBox2.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int r1 = int.Parse(textBox1.Text);
            int s1 = int.Parse(textBox2.Text);
            int r2 = int.Parse(textBox3.Text);
            int s2 = int.Parse(textBox4.Text);

            try
            {
                Bitmap bmp = (Bitmap)pictureBox1.Image;

                for (int i = 0; i < bmp.Height; i++)
                {
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        Color pixel = bmp.GetPixel(j, i);
                        int formula = s1 + (pixel.R - r1) * ((int)(s2 - s1) / (r2 - r1));

                        if (formula < 0)
                        {
                            formula = 0;
                        }
                        else if (formula > 255)
                        {
                            formula = 255;
                        }

                        formula = (formula * int.Parse(textBox5.Text)) / 255;

                        Color pixelColor = Color.FromArgb(formula, formula, formula);
                        bmp.SetPixel(j, i, pixelColor);
                    }
                }


                pictureBox2.Image = bmp;
                pictureBox2.Refresh();
            }
            catch (Exception me)
            {
                return;
            }
        }

        private void SaveAsPgm(string filePath, Bitmap bmp)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("P2");
                writer.WriteLine("# Saved by your application");
                writer.WriteLine($"{bmp.Width} {bmp.Height}");
                writer.WriteLine("255");

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        Color pixelColor = bmp.GetPixel(x, y);
                        int grayValue = (int)(pixelColor.R);
                        writer.Write($"{grayValue} ");
                    }
                    writer.WriteLine();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Ensure there is an image in pictureBox2
            if (pictureBox2.Image != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PGM files (*.pgm)|*.pgm|All files (*.*)|*.*";
                    saveFileDialog.Title = "Save Image As PGM";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Save the image as PGM
                        SaveAsPgm(saveFileDialog.FileName, (Bitmap)pictureBox2.Image);
                    }
                }
            }
            else
            {
                MessageBox.Show("No image to save.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
