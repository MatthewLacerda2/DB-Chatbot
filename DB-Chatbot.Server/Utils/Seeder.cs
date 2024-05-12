using Bogus;
using DB_Chatbot.Server.Models;

namespace DB_Chatbot.Server.Utils;

public class Seeder
{

    public Faker<Pessoa> PessoaFaker;
    public Faker<Item> ItemFaker;

    public int bogusSeed = 777;

    public Seeder()
    {
        PessoaFaker = GetPessoaFaker();
        ItemFaker = GetItemFaker();
    }

    Faker<Pessoa> GetPessoaFaker()
    {
        Faker<Pessoa> pessoaFaker = new Faker<Pessoa>("pt-BR")
        .UseSeed(bogusSeed)
        .StrictMode(true)

        .RuleFor(e => e.Id, f => f.Random.Guid().ToString())
        .RuleFor(e => e.FullName, f => f.Person.FullName)
        .RuleFor(e => e.age, f => f.Random.Int(18, 64));

        return pessoaFaker;
    }

    Faker<Item> GetItemFaker()
    {
        Faker<Item> itemFaker = new Faker<Item>("pt-BR")
        .UseSeed(bogusSeed)
        .StrictMode(true)

        .RuleFor(e => e.Id, f => f.Random.Guid().ToString())
        .RuleFor(e => e.PessoaId, "")
        .RuleFor(e => e.Name, f => f.Commerce.Product())
        .RuleFor(e => e.Descricao, f => f.Commerce.ProductDescription())
        .RuleFor(e => e.Preço, f => f.Random.Float(20, 200));

        return itemFaker;
    }

    public Pessoa[] GetPessoas(int count)
    {
        return PessoaFaker.Generate(count).ToArray();
    }

    public Item[] GetItems(int count)
    {
        return ItemFaker.Generate(count).ToArray();
    }
}