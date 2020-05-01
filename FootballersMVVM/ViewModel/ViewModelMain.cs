using System;
using System.Collections.Generic;
using System.Text;

namespace FootballersMVVM.ViewModel
{
    using Model;
    using BaseClass;
    using System.Windows.Input;
    using System.Windows;
    using System.ComponentModel;
    using System.Collections.ObjectModel; 
    using GalaSoft.MvvmLight.CommandWpf;
    using System.IO;
    using Newtonsoft.Json;

    internal class ViewModelMain : ViewModelBase, INotifyPropertyChanged
    {
        public ViewModelMain()
        {
            #region Players
            Players = LoadPlayers(@"players.json");
            #endregion

            #region ComboBox
            foreach (var i in AgeIntList.Age)
                AgeList.Add(i);
            #endregion

            #region CreateCommands
            CreateAddCommand();
            CreateDeleteCommand();
            CreateEditCommand();
            CreateCopyCommand();
            CreateCloseCommand();
            #endregion
        }

        #region Players
        private ObservableCollection<Player> players = new ObservableCollection<Player>();
        public ObservableCollection<Player> Players
        {
            get { return players; }
            set { players = value; onPropertyChanged("Players"); }
        }
        private Player selectedPlayer = null;
        public Player SelectedPlayer
        {
            get { return selectedPlayer; }
            set { selectedPlayer = value; onPropertyChanged("SelectedPlayer"); }
        }

        #endregion

        #region ComboBox
        AgeIntList ageIntList = new AgeIntList();

        private ObservableCollection<int> ageList = new ObservableCollection<int>();
        public ObservableCollection<int> AgeList
        {
            get { return ageList; }
            set { ageList = value; onPropertyChanged("AgeList"); }
        }


        private int selectedAge = 15; 
        public int SelectedAge
        {
            get { return selectedAge; }
            set { selectedAge = value; onPropertyChanged("SelectedAge"); }
        }
        #endregion

        #region Commands
        #region Add
        public ICommand AddCommand
        {
            get;
            internal set;
        }
        private void CreateAddCommand()
        {
            AddCommand = new RelayCommand(AddExecute);
        }
        public void AddExecute()
        {
            Player p = new Player() { Name = Name, Surname = Surname, Age = SelectedAge, Weight = Weight };

            bool isDuplicate = false;
            foreach(var player in Players)
            {
                if (player.ToString() == p.ToString())
                {
                    isDuplicate = true;
                    break;
                }
            }
            if (isDuplicate == true)
                MessageBox.Show("Nie wolno umieszczac duplikatow! ");
            else if(Name == "Podaj imie" || Surname == "Podaj nazwisko")
                MessageBox.Show("Niedowwolone imie badz nazwisko! ");
            else
                Players.Add(p);
            SavePlayers(@"players.json");
        }
        #endregion
        #region Edit
        public ICommand EditCommand
        {
            get;
            internal set;
        }
        private bool CanExecuteEditCommand()
        {
            return true;
        }
        private void CreateEditCommand()
        {
            EditCommand = new RelayCommand(EditExecute, CanExecuteEditCommand);
        }
        public void EditExecute()
        {
            if (SelectedPlayer != null)
            {
                int id = GetPlayerIdFromString(SelectedPlayer.ToString());
                Players.Insert(id, new Player { Name = Name, Surname = Surname, Age = SelectedAge, Weight = Weight });
                Players.RemoveAt(id+1);
                SavePlayers(@"players.json");
            }
            else
                MessageBox.Show("Prosze wybrac gracza!");

        }
        #endregion
        #region Delete
        public ICommand DeleteCommand
        {
            get;
            internal set;
        }
        private bool CanExecuteDeleteCommand()
        {
            return true; 
        }
        private void CreateDeleteCommand()
        {
            DeleteCommand = new RelayCommand(DeleteExecute, CanExecuteDeleteCommand);
        }
        public void DeleteExecute()
        {
            if (selectedPlayer != null)
                Players.Remove(SelectedPlayer);
            else
                MessageBox.Show("Prosze wybrac gracza!");
            SavePlayers(@"players.json");
        }
        #endregion
        #region Copy
        public ICommand CopyCommand
        {
            get;
            internal set;
        }
        private bool CanExecuteCopyCommand()
        {
            return true;
        }
        private void CreateCopyCommand()
        {
            CopyCommand = new RelayCommand(CopyExecute, CanExecuteCopyCommand);
        }
        public void CopyExecute()
        {
            if (selectedPlayer != null)
            {
                Weight = selectedPlayer.Weight;
                SelectedAge = selectedPlayer.Age;
                Name = selectedPlayer.Name;
                Surname = selectedPlayer.Surname;
            }
        }
        #endregion
        #region Close
        public ICommand CloseCommand
        {
            get;
            internal set;
        }
        private bool CanExecuteCloseCommand()
        {
            return true;
        }
        private void CreateCloseCommand()
        {
            CloseCommand = new RelayCommand(CloseExecute, CanExecuteCloseCommand);
        }
        public void CloseExecute()
        {
            SavePlayers(@"players.json");
        }
        #endregion
        #endregion

        #region weight
        private double weight = 50;
        public double Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                onPropertyChanged(nameof(weight));
            }
        }
        #endregion

        #region name
        private string name = "Podaj imie";
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                onPropertyChanged(nameof(name));
            }
        }
        #endregion

        #region surname
        private string surname = "Podaj nazwisko";
        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                onPropertyChanged(nameof(surname));
            }
        }


        #endregion

        private int GetPlayerIdFromString(string searchFor)
        {
            int id = -1;
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].ToString() == searchFor)
                {
                    id = i;
                    break;
                }
            }
            return id;
        }

        private ObservableCollection<Player> LoadPlayers(string path)
        {
            ObservableCollection<Player> PlayersCollection = new ObservableCollection<Player>();
            if (File.Exists(path))
                PlayersCollection = JsonConvert.DeserializeObject<ObservableCollection<Player>>(File.ReadAllText(path));
            return PlayersCollection;
        }

        public void SavePlayers(string path)
        {
            string save = JsonConvert.SerializeObject(Players);
            File.WriteAllText(path, save);
        }
    }
}
