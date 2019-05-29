﻿using System;

namespace ECommerce.ProductCatalog.Models
{
    public class Product    
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Availability { get; set; }
    }
}
