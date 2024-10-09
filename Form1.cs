using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Lab2_OOAP_
{
    // Абстрактний клас, що представляє працівника
    public abstract class Employee
    {
        public string Name { get; set; } // Ім'я працівника
        public string Position { get; set; } // Посада працівника

        // Делегат для розрахунку зарплати
        public delegate decimal CalculateSalaryDelegate();
        public CalculateSalaryDelegate CalculateSalary; // Делегат для конкретного розрахунку зарплати

        // Конструктор для ініціалізації імені та посади
        public Employee(string name, string position)
        {
            Name = name;
            Position = position;
        }
    }

    // Клас для начальника відділу
    public class DepartmentHead : Employee
    {
        public DepartmentHead(string name) : base(name, "Начальник відділу")
        {
            // Визначення зарплати для начальника відділу
            CalculateSalary = () => 15000;
        }
    }

    // Клас для головного інженера
    public class ChiefEngineer : Employee
    {
        public ChiefEngineer(string name) : base(name, "Головний інженер")
        {
            // Визначення зарплати для головного інженера
            CalculateSalary = () => 12000;
        }
    }

    // Клас для інженера-програміста
    public class SoftwareEngineer : Employee
    {
        public SoftwareEngineer(string name) : base(name, "Інженер-програміст")
        {
            // Визначення зарплати для інженера-програміста
            CalculateSalary = () => 10000;
        }
    }

    // Клас для системного адміністратора
    public class SystemAdministrator : Employee
    {
        public SystemAdministrator(string name) : base(name, "Системний адміністратор")
        {
            // Визначення зарплати для системного адміністратора
            CalculateSalary = () => 9000;
        }
    }

    // Головна форма програми
    public partial class Form1 : Form
    {
        private ComboBox cmbEmployees; // ComboBox для вибору працівника
        private Button btnAddEmployee; // Кнопка для додавання нового працівника
        private Button btnCalculateSalary; // Кнопка для розрахунку зарплати
        private List<Employee> employees; // Список працівників

        public Form1()
        {
            InitializeComponent(); // Ініціалізація компонентів форми
            InitializeCustomComponents(); // Ініціалізація кастомних компонентів
            InitializeEmployees(); // Ініціалізація списку працівників
        }

        private void InitializeCustomComponents()
        {
            // Налаштування ComboBox для вибору працівника
            cmbEmployees = new ComboBox { Location = new Point(10, 10), Width = 200 };
            Controls.Add(cmbEmployees); // Додавання ComboBox до форми

            // Налаштування кнопки для додавання нового працівника
            btnAddEmployee = new Button { Text = "Додати працівника", Location = new Point(10, 50), Width = 200 };
            btnAddEmployee.Click += BtnAddEmployee_Click; // Прив'язка обробника події кліку
            Controls.Add(btnAddEmployee); // Додавання кнопки до форми

            // Налаштування кнопки для розрахунку зарплати
            btnCalculateSalary = new Button { Text = "Розрахувати зарплату", Location = new Point(10, 90), Width = 200 };
            btnCalculateSalary.Click += BtnCalculateSalary_Click; // Прив'язка обробника події кліку
            Controls.Add(btnCalculateSalary); // Додавання кнопки до форми

            // Налаштування заголовка форми
            this.Text = "Керування працівниками";
        }

        private void InitializeEmployees()
        {
            // Ініціалізація списку працівників
            employees = new List<Employee>
            {
                new DepartmentHead("Іван"),
                new ChiefEngineer("Олег"),
                new SoftwareEngineer("Анна"),
                new SystemAdministrator("Сергій"),
                new SoftwareEngineer("Марія")
            };

            // Додаємо працівників у ComboBox
            UpdateEmployeeComboBox();
        }

        private void UpdateEmployeeComboBox()
        {
            cmbEmployees.Items.Clear(); // Очищення ComboBox перед додаванням нових працівників
            foreach (var employee in employees)
            {
                // Додаємо інформацію про працівника у ComboBox
                cmbEmployees.Items.Add($"{employee.Name} ({employee.Position})");
            }
        }

        private void BtnAddEmployee_Click(object sender, EventArgs e)
        {
            // Відкриття нової форми для додавання працівника
            FormAddEmployee formAddEmployee = new FormAddEmployee(employees);
            formAddEmployee.EmployeeAdded += UpdateEmployeeComboBox; // Оновлюємо ComboBox після додавання
            formAddEmployee.ShowDialog(); // Відображення діалогової форми
        }

        private void BtnCalculateSalary_Click(object sender, EventArgs e)
        {
            // Отримуємо вибраного працівника з ComboBox
            string selectedEmployee = cmbEmployees.SelectedItem.ToString();
            if (string.IsNullOrEmpty(selectedEmployee))
            {
                MessageBox.Show("Будь ласка, виберіть працівника."); // Перевірка наявності вибраного працівника
                return; // Вихід з методу, якщо нічого не вибрано
            }

            // Витягуємо ім'я працівника з рядка
            string employeeName = selectedEmployee.Split('(')[0].Trim();
            // Знаходимо працівника у списку за ім'ям
            Employee employee = employees.Find(emp => emp.Name == employeeName);

            if (employee != null)
            {
                // Виводимо повідомлення з розрахованою зарплатою
                MessageBox.Show($"Заробітна плата {employee.Name} ({employee.Position}): {employee.CalculateSalary()} грн");
            }
        }
    }

    // Форма для додавання нового працівника
    public partial class FormAddEmployee : Form
    {
        private TextBox txtName; // TextBox для введення імені
        private ComboBox cmbPosition; // ComboBox для вибору посади
        private Button btnSubmit; // Кнопка для підтвердження
        private List<Employee> employees; // Список працівників

        public event Action EmployeeAdded; // Подія для оновлення ComboBox

        public FormAddEmployee(List<Employee> employees)
        {
            this.employees = employees; // Збереження списку працівників
            InitializeCustomComponents(); // Ініціалізація компонентів форми
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Додати працівника"; // Налаштування заголовка форми
            this.Size = new Size(300, 200); // Налаштування розміру форми

            // Налаштування TextBox для введення імені
            txtName = new TextBox { Location = new Point(10, 10), Width = 200 };
            Controls.Add(txtName); // Додавання TextBox до форми

            // Налаштування ComboBox для вибору посади
            cmbPosition = new ComboBox { Location = new Point(10, 40), Width = 200 };
            cmbPosition.Items.AddRange(new string[] // Додавання можливих посад
            {
                "Начальник відділу",
                "Головний інженер",
                "Інженер-програміст",
                "Системний адміністратор"
            });
            Controls.Add(cmbPosition); // Додавання ComboBox до форми

            // Налаштування кнопки для підтвердження
            btnSubmit = new Button { Text = "Додати", Location = new Point(10, 70), Width = 200 };
            btnSubmit.Click += BtnSubmit_Click; // Прив'язка обробника події кліку
            Controls.Add(btnSubmit); // Додавання кнопки до форми
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            // Отримання введених даних
            string name = txtName.Text;
            string position = cmbPosition.SelectedItem.ToString(); // Отримання вибраної посади

            // Перевірка на введення імені та вибір посади
            if (string.IsNullOrWhiteSpace(name) || position == null)
            {
                MessageBox.Show("Будь ласка, введіть ім'я та виберіть посаду."); // Попередження при неправильному введенні
                return; // Вихід з методу
            }

            Employee newEmployee = null; // Змінна для нового працівника

            // Визначення нового працівника за вибраною посадою
            switch (position)
            {
                case "Начальник відділу":
                    newEmployee = new DepartmentHead(name);
                    break;
                case "Головний інженер":
                    newEmployee = new ChiefEngineer(name);
                    break;
                case "Інженер-програміст":
                    newEmployee = new SoftwareEngineer(name);
                    break;
                case "Системний адміністратор":
                    newEmployee = new SystemAdministrator(name);
                    break;
            }

            employees.Add(newEmployee); // Додавання нового працівника у список
            EmployeeAdded.Invoke(); // Виклик події оновлення ComboBox

            this.Close(); // Закриття форми
        }
    }
}