using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.Common.Interfaces
{
    // Command без результату
    public interface ICommand : IRequest { }

    // Command з результатом
    public interface ICommand<TResponse> : IRequest<TResponse> { }
}
