using UnityEngine;

[CreateAssetMenu(fileName = "New Character DataBase", menuName = "Characters/Database")]
public class CharacterDataBase : ScriptableObject
{
    [SerializeField] private Character[] characters = new Character[0];

    public Character[] GetAllCharacters() => characters;

    public Character GetCharacterById(int id)
    {
        foreach (Character character in characters)
        {
            if (character.Id == id) return character;
        }
        return null;
    }

}