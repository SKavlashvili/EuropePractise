using System.Reflection;
using System.Text.Json;

namespace ORM
{
    public class DbContext
    {
        private string _tablesPath;
        private Assembly _entitiesAssembly;

        /*
         * EntitiesAssembly-should take project where entities are located
         * TablesPath-Should take path where tables.txt files should be created
         */
        public DbContext(string TablesPath, Assembly EntitiesAssembly)
        {
            _tablesPath = TablesPath;
            _entitiesAssembly = EntitiesAssembly;
            ConfigureTables(TablesPath,EntitiesAssembly);
        }

        private string GetTablePath<T>() where T : Entity
        {
            return _tablesPath + typeof(T).Name + "s.txt";
        }



        private Table<T>? GetTableIns<T>() where T : Entity
        {
            return JsonSerializer.Deserialize<Table<T>>(File.ReadAllText(GetTablePath<T>()));
        }


        //Returns T typed table
        public List<T>? GetTable<T>() where T : Entity
        {
            return GetTableIns<T>().Items;
        }

        public bool Exists<T>(Func<T,bool> predicate) where T : Entity
        {
            List<T> Table = GetTable<T>();
            if (Table == null) return false;
            for(int i = 0; i < Table.Count; i++)
            {
                if (predicate.Invoke(Table[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /*
         Returns id of added data element
         */
        public int Add<T>(T data) where T : Entity
        {
            Table<T> Entities = GetTableIns<T>();
            if (Entities.Items == null)
            {
                Entities.Items = new List<T>();
            }
            Entities.Items.Add(data);
            data.ID = Entities.IDCounter;
            Entities.IDCounter++;
            File.WriteAllText(GetTablePath<T>(), JsonSerializer.Serialize(Entities, new JsonSerializerOptions() { WriteIndented = true}));
            return data.ID;
        }
        
        public void Remove<T>(Func<T,bool> where) where T : Entity
        {
            Table<T> Entities = GetTableIns<T>();
            for(int i = 0; i < Entities.Items.Count; i++)
            {
                if (where.Invoke(Entities.Items[i]))
                {
                    Entities.Items.RemoveAt(i);
                }
            }
            File.WriteAllText(GetTablePath<T>(), JsonSerializer.Serialize(Entities, new JsonSerializerOptions() { WriteIndented = true }));
        }

        #region TableConfiguration
        /*
         This static part configures Tables. it creates Tables on TablesPath folder if there are
         not created this type of tables. it runs only ones during program, because of _isConfigured 
         boolean vairable
         */
        private static bool _isConfigured = false;
        public static void ConfigureTables(string TablesPath, Assembly EntitiesAssembly)
        {
            if (!_isConfigured)
            {
                Type[] entities = GetAllEntitiesFromAssembly(EntitiesAssembly);
                for (int i = 0; i < entities.Length; i++)//In this loop we create all of the tables as txt files
                {
                    if (!File.Exists(TablesPath + entities[i].Name + "s.txt"))
                    {
                        File.Create(TablesPath + entities[i].Name + "s.txt").Close();
                        File.AppendAllText(TablesPath + entities[i].Name + "s.txt", 
                 JsonSerializer.Serialize(new Table<Entity>(), new JsonSerializerOptions() { WriteIndented = true}));
                    }
                }
                _isConfigured = true;
            }
        }

        /*
         This method gets all of the classes where base class is abstract entity
         */
        private static Type[] GetAllEntitiesFromAssembly(Assembly EntitiesAssembly)
        {
            return EntitiesAssembly.GetTypes().Where(e => e.BaseType == typeof(Entity)).ToArray();
        }
        #endregion
    }
}
