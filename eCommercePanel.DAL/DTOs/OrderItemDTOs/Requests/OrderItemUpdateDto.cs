﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.OrderItemDTOs.Requests;

public class OrderItemUpdateDto
{
    public int Id { get; set; } 
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
