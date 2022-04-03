using VaultManager.Models;

namespace VaultManager;

public interface ISerializable<T>
{
    T ToSerializable();
    
    void FromSerializable(T serializable);
}
public interface ISerializableVault : ISerializable<IEnumerable<BaseEntry>>
{
    
}
