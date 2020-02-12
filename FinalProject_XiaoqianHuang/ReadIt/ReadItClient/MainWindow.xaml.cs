using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ReadItClient
{

    public partial class MainWindow : Window
    {
        string downloadpath = "";
        //string dirroot = "";
        List<string> files = new List<string>();
        string selectedfile = "";
        public HttpClient client { get; set; }

        private string baseUrl_;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            baseUrl_ = args[1];
            client = new HttpClient();
            string path = Directory.GetCurrentDirectory();
            downloadpath = path + "\\DownloadBook";
            //var filepath = Directory.GetParent(path);
            //path = filepath.ToString() + "\\ReadIt\\wwwroot\\FileStorage";
            //dirroot = path;
            //currentpath = path;
            CurrentPath.Text = baseUrl_;
            //DirectoryInfo di = new DirectoryInfo(path);
            //foreach (var f in di.GetFiles())
            //{
            //    files_.Add(f.Name.ToString());
            //    FileList.Items.Add(f.Name.ToString());
            //} 
            GetFileList();
        }


        private void Files_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                selectedfile = "";
            try
            {
                string selStr = e.AddedItems[0].ToString();
                selectedfile = selStr;
                Console.Write("\n {0}",selectedfile);
            }
            catch
            {
                Console.Write("\n Please select an item!");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Delete();
            GetFileList();
        }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
      GetFileList();
    }

    private void Download_Click(object sender, RoutedEventArgs e)
        {
            Download();
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            //dlg.DefaultExt = ".pdf";
            //dlg.Filter = "Text documents (.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
               UploadText.Text = System.IO.Path.GetFileName(filename);
               Upload(filename);
            }

        }

        public async Task<IEnumerable<string>> GetFileList()
        {
            FileList.Items.Clear();
            files.Clear();
            HttpResponseMessage resp = await client.GetAsync(baseUrl_);
            if (resp.IsSuccessStatusCode)
            {
                var json = await resp.Content.ReadAsStringAsync();
                JArray jArr = (JArray)JsonConvert.DeserializeObject(json);
                foreach (var item in jArr)
                {
                    files.Add(item.ToString());
                    FileList.Items.Add(item.ToString());
                }
            }
            return files;
        }

        public async Task Download()
        {
            var index = 0;
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i] == selectedfile)
                {
                    index = i;
                }
            }
            if (index != 0)
            {
                UploadText.Text = selectedfile + " has been downloaded to [DownloadBook] folder!";
                string path = downloadpath + "\\" + selectedfile;
                Console.Write("\n[{0} has been downloaded to {1}!]", files[index], path);
                //download file
                string display = baseUrl_ + "/" + index;
                var resp2 = await client.GetAsync(baseUrl_ + "/" + index).ConfigureAwait(false);
                System.Net.Http.HttpContent content = resp2.Content; // actually a System.Net.Http.StreamContent instance but you do not need to cast as the actual type does not matter in this case
                using (var file = System.IO.File.Create(path))
                { // create a new file to write to
                    var contentStream = await content.ReadAsStreamAsync(); // get the actual content stream
                    await contentStream.CopyToAsync(file); // copy that stream to the file stream
                    await file.FlushAsync(); // flush back to disk before disposing
                }

            }
        }
       
        public async Task Delete()
        {
            var index = 0;
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i] == selectedfile)
                {
                    index = i;
                }
            }
            UploadText.Text = selectedfile + " has been deleted!";
            Console.Write("\n[{0} has been deleted!]", files[index]);
            await client.DeleteAsync(baseUrl_ + "/" + index).ConfigureAwait(false);
        }

        public async Task Upload(string filepath)
        {
            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            byte[] data = File.ReadAllBytes(filepath);
            ByteArrayContent bytes = new ByteArrayContent(data);
            string fileName = System.IO.Path.GetFileName(filepath);
            multiContent.Add(bytes, "files", fileName);

            await client.PostAsync(baseUrl_, multiContent);
            UploadText.Text = fileName + " has been uploaded!";
            Console.Write("\n[{0} has been uploaded!]", fileName);
            await GetFileList();
        }

    void OpenFile(object sender, RoutedEventArgs e)
    {
      if (selectedfile != null)
      {
        string path = Directory.GetCurrentDirectory();
        var parentdir = Directory.GetParent(path);
        path = parentdir.ToString() + "\\ReadIt\\wwwroot\\FileStorage\\" + selectedfile;
        System.Diagnostics.Process.Start(path);
      }
    }
   }
}
