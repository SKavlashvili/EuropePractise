﻿using ORM;


namespace MainProject.Entities
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Password { get; set; }

    }
}
