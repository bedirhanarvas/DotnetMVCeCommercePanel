﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.CategoryDTOs.Requests;

public class CategoryCreateDto
{
    [Required]
    public string CategoryName { get; set; }

    public string? Description { get; set; }
}
