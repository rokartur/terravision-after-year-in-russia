﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using TerraVision.Models;
using Newtonsoft.Json;

namespace TerraVision
{
    public partial class Register : UtilsForm
    {
        public Register()
        {
            InitializeComponent();
        }
        private void DeleteAllUsersButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(DataPath))
            {
                File.Delete(DataPath);
                MessageBox.Show("Wszyscy użytkownicy zostali usunięci.", "Usuwanie użytkowników", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Nie ma żadnych użytkowników do usunięcia.", "Usuwanie użytkowników", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            string hashedPassword = HashPassword(password);
            var users = LoadUsers();

            if (string.IsNullOrWhiteSpace(usernameTextBox.Text) || string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                MessageBox.Show("Pola nazwa użytkownika i hasło nie mogą być puste.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (UserExists(users, username))
            {
                MessageBox.Show("User already exists. Try another username.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var user = new User
            {
                Id = users.Count + 1,
                Username = username,
                Password = hashedPassword,
                SearchHistory = new List<string>(),
                HomeLocation = new List<string>(2),
                Country = "Poland"
            };

            users.Add(user);

            var serializedData = JsonConvert.SerializeObject(users);

            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));
            }

            File.WriteAllText(DataPath, serializedData);

            MessageBox.Show("User registered successfully.", "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }

        private void SwitchToLoginButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            var loginForm = new Login();
            loginForm.Show();
        }
    }
}
