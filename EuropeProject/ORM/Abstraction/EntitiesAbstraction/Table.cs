namespace ORM
{
    internal class Table<T> where T : Entity
    {
        public List<T> Items { get; set; }
        public int IDCounter { get; set; }
    }
}
