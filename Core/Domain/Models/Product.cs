﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Product : BaseEntity<int>
    {
        public string Description { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }

        #region Foreign Key
        public int BrandId { get; set; }
        public int TypeId { get; set; }
        #endregion

        #region Navigational Property
        public ProductBrand ProductBrand { get; set; }
        public ProductType ProductType { get; set; }
        #endregion
    }
}
