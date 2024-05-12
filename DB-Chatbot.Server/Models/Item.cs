namespace DB_Chatbot.Server.Models;

public class Item
{
    public string Id { get; set; } = string.Empty;
    public string PessoaId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public float Preço { get; set; } = 20;
}