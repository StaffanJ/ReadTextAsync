using System;
using Information;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
/// <summary>
/// An example of an async operation vs a regular operation.
/// The async dosen't freeze the UI, but the regular does.
/// </summary>
namespace ReadTextAsync
{
    [Author("S.Jansson")]
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //WriteFile();
        }

        /// <summary>
        /// Simple method to write a file to the selected path.
        /// </summary>
        private static void WriteFile()
        {
            //Words to write to the text file.
            string[] words = { "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "Aliquam sit amet arcu commodo, feugiat justo sit amet, sodales elit.", "Phasellus sollicitudin, mauris sed ullamcorper facilisis, erat tellus tristique augue, id elementum lorem sem non urna.", "Nulla felis ipsum, convallis et sollicitudin ut, suscipit at magna.", "Interdum et malesuada fames ac ante ipsum primis in faucibus.", "Aenean lacinia faucibus pulvinar.",  "Vestibulum nec justo viverra nunc aliquet semper.", "Mauris sodales nisi sit amet nibh porta pulvinar.", "Donec cursus dignissim congue.", "Suspendisse a elit vehicula, consectetur lectus sed, aliquam tortor.", "Duis id lorem lacus.", "Curabitur non consequat eros."};

            //Writes the words to the filepath
            File.AppendAllLines(@"Path/To/Your/Text/File", words);

        }

        /// <summary>
        /// getTextButton async operation, tries to get the string from the GetTextAsync if it fails throwns a general exception.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void gettextButton_Click(object sender, RoutedEventArgs e)
        {
            //Clears the textbox.
            readtextTextBox.Clear();
            //Tries to call the async method and write it to a string and appends the text to a textbox.
            try
            {
                string text = await GetTextAsync();
                readtextTextBox.Text += text;
            }
            //Catches a general exception and writes it to a error textbox.
            catch (Exception ex)
            {
                errorTextBox.Text = ex.Message;
            }
        }

        /// <summary>
        /// This method returns the string to the calling type.
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetTextAsync()
        {
            //Path to the text file
            string filePath = @"C:\Users\Staffan\Source\Repos\ReadTextAsync\text.txt";

            //Using statement for the filestream, takes the path to the file, what mode to use, what access, and how to handle the sharing.
            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true))
            {
                //A StringBuilder to write the text to.
                StringBuilder sb = new StringBuilder();

                //How many bytes the buffer can handle.
                byte[] buffer = new byte[0x1000];
                //Declaring an int
                int numRead;
                //A while statement that advances the position within the stream until it reaches 0
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    //A delay to simulate a real world delay from a server around the globe.
                    await Task.Delay(3000);
                    //A string that uses encoding on the byte array and transform it into a string.
                    string text = Encoding.UTF8.GetString(buffer, 0, numRead);
                    //Append the string to the StringBuilder
                    sb.Append(text);
                }
                //The return type.
                return sb.ToString();
            }
        }

        /// <summary>
        /// Standard button operation, calls the GetText method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetText();
            }
            catch (Exception ex)
            {
                errorTextBox.Text = ex.Message; 
            }
        }

        /// <summary>
        /// Regular example of a example to get a text file, it freezes the UI thread.
        /// </summary>
        private void GetText()
        {
            //Clear the textbox.
            notAsyncTextBox.Clear();
            //Path to the text file.
            string fileInfo = @"C:\Users\Staffan\Source\Repos\ReadTextAsync\text.txt";

            //Using statement for the filestream, takes the path to the file, what mode to use, what access, and how to handle the sharing.
            using (FileStream fs = new FileStream(fileInfo, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096))
            {
                //How many bytes the buffer can handle.
                byte[] buffer = new byte[0x1000];
                //Declaring an int
                int numRead;
                //A not async while loop that reads the filestream until it reaches 0
                while ((numRead = fs.Read(buffer, 0, buffer.Length)) != 0)
                {
                    //Simulates a server somewhere in world.
                    Thread.Sleep(3000);
                    //Writes the buffer to a string.
                    string text = Encoding.UTF8.GetString(buffer, 0, numRead);
                    //Writes the string to a textbox.
                    notAsyncTextBox.Text += text;
                }
            }
        }
    }
}