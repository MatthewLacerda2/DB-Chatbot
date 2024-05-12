using Bogus;
using DB_Chatbot.Server.Models;

namespace DB_Chatbot.Server.Utils;

public class Seeder
{

    public Faker<Models.Person> PersonFaker;
    public Faker<Item> ItemFaker;

    public int bogusSeed = 777;

    public Seeder()
    {
        PersonFaker = GetPersonFaker();
        ItemFaker = GetItemFaker();
    }

    Faker<Models.Person> GetPersonFaker()
    {
        Faker<Models.Person> personFaker = new Faker<Models.Person>()
        .UseSeed(bogusSeed)
        .StrictMode(true)

        .RuleFor(e => e.Id, f => f.Random.Guid().ToString())
        .RuleFor(e => e.FullName, f => f.Person.FullName)
        .RuleFor(e => e.age, f => f.Random.Int(18, 64));

        return personFaker;
    }

    Faker<Item> GetItemFaker()
    {
        Faker<Item> itemFaker = new Faker<Item>()
        .UseSeed(bogusSeed)
        .StrictMode(true)

        .RuleFor(e => e.Id, f => f.Random.Guid().ToString())
        .RuleFor(e => e.PersonId, string.Empty)
        .RuleFor(e => e.Name, f => f.Commerce.Product())
        .RuleFor(e => e.Descricao, f => f.Commerce.ProductDescription())
        .RuleFor(e => e.preco, f => f.Random.Float(20, 200));

        return itemFaker;
    }

    public Models.Person[] GetPeople(int count)
    {
        return PersonFaker.Generate(count).ToArray();
    }

    public Item[] GetItems(int count)
    {
        return ItemFaker.Generate(count).ToArray();
    }
}