﻿namespace spl.Model
{
    public class Cawangan
    {
        public int? Id { get; set; }
        public String NamaCawangan { get; set; }
        public int? IdBahagian { get; set; }

        public Bahagian? Bahagian { get; set; }
    }
}
