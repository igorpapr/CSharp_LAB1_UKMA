using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using BirthdayPicker.Annotations;

namespace BirthdayPicker
{
    internal class BirthdayPickerViewModel:INotifyPropertyChanged
    {
        #region Fields
        private readonly User _user;

        private RelayCommand<object> _submitCommand;
        #endregion

        #region Properties
        public DateTime Date
        {
            get { return _user.Date; }
            set
            {
                _user.Date = value;
                OnPropertyChanged();
            }
        }

        public string Age
        {
            get { return _user.Age; }
            set
            {
                _user.Age = value; 
                OnPropertyChanged();
            }
        }

        public string WesternZodiac
        {
            get { return _user.WesternZodiac; }
            set
            {
                _user.WesternZodiac = value;
                OnPropertyChanged();
            }
        }

        public string ChineseZodiac
        {
            get { return _user.ChineseZodiac;}
            set
            {
                _user.ChineseZodiac = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> SubmitCommand
        {
            get
            {
                return _submitCommand ?? (_submitCommand = new RelayCommand<object>(
                           ProcessSubmitting));
            }
        }
        #endregion

        internal BirthdayPickerViewModel()
        {
            _user = new User();
            Date = DateTime.Today;
        }


        private async void ProcessSubmitting(object obj)
        {
            Age = "";
            WesternZodiac = "";
            ChineseZodiac = "";
            
            await Task.Run(() =>
            {
                try
                {
                    ComputeAge();
                    ComputeWesternZodiac();
                    ComputeChineseZodiac();
                    CheckForBirthday();
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }

        private void ComputeAge()
        {
            DateTime atTheMoment = DateTime.Today;
            int resultAge = atTheMoment.Year - Date.Year;

            if (((atTheMoment.Month == Date.Month) && (Date.Day > atTheMoment.Day)) || Date.Month > atTheMoment.Month)
            {
                resultAge--;
            }

            if(resultAge >= 0 && resultAge < 135)
            {
                Age = "Your age: " + resultAge;
            }
            else
            {
                throw new ArgumentException("Error! You picked irrelevant date");
            }
        }

        private void CheckForBirthday()
        {
            if (Date.Day == DateTime.Today.Day && Date.Month == DateTime.Today.Month)
            {
                MessageBox.Show("Happy Birthday, Dear! Wish you luck <3");
            }
        }

        private void ComputeWesternZodiac()
        {
            string res = "";
            if ((Date.Month == 1 && Date.Day >= 21) || (Date.Month == 2 && Date.Day <= 20))
                res += "Aquarius";
            if ((Date.Month == 2 && Date.Day >= 21) || (Date.Month == 3 && Date.Day <= 20))
                res += "Pisces";
            if ((Date.Month == 3 && Date.Day >= 21) || (Date.Month == 4 && Date.Day <= 20))
                res += "Aries";
            if ((Date.Month == 4 && Date.Day >= 21) || (Date.Month == 5 && Date.Day <= 20))
                res += "Taurus";
            if ((Date.Month == 5 && Date.Day >= 21) || (Date.Month == 6 && Date.Day <= 21))
                res += "Gemini";
            if ((Date.Month == 6 && Date.Day >= 22) || (Date.Month == 7 && Date.Day <= 22))
                res += "Cancer";
            if ((Date.Month == 7 && Date.Day >= 23) || (Date.Month == 8 && Date.Day <= 23))
                res += "Leo";
            if ((Date.Month == 8 && Date.Day >= 24) || (Date.Month == 9 && Date.Day <= 23))
                res += "Virgo";
            if ((Date.Month == 9 && Date.Day >= 24) || (Date.Month == 10 && Date.Day <= 22))
                res += "Libra";
            if ((Date.Month == 10 && Date.Day >= 23) || (Date.Month == 11 && Date.Day <= 22))
                res += "Scorpio";
            if ((Date.Month == 11 && Date.Day >= 23) || (Date.Month == 12 && Date.Day <= 21))
                res += "Sagittarius";
            if ((Date.Month == 12 && Date.Day >= 22) || (Date.Month == 1 && Date.Day <= 20))
                res += "Capricorn";
            WesternZodiac = "Your Western Zodiac Sign is " + res;
        }

        private void ComputeChineseZodiac()
        {
            int yearForAnimal = Date.Year % 12;
            CorrectChineseYear(ref yearForAnimal);
            int yearForElemental = Date.Year % 10;
            CorrectChineseYear(ref yearForElemental);

            ChineseZodiac = $"Your Chinese Zodiac Sign is {ChooseChineseElemental(yearForElemental)} {ChooseChineseAnimal(yearForAnimal)}";
        }

        /*Due to the fact that chinese new year starts
          according to the moon phases, and it can be [Jan21;Feb21],
          for more simplicity we'll suppose that in the year when user was born,
          China was celebrating New Year at 21 of Jan*/
        private void CorrectChineseYear(ref int year)
        {
            if ((Date.Month == 1) && Date.Day < 21)
            {
                if (year == 0)
                {
                    year = 11;
                }
                else
                {
                    year--;
                }
            }
        }

        private string ChooseChineseElemental(int year)
        {
            switch (year)
            {
                case 0:
                case 1:
                    return "Metal";
                case 2:
                case 3:
                    return "Water";
                case 4:
                case 5:
                    return "Wood";
                case 6:
                case 7:
                    return "Fire";
                case 8:
                    return "Earth";
                default:
                    return "Earth";
            }
        }

        private string ChooseChineseAnimal(int year)
        {
            switch (year)
            {
                case 1:
                    return "Rooster";
                case 2:
                    return "Dog";
                case 3:
                    return "Pig";
                case 4:
                    return "Rat";
                case 5:
                    return "Ox";
                case 6:
                    return "Tiger";
                case 7:
                    return "Rabbit";
                case 8:
                    return "Dragon";
                case 9:
                    return "Snake";
                case 10:
                    return "Horse";
                case 11:
                    return "Goat";
                default:
                    return "Monkey";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
