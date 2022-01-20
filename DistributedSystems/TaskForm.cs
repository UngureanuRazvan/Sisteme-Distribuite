using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DistributedSystems
{//TO DO: Add example for non parallelism to have a comparison
    public partial class TaskForm : Form
    {
        string sourceFileName = "";
        string targetFileName = "result.csv";
        float progress = 0;

        public TaskForm()
        {
            InitializeComponent();
            backgroundWorker1 = new BackgroundWorker();

            backgroundWorker1.DoWork += backgroundWorker_DoWork; ;

            backgroundWorker1.RunWorkerCompleted += backgroundWorker_Completed;

            backgroundWorker1.ProgressChanged += Bgworker_ProgressChanged;

            backgroundWorker1.WorkerReportsProgress = true;

            backgroundWorker1.WorkerSupportsCancellation = true;

        }
        
        private void btnUpload_Click(object sender, EventArgs e)
        {


            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.AddExtension = true;
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sourceFileName = openFileDialog.FileName;
            }

        }
        private void copyFileTPL(string sourceFile, string targetFile)
        {
            richTextBox1.AppendText("File Copy Operation using the Task Parallel Library" + "\n");
            richTextBox1.AppendText("File to copy: " + @sourceFile + "\n");


            StreamReader sr = new StreamReader(@sourceFile);
            float totalLines = File.ReadLines(@sourceFile).Count();
            sr.Close();

            string line = "";
            progress = 0;

            richTextBox1.AppendText("Copying file..." + "\n");

            int count = 0;

            //file copy operations
            using (StreamReader reader = new StreamReader(@sourceFile))
            {
                StreamWriter writer = new StreamWriter(@targetFile);

                while ((line = reader.ReadLine()) != null)
                {
                    ++count;

                    

                    //avoid adding blank line in the end of file
                    if (count != totalLines)
                        writer.WriteLine(line);
                    else
                        writer.Write(line);

                    progress = (count / totalLines) * 100;
                   
                    
                    richTextBox1.AppendText("Progress: " + Math.Round(progress, 2).ToString() + "% (rows copied: " + count.ToString() + ")" + "\n");

                }

                writer.Close();
            }
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            progressBar2.Maximum = 100;
            progressBar2.Value = 0;
            
            Parallel.Invoke(

              () =>copyFileTPL(@sourceFileName, @targetFileName),             
              () =>progressBar2.BeginInvoke( (MethodInvoker)( () =>{ progressBar2.Value = (int)progress; } ) )             
              
            );
           
        }

        private void Calculate(int i)
        {
            double pow = Math.Pow(i, i);
        }

        public void DoWork(IProgress<int> progress)
        {
          
            for (int j = 0; j < 100000; j++)
            {
                Calculate(j);

                // Use progress to notify UI thread that progress has changed
                if (progress != null)
                    progress.Report((j + 1) * 100 / 100000);
            }
        }

        private void btnTask_Click(object sender, EventArgs e)
        {

            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            progressBar1.Value = 0;

            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
                btnTask.Enabled = false;
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            btnTask.Enabled = true;
            
        }


        private void Bgworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                label2.Text = @"The user cancelled the operation";
            else
                label2.Text = @"operation done";

        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            //TODO : Upload video code 


            for (int i = 0; i <= 100; i++)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Thread.Sleep(100);
                backgroundWorker1.ReportProgress(i);
            }
        }

       
    }

}
