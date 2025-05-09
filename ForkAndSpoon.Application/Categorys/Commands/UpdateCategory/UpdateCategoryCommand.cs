﻿using ForkAndSpoon.Application.Categorys.DTOs;
using ForkAndSpoon.Domain.Models;
using MediatR;

namespace ForkAndSpoon.Application.Categorys.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<OperationResult<CategoryDto>>
    {
        public int CategoryID { get; }
        public string Name { get; }
        public string Role { get; }

        public UpdateCategoryCommand(int categoryId, string name, string role)
        {
            CategoryID = categoryId;
            Name = name;
            Role = role;
        }
    }
}