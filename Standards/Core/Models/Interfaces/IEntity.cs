using System.ComponentModel.DataAnnotations;
using Standards.Core.Constants;

namespace Standards.Core.Models.Interfaces
{
    public interface IEntity<out TId>
    {
        TId Id { get; }
    }

    public interface IEntity : IEntity<int>
    {
    }
}
