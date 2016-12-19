using System;
using System.Collections.Generic;
using Starcounter;

namespace KitchenSink
{
    [Database]
    public class Person
    {
        public string Name;
        public decimal Order;

        public static explicit operator Person(SortableListPage.PeopleElementJson v)
        {
            throw new NotImplementedException();
        }
    }

    partial class SortableListPage : Json
    {
        protected override void OnData()
        {
            base.OnData();

            Db.Transact(() =>
            {
                /* 
                 * Here initial data is added do the database 
                 */
                Db.SlowSQL("DELETE FROM Person");

                var anyone = Db.SQL<Person>("SELECT p FROM Person p").First;
                if (anyone == null)
                {
                    var Person1 = new Person
                    {
                        Name = "John Doe",
                        Order = 1,
                    };

                    var Person2 = new Person
                    {
                        Name = "Tony Hawk",
                        Order = 2
                    };

                    var Person3 = new Person
                    {
                        Name = "Sir Richard Branson",
                        Order = 3
                    };
                }
            });


            /*
             * here i dont know why "SELECT p FROM Person p ORDER BY p.Order ASC" isn't working...
             */
            var result = Db.SQL<Person>("SELECT p FROM Person p");

            //so i had to sort People in other way
            List<Person> sortedPeople = new List<Person>();
            foreach (var person in result)
            {
                Person p = (Person)person;
                sortedPeople.Add(p);
            }

            sortedPeople.Sort(delegate (Person x, Person y)
            {
                if (x.Order == y.Order) return 0;
                else if (x.Order > y.Order) return 1;
                else if (x.Order < y.Order) return -1;
                else return 0;
            });

            foreach (var person in sortedPeople)
            {
                SortableListPage.PeopleElementJson pet;
                pet = this.People.Add();
                pet.Name = person.Name;
                pet.Order = (long)person.Order;
            }

            /*
             * Here we are disabling displaying first button for up and the last button for down
             */
            var i = 0;
            foreach (var person in this.People)
            {
                if (i == 0) person.UPVisible = false;
                if (this.People.Count == i + 1) person.DOWNVisible = false;
                i++;
            }
        }

        void Handle(Input.ChangeOrderUp Action)
        {
            this.StoreOrder("UP");
        }

        void Handle(Input.ChangeOrderDown Action)
        {
             this.StoreOrder("DOWN");
        }

        public void StoreOrder(string query)
        {
            Db.Transact(() =>
            {
                if (query == "UP")
                {
                  /*
                    I dont know which element was clicked...
                    so i dont know how to update appropriate Person and Person who has lower Order parameter in database
                    */
                }
                else if (query == "DOWN")
                {

                }
            });
        }
    }
}
