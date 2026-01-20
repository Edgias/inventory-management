namespace EdgIQ.Inventory.Domain.Interfaces;

public interface INumberGeneratorService
{
    Task<string> GenerateNextNumberAsync();

}
