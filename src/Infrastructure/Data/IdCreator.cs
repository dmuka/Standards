using Core;

namespace Infrastructure.Data;

public class IdCreator : IIdCreator
{
    public Guid CreateId()
    {
        return Guid.CreateVersion7();
    }
}