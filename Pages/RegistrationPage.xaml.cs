using Uchebka.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Uchebka.Pages
{
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            try
            {
                var db = ConnectionClass.comfortEntities;

                cbMaritalStatus.ItemsSource = db.FamilyStatus.ToList();
                cbMaritalStatus.DisplayMemberPath = "NmaeStatus";
                cbMaritalStatus.SelectedValuePath = "Id_status";

                cbHealthStatus.ItemsSource = db.Health.ToList();
                cbHealthStatus.DisplayMemberPath = "NameHealth";
                cbHealthStatus.SelectedValuePath = "Id_health";

                cbPosition.ItemsSource = db.Role.ToList();
                cbPosition.DisplayMemberPath = "NameRole";
                cbPosition.SelectedValuePath = "Id_role";

                if (cbMaritalStatus.Items.Count > 0)
                    cbMaritalStatus.SelectedIndex = 0;
                if (cbHealthStatus.Items.Count > 0)
                    cbHealthStatus.SelectedIndex = 0;
                if (cbPosition.Items.Count > 0)
                    cbPosition.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки справочников: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {


            if (string.IsNullOrWhiteSpace(txtLogin.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                dpBirthDate.SelectedDate == null ||
                string.IsNullOrWhiteSpace(txtPassportSeries.Text) ||
                string.IsNullOrWhiteSpace(txtPassportNumber.Text) ||
                cbMaritalStatus.SelectedItem == null ||
                cbHealthStatus.SelectedItem == null ||
                cbPosition.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка регистрации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка регистрации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var db = ConnectionClass.comfortEntities;

                var existingLogin = db.Logins.FirstOrDefault(l => l.Login == txtLogin.Text.Trim());
                if (existingLogin != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!",
                        "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtLogin.Focus();
                    return;
                }

                var existingEmployee = db.Employee.FirstOrDefault(emp =>
                    emp.PassportSeria == txtPassportSeries.Text.Trim() &&
                    emp.PassportNumber == txtPassportNumber.Text.Trim());

                if (existingEmployee != null)
                {
                    MessageBox.Show("Сотрудник с такими паспортными данными уже зарегистрирован!",
                        "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                Employee employee = new Employee
                {
                    Surname = txtLastName.Text.Trim(),
                    Name = txtFirstName.Text.Trim(),
                    Patronumic = string.IsNullOrWhiteSpace(txtMiddleName.Text) ? null : txtMiddleName.Text.Trim(),
                    Birthday = dpBirthDate.SelectedDate.Value,
                    PassportSeria = txtPassportSeries.Text.Trim(),
                    PassportNumber = txtPassportNumber.Text.Trim(),
                    Id_family = (int)cbMaritalStatus.SelectedValue,
                    Id_health = (int)cbHealthStatus.SelectedValue,
                    Id_role = (int)cbPosition.SelectedValue,
                    BankDetails = "Не указаны"
                };

                db.Employee.Add(employee);
                db.SaveChanges();

                Logins login = new Logins
                {
                    Login = txtLogin.Text.Trim(),
                    Password = txtPassword.Password.Trim(),
                    Id_user = employee.Id_employee
                };

                db.Logins.Add(login);
                db.SaveChanges();

                CurrentUser.User = login;

                MessageBox.Show($"Регистрация успешно завершена!\n\n" +
                    $"Сотрудник: {employee.Surname} {employee.Name}\n" +
                    $"Логин: {login.Login}\n\n" +
                    "Теперь вы можете войти в систему.",
                    "Успешная регистрация",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                NavigationService.Navigate(new LoginPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}",
                    "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }

}