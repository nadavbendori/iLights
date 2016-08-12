using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using Microsoft.WindowsAzure.Storage.Auth;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iLights
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class signUpPage : Page
    {
        public signUpPage()
        {
            this.InitializeComponent();
        }

        async private void submitSignIn(object sender, RoutedEventArgs e)
        {
                if (passwordBox.Text == "" || userNameBox.Text == "")
            {
                errorBox.Text = "Error! one of the fields is empty. try again!";
            }
                else
            {
                errorBox.Text = "";
                var credentials = new StorageCredentials("ilights", "XBljb0/gcAkqwhGUziEhSS2Wm1eebhsQhJBsGDlo0esqdoaVmRIFB7QWr6Eq5fF8ErnxInKQjH9jpbF6S1Y8kA==");
                var account = new CloudStorageAccount(credentials, true);

                CloudTableClient tableClient = account.CreateCloudTableClient();

                // Retrieve a reference to the table.
                CloudTable table = tableClient.GetTableReference("Coaches");

                // Create the table if it doesn't exist.
                await table.CreateIfNotExistsAsync();

                TableOperation retrieveOperation =
                    TableOperation.Retrieve<user>(userNameBox.Text, passwordBox.Text);

                // Execute the retrieve operation.
                TableResult result = await table.ExecuteAsync(retrieveOperation);
                if (result.Result != null)
                {
                    errorBox.Text = "Error! user name taken";
                    return;
                }


                user newUser = new user(userNameBox.Text, passwordBox.Text);
                TableOperation insertOperation = TableOperation.Insert(newUser);
                await table.ExecuteAsync(insertOperation);

                Frame.Navigate(typeof(chooseProgram), newUser);

            }
               
        }

        private void OnGoBack(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
