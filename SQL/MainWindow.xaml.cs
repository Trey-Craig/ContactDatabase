using SQL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.TeamFoundation.Common.Internal;

namespace SQL {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    
    public partial class MainWindow : Window {
        List<properties> people_results = new List<properties>();
        public MainWindow() {
            InitializeComponent();
            update.IsEnabled    = false;
            add_new.IsEnabled   = false;
            delete.IsEnabled    = false;
        }

        private void RefreshListBoxBinding() {
            //SET BINDING INSTANCE FOR LISTBOX
            lstbox.ItemsSource = people_results;

            //SET BINDING FIELD FOR LISTBOX
            lstbox.DisplayMemberPath = "ResultData";
        }

        private void Display_Info(object sender, SelectionChangedEventArgs e) {
            if (lstbox.HasItems) {
                delete.IsEnabled = true;
            }
            
            DataAccess database_data = new DataAccess();
            if(lstbox.SelectedIndex > -1) {
                Bitmap image;
                properties person = people_results.ElementAt(lstbox.SelectedIndex);
                //display properties in appropriate textboxes
                firstname.Text  = person.first_name.ToString();
                lastname.Text   = person.last_name.ToString();
                cell.Text       = person.cell.ToString();
                work.Text       = person.work.ToString();
                mail.Text       = person.mail.ToString();
                notebox.Text    = person.notes.ToString();

                //if the person has no picture no picture will be displayed
                if(person.image != null) {
                    image = ByteArrayToImage(person.image);
                    imagebox.Source = ToBitmapSource(image);
                }
                else {
                    imagebox.Source = null;
                }
                FillEmailbox();
            }
            RefreshListBoxBinding();
        }
        private void FillEmailbox() {
            DataAccess database_data = new DataAccess();
            properties person = people_results.ElementAt(lstbox.SelectedIndex);
            List<string> email;
            email = database_data.GetEmails(person.id.ToString());
            foreach (string item in email) {
                emailbox.Items.Add(item);
            }
        }

        private void Add_New_Entry(object sender, RoutedEventArgs e) {
            DataAccess database_data = new DataAccess();
            properties person;
            //places new info into array
            string[] new_data = new string[7];
            new_data[0] = firstname.Text;
            new_data[1] = lastname.Text;
            new_data[2] = cell.Text;
            new_data[3] = work.Text;
            new_data[4] = email.Text;
            new_data[5] = mail.Text;
            new_data[6] = notebox.Text;
            
            database_data.AddData(new_data);
            string name = $"{new_data[0]} {new_data[1]}";
            List<properties> new_entry = database_data.GetPeople(new_data[0]);
            person = new_entry.First();
            database_data.UpdateEmail(new_data[4], person.id.ToString());
            

            RefreshListBoxBinding();
        }

        private void Delete_Entry(object sender, RoutedEventArgs e) {
            DataAccess database_data = new DataAccess();
            string id;
            
            //grab properties of selected entry
            if(lstbox.SelectedIndex > -1) {
                    properties person = people_results.ElementAt(lstbox.SelectedIndex);
                    id = person.id.ToString();
                    people_results.RemoveAt(lstbox.SelectedIndex);
                    database_data.Deactivate(id);
            }

            //strore person id from record
            firstname.Clear();
            lastname.Clear();
            cell.Clear();
            work.Clear();
            email.Clear();
            mail.Clear();
            notebox.Clear();

            RefreshListBoxBinding();
        }

        private void Update_Entry(object sender, RoutedEventArgs e) {
            DataAccess database_data = new DataAccess();
            if(lstbox.SelectedIndex > -1) {
                properties person = people_results.ElementAt(lstbox.SelectedIndex);

                //store potential new data
                string[] new_data = new string[7];
                new_data[0] = firstname.Text;
                new_data[1] = lastname.Text;
                new_data[2] = cell.Text;
                new_data[3] = work.Text;
                new_data[4] = email.Text;
                new_data[5] = mail.Text;
                new_data[6] = notebox.Text;
                database_data.UpdateData(new_data,person.id.ToString());
                database_data.UpdateEmail(new_data[4], person.id.ToString());
                if(person.image != null) {
                    database_data.AddPicture(person.image, person.id);
                }
            }
            RefreshListBoxBinding();
        }

        private void Manage_Click(object sender, RoutedEventArgs e) {
            //opens sub window
            SubWindow Manage = new SubWindow("");
            Manage.ShowDialog();
        }

        public Bitmap ByteArrayToImage(byte[] data){
           Bitmap photo;    
            
            MemoryStream bipimag = new MemoryStream(data);
            photo = new Bitmap(bipimag);
            
            return photo;
        }

        public static BitmapSource ToBitmapSource( Bitmap source){
            BitmapSource bitSrc = null;
            var hBitmap = source.GetHbitmap();
            try
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (System.ComponentModel.Win32Exception)
            {
                bitSrc = null;
            }
            finally
            {
                NativeMethods.DeleteObject(hBitmap);
            }

            return bitSrc;
        }

        private void UploadPhoto(object sender, RoutedEventArgs e) {
            if(lstbox.SelectedIndex > -1) {
                //selects person for photo to be associated to
                properties person = people_results.ElementAt(lstbox.SelectedIndex);
                Bitmap image;
                string filepath;
                filepath = LaunchSingleFileDialog();
                //if statement for if the person cancels when choosing photo
                if(filepath != "") {
                    person.image = File.ReadAllBytes(filepath);
                }
                if(person.image != null) {
                    image = ByteArrayToImage(person.image);
                    imagebox.Source = ToBitmapSource(image);
                }
                else {
                    imagebox.Source = null;
                }
            }
            RefreshListBoxBinding();
        }

        private string LaunchSingleFileDialog() {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog ofd_temp = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension       
            ofd_temp.DefaultExt = ".ppm";
            ofd_temp.Filter = "All Files (*.*)|*.*";
            ofd_temp.Title = "File Select";
            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = ofd_temp.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true) {
                return ofd_temp.FileName;
            }//end if
            return "";
        }//end method

        private void Search_Contact(object sender, RoutedEventArgs e) {
            //CREATA DATA ACCESS INSTANCE
            DataAccess database_data = new DataAccess();

            //USE DATA ACCESS INSTANCE TO GET PEOPLE DATA FROM THE DB
            people_results = database_data.GetPeople(txtbox.Text);

            //REFRESH LIST BOX
            RefreshListBoxBinding();
            update.IsEnabled    = true;
            add_new.IsEnabled   = true;
        }
    }
}
