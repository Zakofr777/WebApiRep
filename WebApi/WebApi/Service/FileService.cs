using System.Text.Json;

namespace WebApi.Services;

public class FileService
{
    private readonly string _filePath = "respondents.json";

    public List<Person> GetAll()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Person>();
        }

        try
        {
            var jsonString = File.ReadAllText(_filePath);
            
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return new List<Person>();
            }

            return JsonSerializer.Deserialize<List<Person>>(jsonString) ?? new List<Person>();
        }
        catch
        {
            return new List<Person>();
        }
    }

    private void SaveAll(List<Person> people)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonString = JsonSerializer.Serialize(people, options);
        File.WriteAllText(_filePath, jsonString);
    }

    public List<Person> Add(Person person)
    {
        var people = GetAll();
        people.Add(person);
        SaveAll(people);
        return people;
    }

    public Person? GetByIndex(int index)
    {
        var people = GetAll();
        if (index < 0 || index >= people.Count)
        {
            return null; 
        }
        return people[index];
    }

    public List<Person>? DeleteByIndex(int index)
    {
        var people = GetAll();
        if (index < 0 || index >= people.Count)
        {
            return null; 
        }
        
        people.RemoveAt(index);
        SaveAll(people);
        return people;
    }

    public List<Person>? UpdateByIndex(int index, Person updatedPerson)
    {
        var people = GetAll();
        if (index < 0 || index >= people.Count)
        {
            return null; 
        }

        people[index] = updatedPerson;
        SaveAll(people);
        return people;
    }
}