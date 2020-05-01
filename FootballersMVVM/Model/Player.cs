using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FootballersMVVM.Model
{
    public class Player
    {
        private string name;
        private string surname;
        private double weight;
        private int yearOfBirth;

        public string Name { get { return name; } set { name = value; } }
        public string Surname { get { return surname; } set { surname = value; } }
        public double Weight { get { return weight; } set { weight = value; } }
        public int Age { get { 
                return CurrentYear()-yearOfBirth; 
            } set { yearOfBirth = CurrentYear() - value; } } //value = age

        public Player()
        {

        }
        public Player(string name, string surname, double weight, int age)
        {
            this.name = name;
            this.surname = surname;
            this.weight = weight;
            this.yearOfBirth = CurrentYear() - age;
        }

        public Player(Player p)
        {
            this.name = p.name;
            this.surname = p.surname;
            this.weight = p.weight;
            this.yearOfBirth = p.yearOfBirth;
        }
        public override string ToString()
        {
            string sWeight = weight.ToString();
            string sAge = (CurrentYear() - this.yearOfBirth).ToString();
            return String.Format("{0} {1}, masa: {2}, wiek: {3}", name, surname, sWeight, sAge);
        }
        public int CurrentYear()
        {
            int.TryParse(DateTime.Now.Year.ToString(), out int year); //get the current year as int
            return year;
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                PropertyChanged(this, new PropertyChangedEventArgs("HasError"));
            }
        }
    }
}
