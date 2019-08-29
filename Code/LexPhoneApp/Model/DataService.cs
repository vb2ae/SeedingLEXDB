using System;
using Lex.Db;
using System.Linq;
using System.Collections.Generic;
using Windows.Storage;
using System.IO;
using LexCoreLib;

namespace LexPhoneApp.Model
{
    public class DataService : IDataService
    {
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
    }
}