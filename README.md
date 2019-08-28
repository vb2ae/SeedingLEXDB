# Seeding a Lex.DB database for a phone app


# Introduction
There are several ways to store data for a windows phone app.  You can use a xml or text file to store the data.  You can use a Sqlite database or an object database.   One advantage of using a Lex.DB is you do not need to compile the application for multiple platforms.  You can comple for any cpu.  

 

So how does an object database compare to a relation database like SQLite.  The biggest difference is there are not tables. Everything is stored in objects that are serialized into json and stored in a file.  

 

 Lex.DB is a nice way to store data on a windows phone.  This sample will show how to populate a lex.db and include the data files in a windows phone app.

# Building the Sample
This sample was built with Visual Studio 2013 but it will work with Visual Stdio 2012.  I have not tested it but it should work just as well with phone express.

# Description

With this sample we will store data in an Lex.DB (object database) and included it with a windows phone 8 app.  This solution will include 3 apps a console app for creating the database, a portable class library which holds the class we are storing and a windows phone app. First create a portable class library and add a person class to it.  I am using a portable class library so the code can be used with a windows, phone, or store app.

# C#
```c#
    public class Person 
    { 
        public int Id { get; set; } 
 
        public string FirstName { get; set; } 
 
        public string LastName { get; set; } 
 
    }
```

 Second add a console app.  To this console app add the nuget package Lex.DB.  You also need to add a reference to the portable library.  To add data to the database we need to create a database instance and then add data to it.

# C#
```c#
            using (var db = new DbInstance("demo")) 
            { 
                db.Map<Person>().Automap(i => i.Id); 
                db.Initialize(); 
                db.Save(new Person() { FirstName = "Ken", Id = 1, LastName = "Tucker" },  
                    new Person() { FirstName = "Tony", Id = 2, LastName = "Stark" }, 
                    new Person() { FirstName = "John", Id = 3,LastName = "Papa" }); 
            }
```

 Run the console app and open up windows explorer and navigate to c:\users\your user name\app data\lex.db and you will find a folder demo or what ever you named your database instance.  Copy the 2 files into the windows phone project.  You need to set the files property to make them content.  With the files included with the project we will copy the 2 files to isolated storage so they can be used as the database files.   When the app first starts we will check and see if there is any data in it.   If there is not we will copy the 2 files to the isolated storage on the phone.
 
 # C#
```c#
       public async void GetData(Action<DataItem, Exception> callback) 
        { 
            // Use this to connect to the actual data service 
            List<Person> list; 
            var item = new DataItem("Welcome to MVVM Light"); 
            using (var db = new DbInstance("demo")) 
            { 
                db.Map<Person>().Automap(i => i.Id); 
                db.Initialize(); 
                list = db.LoadAll<Person>().ToList(); 
                item.People = list; 
            } 
 
            if (list.Count == 0) 
            { 
                var lex = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Lex.Db", CreationCollisionOption.OpenIfExists); 
                var auto = await lex.CreateFolderAsync("demo", CreationCollisionOption.OpenIfExists); 
                StorageFile seedFile1 = await StorageFile.GetFileFromPathAsync(Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, @"Person.index")); 
                StorageFile seedFile2 = await StorageFile.GetFileFromPathAsync(Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, @"Person.data")); 
                await seedFile1.CopyAsync(auto, @"Person.index", NameCollisionOption.ReplaceExisting); 
                await seedFile2.CopyAsync(auto, @"Person.data", NameCollisionOption.ReplaceExisting); 
                using (var db = new DbInstance("demo")) 
                { 
                    db.Map<Person>().Automap(i => i.Id); 
                    db.Initialize(); 
                    list = db.LoadAll<Person>().ToList(); 
                    item.People = list; 
                } 
            } 
 
            callback(item, null); 
        }
```
  
