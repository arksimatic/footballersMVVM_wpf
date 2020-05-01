using System;
using System.Collections.Generic;
using System.Text;

namespace FootballersMVVM.Model
{
    class AgeIntList
    {
        private static List<int> age = new List<int>();
        public static List<int> Age { 
            get { return age; }
            private set { }
        }
        public AgeIntList()
        {
            for (int i = 15; i <= 60; i++)
            {
                age.Add(i);
            }
        }

    }
}
