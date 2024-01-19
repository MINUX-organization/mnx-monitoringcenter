namespace MINUX.Backend.Worker.Core;

public class Pool
{
    public Guid Id { get; set; }

    public string Host {  get; set; }

    public int Port { get; set; }

    public Guid CryptocurrencyId { get; set; }
}