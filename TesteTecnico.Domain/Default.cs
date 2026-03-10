namespace TesteTecnico.Domain;

public abstract class Default
{
    public int Id { get; set; }
    
    public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;
    public DateTime? DataAlteracao { get; private set; }

    public void SetUpdated()
    {
        DataAlteracao = DateTime.UtcNow;
    }
}