﻿namespace spl.Model
{
    public class Stesen
    {
        public int Id { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
