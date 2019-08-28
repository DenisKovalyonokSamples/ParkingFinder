using ParkCred.Shared.Entities.SQL;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParkCred.Shared.DataAccess
{
    public class SQLDataManager
    {
        #region Initialization

        static object locker = new object();
        SQLiteConnection _database;

        public SQLDataManager()
        {
            _database = GetDBConnection();

            // create the tables
            _database.CreateTable<User>();
            _database.CreateTable<Parking>();
        }
        public SQLiteConnection GetDBConnection()
        {
            var databaseName = "RYXOLParkCredDB.db3";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var databasePath = Path.Combine(folderPath, databaseName);

            var connection = new SQLiteConnection(databasePath);

            return connection;
        }

        #endregion

        #region User

        public int Register(string username)
        {
            try
            {
                lock (locker)
                {
                    var user = _database.Table<User>().FirstOrDefault();
                    if (user != null)
                        ClearUsers();

                    var entity = new User();
                    entity.Name = username;
                    entity.IsActiveSession = false;

                    SaveUser(entity);

                    return user.Id;
                }
            }
            catch (Exception ex)
            {
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite Register ERROR: " + ex.ToString());

                return 0;
            }
        }

        public User GetUser()
        {
            try
            {
                lock (locker)
                {
                    return _database.Table<User>().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite GetUser ERROR: " + ex.ToString());

                return null;
            }
        }

        public int SaveUser(User value)
        {
            try
            {
                lock (locker)
                {
                    if (value.Id != 0)
                    {
                        _database.Update(value);
                        return value.Id;
                    }
                    else
                    {
                        return _database.Insert(value);
                    }
                }
            }
            catch (Exception ex)
            {
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite SaveUser ERROR: " + ex.ToString());

                return 0;
            }
        }

        public void ClearUsers()
        {
            try
            {
                lock (locker)
                {
                    List<User> data = (from i in _database.Table<User>() select i).ToList();
                    if (null != data && data.Count > 0)
                    {
                        foreach (var entity in data)
                        {
                            _database.Delete<User>(entity.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite ClearUsers ERROR: " + ex.ToString());
            }
        }

        #endregion

        #region Parking

        public List<Parking> GetParkings()
        {
            try
            {
                lock (locker)
                {
                    return _database.Table<Parking>().ToList();
                }
            }
            catch (Exception ex)
            {
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite GetUser ERROR: " + ex.ToString());

                return null;
            }
        }

        public int SaveParking(Parking value)
        {
            try
            {
                lock (locker)
                {
                    if (value.Id != 0)
                    {
                        _database.Update(value);
                        return value.Id;
                    }
                    else
                    {
                        return _database.Insert(value);
                    }
                }
            }
            catch (Exception ex)
            {
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite SaveUser ERROR: " + ex.ToString());

                return 0;
            }
        }

        public void ClearParkings()
        {
            try
            {
                lock (locker)
                {
                    List<Parking> data = (from i in _database.Table<Parking>() select i).ToList();
                    if (null != data && data.Count > 0)
                    {
                        foreach (var entity in data)
                        {
                            _database.Delete<Parking>(entity.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite ClearUsers ERROR: " + ex.ToString());
            }
        }

        #endregion
    }
}
