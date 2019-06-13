using System;
using System.Collections.Generic;
using System.Linq;
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
using TaskManager.Models;
using TaskManager.DataAccess;
using System.Threading;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace TaskManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            jobTypeComboBox.SelectedIndex = 0;
            repeatComboBox.SelectedIndex = 0;
            calendar.SelectedDate = DateTime.Now;

            var daemon = new Thread(new ThreadStart(TimerStart));
            daemon.IsBackground = false;
            daemon.Start();
        }

        private void TaskButtonClick(object sender, RoutedEventArgs e)
        {
            var job = new Job
            {
                TaskDate = (DateTime)calendar.SelectedDate,
                JobType = jobTypeComboBox.SelectedIndex,
                RepeatMode = repeatComboBox.SelectedIndex
            };

            using (var context = new DataAccess.AppContext())
            {
                context.Jobs.Add(job);
                context.SaveChanges();
            }

            MessageBox.Show("Задача запланирована");
        }

        private void TimerStart()
        {
            TimerCallback timerCallback = new TimerCallback(CheckTasks);
            Timer timer = new Timer(timerCallback, null, 0, 60000);
        }

        private void CheckTasks(object obj)
        {
            using (var context = new DataAccess.AppContext())
            {
                var jobs = context.Jobs.ToList();

                foreach(var job in jobs)
                {
                    if(job.TaskDate <= DateTime.Now)
                    {
                        switch (job.RepeatMode)
                        {
                            case 0:
                                job.TaskDate = job.TaskDate.AddDays(7);
                                break;
                            case 1:
                                job.TaskDate = job.TaskDate.AddMonths(1);
                                break;
                            case 2:
                                job.TaskDate = job.TaskDate.AddYears(1);
                                break;
                        }

                        switch (job.JobType)
                        {
                            case 0:
                                SendEmail();
                                MessageBox.Show("Задача \"Отправка Email\" выполнена");
                                break;
                            case 1:
                                DownloadFile();
                                MessageBox.Show("Задача \"Скачка файла\" выполнена");
                                break;
                            case 2:
                                MoveFile();
                                MessageBox.Show("Задача \"Перемещение каталога\" выполнена");
                                break;
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        private void DownloadFile()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(@"http://www.cyberforum.ru/images/cyberforum_logo.jpg", @"c:\cyberforum_logo.jpg");
        }

        private void SendEmail()
        {
            MailAddress from = new MailAddress("5343454_10@mail.ru", "Shipa");
            MailAddress to = new MailAddress("5343454_10@mail.ru");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Тест";
            m.Body = "<h2>Йоу! Всё работает</h2>";
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
            smtp.Credentials = new NetworkCredential("5343454_10@mail.ru", "5343454karp99");
            smtp.EnableSsl = true;
            smtp.Send(m);
            Console.Read();
        }

        private void MoveFile()
        {
            string path = @"c:\Users\Shipa\Desktop\1\test.txt";
            string path2 = @"c:\Users\Shipa\Desktop\2\test.txt";

            File.Move(path, path2);
        }
    }
}
