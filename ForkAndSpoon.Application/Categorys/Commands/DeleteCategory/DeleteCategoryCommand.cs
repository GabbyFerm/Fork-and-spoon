﻿using MediatR;

namespace ForkAndSpoon.Application.Categorys.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public int CategoryID { get; }
        public string Role { get; }

        public DeleteCategoryCommand(int categoryId, string role)
        {
            CategoryID = categoryId;
            Role = role;
        }
    }

}